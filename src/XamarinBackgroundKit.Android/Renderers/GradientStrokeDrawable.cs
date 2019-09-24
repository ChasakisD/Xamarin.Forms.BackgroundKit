using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Xamarin.Forms.Platform.Android;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Android.PathProviders;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Extensions;
using AColor = Android.Graphics.Color;
using Color = Xamarin.Forms.Color;

namespace XamarinBackgroundKit.Android.Renderers
{
    public class GradientStrokeDrawable : ShapeDrawable
    {
        private readonly double _density;

        private bool _dirty;
        private bool _pathDirty;

        private bool _disposed;

        private int _width;
        private int _height;

        private Path _clipPath;
        private Path _maskPath;
        private Paint _maskPaint;
        private Paint _strokePaint;

        private Color _color;
        private Color _strokeColor;

        private int[] _colors;
        private float[] _positions;
        private float[] _colorPositions;

        private float _strokeWidth;
        private PathEffect _strokePathEffect;

        private int[] _strokeColors;
        private float[] _strokePositions;
        private float[] _strokeColorPositions;

        private IPathProvider _pathProvider;

        public GradientStrokeDrawable(IPathProvider pathProvider, IMaterialVisualElement background = null) : base(new RectShape())
        {
            _dirty = true;
            _pathDirty = true;
            _clipPath = new Path();
            _density = BackgroundKit.Density;
            _pathProvider = pathProvider;

            if (background == null) return;
            SetColor(background.Color);
            SetStroke(background.BorderWidth, background.BorderColor);
            SetDashedStroke(background.DashWidth, background.DashGap);
            SetGradient(background.GradientBrush?.Gradients, background.GradientBrush?.Angle ?? 0);
            SetBorderGradient(background.BorderGradientBrush?.Gradients, background.BorderGradientBrush?.Angle ?? 0);
        }

        protected override void OnBoundsChange(Rect bounds)
        {          
            var width = bounds.Width();
            var height = bounds.Height();

            if (_width == width && _height == height) return;

            _dirty = true;
            _pathDirty = true;
            _pathProvider?.Invalidate();

            _width = width;
            _height = height;

            base.OnBoundsChange(bounds);
        }

        #region Actual Draw

        protected override void OnDraw(Shape shape, Canvas canvas, Paint paint)
        {
            if (_disposed) return;

            if (_dirty)
            {
                _dirty = false;
                InitializePaints();
            }

            /* Update the path only if it needs update */
            if (_pathDirty || (_pathProvider != null && _pathProvider.IsPathDirty))
            {
                _pathDirty = false;
                InitializeClippingPath();
            }

            if (CanDrawBorder())
            {
                /*
                 * In order to make the curves smooth, Canvas.ClipPath() method
                 * needs to be avoided. So the only way to make
                 * anti alias clipping is by masking the Canvas.
                 */

                /*
                 * Create a new layer for the clip operation.
                 * After drawing the stroke and clipping the outer stroke,
                 * restore the canvas, in order to merge the layers
                 */
                var saveCount = canvas.SaveLayer(0, 0, _width, _height, null);
                canvas.DrawPath(_clipPath, Paint);
                canvas.DrawPath(_clipPath, _strokePaint);
                canvas.DrawPath(_maskPath, _maskPaint);
                canvas.RestoreToCount(saveCount);
            }
            else
            {
                canvas.DrawPath(_clipPath, Paint);
            }
        }

        #endregion

        #region Draw Helpers

        private bool HasBorder()
        {
            return _strokeWidth > 0;
        }

        private bool HasGradient()
        {
            return _colors != null && _positions != null && _colorPositions != null;
        }

        private bool HasBorderGradient()
        {
            return _strokeColors != null && _strokePositions != null && _strokeColorPositions != null;
        }

        private bool CanDrawBorder()
        {
            EnsureStrokeAlloc();

            return HasBorder();
        }

        private void InitializePaints()
        {
            if (HasGradient())
            {
                /* Color of paint will be ignored */
                Paint.Color = AColor.White;
                Paint.SetShader(new LinearGradient(
                    _width * _positions[0],
                    _height * _positions[1],
                    _width * _positions[2],
                    _height * _positions[3],
                    _colors,
                    _colorPositions,
                    Shader.TileMode.Clamp));
            }
            else
            {
                Paint.SetShader(null);

                if (_color != Color.Default)
                {
                    Paint.Color = _color.ToAndroid();
                }
            }

            if (CanDrawBorder())
            {
                _strokePaint.StrokeWidth = _strokeWidth;
                _strokePaint.SetPathEffect(_strokePathEffect);

                if (HasBorderGradient())
                {
                    _strokePaint.Color = AColor.White;
                    _strokePaint.SetShader(new LinearGradient(
                        _width * _strokePositions[0],
                        _height * _strokePositions[1],
                        _width * _strokePositions[2],
                        _height * _strokePositions[3],
                        _strokeColors,
                        _strokeColorPositions,
                        Shader.TileMode.Clamp));
                }
                else
                {
                    _strokePaint.SetShader(null);

                    if (_strokeColor != Color.Default)
                    {
                        _strokePaint.Color = _strokeColor.ToAndroid();
                    }
                }
            }
        }

        private void InitializeClippingPath()
        {
            if (_pathProvider == null) return;

            var clipPath = _pathProvider.CreatePath(_width, _height);
            if (clipPath == null) return;

            _clipPath.Reset();
            _clipPath.Set(clipPath);

            if (CanDrawBorder())
            {
                _maskPath.Reset();
                _maskPath.AddRect(0, 0, _width, _height, Path.Direction.Cw);
                _maskPath.InvokeOp(_clipPath, Path.Op.Difference);             
            }
        }

        private void EnsureStrokeAlloc()
        {
            if (!HasBorder())
            {
                DisposeBorder(true);
                return;
            }

            if (_maskPath == null)
            {
                _maskPath = new Path();
            }

            if (_maskPaint == null)
            {
                _maskPaint = new Paint(PaintFlags.AntiAlias);
                _maskPaint.SetStyle(Paint.Style.FillAndStroke);
                _maskPaint.SetXfermode(BackgroundKit.PorterDuffClearMode);
            }

            if (_strokePaint == null)
            {
                _strokePaint = new Paint(PaintFlags.AntiAlias);
                _strokePaint.SetStyle(Paint.Style.Stroke);
            }
        }

        #endregion

        #region Public Setters

        public void InvalidatePath()
        {
            _pathDirty = true;
        }

        public void SetPathProvider(IPathProvider pathProvider)
        {
            _pathDirty = true;
            _pathProvider = pathProvider;

            InvalidateSelf();
        }

        public void SetColor(Color color)
        {
            _dirty = true;
            _color = color;

            InvalidateSelf();
        }

        public void SetStroke(double strokeWidth, Color strokeColor)
        {
            _dirty = true;
            _strokeColor = strokeColor;
            _strokeWidth = (float)(strokeWidth * _density * 2);

            EnsureStrokeAlloc();

            InvalidateSelf();
        }

        public void SetDashedStroke(double dashWidth, double dashGap)
        {
            _dirty = true;
            if (dashWidth <= 0 || dashGap <= 0)
            {
                _strokePathEffect = null;
            }
            else
            {
                _strokePathEffect = new DashPathEffect(new float[]
                {
                    (int) (dashWidth * _density),
                    (int) (dashGap * _density)
                }, 0);
            }

            InvalidateSelf();
        }

        public void SetBorderGradient(IList<GradientStop> gradients, float angle)
        {
            _dirty = true;
            if (gradients == null || gradients.Count < 2)
            {
                _strokeColors = null;
                _strokePositions = null;
                _strokeColorPositions = null;

                InvalidateSelf();
                return;
            }

            var positions = angle.ToStartEndPoint();

            for (var i = 0; i < positions.Length; i++)
            {
                if (!(positions[i] > 1)) continue;
                positions[i] = 1;
            }

            _strokePositions = positions;
            _strokeColors = gradients.Select(x => (int)x.Color.ToAndroid()).ToArray();
            _strokeColorPositions = gradients.Select(x => x.Offset).ToArray();

            _strokePaint.Color = Color.White.ToAndroid();

            InvalidateSelf();
        }

        public void SetGradient(IList<GradientStop> gradients, float angle)
        {
            _dirty = true;
            if (gradients == null || gradients.Count < 2)
            {
                _colors = null;
                _positions = null;
                _colorPositions = null;

                InvalidateSelf();
                return;
            }

            var positions = angle.ToStartEndPoint();

            for (var i = 0; i < positions.Length; i++)
            {
                if (!(positions[i] > 1)) continue;
                positions[i] = 1;
            }

            _positions = positions;
            _colors = gradients.Select(x => (int)x.Color.ToAndroid()).ToArray();
            _colorPositions = gradients.Select(x => x.Offset).ToArray();

            InvalidateSelf();
        }

        #endregion

        #region LifeCycle

        protected virtual void DisposeBorder(bool disposing)
        {
            if (disposing)
            {
                if (_maskPath != null)
                {
                    _maskPath.Dispose();
                    _maskPath = null;
                }

                if (_maskPaint != null)
                {
                    _maskPaint.SetXfermode(null);
                    _maskPaint.Dispose();
                    _maskPaint = null;
                }

                if (_strokePaint != null)
                {
                    _strokePaint.Dispose();
                    _strokePaint = null;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;

            _disposed = true;

            if (disposing)
            {
                if (_clipPath != null)
                {
                    _clipPath.Dispose();
                    _clipPath = null;
                }

                if (_strokePathEffect != null)
                {
                    _strokePathEffect.Dispose();
                    _strokePathEffect = null;
                }

                _pathProvider = null;

                _colors = null;
                _positions = null;
                _colorPositions = null;

                _strokeColors = null;
                _strokePositions = null;
                _strokeColorPositions = null;
            }

            DisposeBorder(disposing);

            base.Dispose(disposing);
        }

        #endregion
    }
}