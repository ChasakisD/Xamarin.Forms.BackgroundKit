using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Extensions;
using AColor = Android.Graphics.Color;
using Color = Xamarin.Forms.Color;

namespace XamarinBackgroundKit.Android.Renderers
{
    public class GradientStrokeDrawable : PaintDrawable
    {
        private readonly double _density;

        private Path _clipPath;
        private Paint _clipPaint;
        private Paint _emptyPaint;
        private Paint _strokePaint;
        private Bitmap _maskBitmap;
        private Bitmap _tempBitmap;
        private Canvas _maskCanvas;
        private Canvas _tempCanvas;

        private Color _color;
        private Color _strokeColor;
        private CornerRadius _cornerRadius;

        private int[] _colors;
        private float[] _positions;
        private float[] _colorPositions;

        private int[] _strokeColors;
        private float[] _strokePositions;
        private float[] _strokeColorPositions;

        public GradientStrokeDrawable(double density, IMaterialVisualElement background = null)
        {
            Initialize();

            _density = density;

            if (background == null) return;
            SetColor(background.Color);
            SetCornerRadius(background.CornerRadius);
            SetStroke(background.BorderWidth, background.BorderColor);
            SetDashedStroke(background.DashWidth, background.DashGap);
            SetGradient(background.GradientBrush?.Gradients, background.GradientBrush?.Angle ?? 0);
            SetBorderGradient(background.BorderGradientBrush?.Gradients, background.BorderGradientBrush?.Angle ?? 0);
        }

        private void Initialize()
        {
            Shape = new RectShape();

            _clipPath = new Path();

            _clipPaint = new Paint(PaintFlags.AntiAlias);
            _emptyPaint = new Paint(PaintFlags.AntiAlias);
            _strokePaint = new Paint(PaintFlags.AntiAlias);
            _strokePaint.SetStyle(Paint.Style.Stroke);
        }

        #region Actual Draw

        protected override void OnDraw(Shape shape, Canvas canvas, Paint paint)
        {
            InitializePaints(canvas);
            InitializeClippingPath(canvas);

            /*
             * On Android Pie when drawing stroke via DrawPath()
             * after clipping the mask, when CornerRadius was set,
             * the outer stroke was not clipped.
             *
             * By drawing on a bitmap and then draw that bitmap
             * inside the clipped canvas outer stroke was clipped!
             */
            DrawApi28(canvas);
        }

        private void DrawApi28(Canvas canvas)
        {
            var width = Bounds.Width();
            var height = Bounds.Height();

            if (width <= 0 || height <= 0) return;

            EnsureInitialized(width, height);

            /*
             * Masking the canvas is anti-aliased and also is more
             * efficient than clipping the canvas on android.
             *
             * The masking is performed on a temp canvas that is later
             * draw on the original one. We draw the background and the
             * border paints. Then we mask the canvas by DstIn, order
             * to clear the outer stroke.
             */
            _clipPaint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.DstIn));

            _maskCanvas.DrawColor(AColor.Transparent, PorterDuff.Mode.Multiply);
            _maskCanvas.DrawPath(_clipPath, _emptyPaint);

            _tempCanvas.DrawColor(AColor.Transparent, PorterDuff.Mode.Multiply);
            _tempCanvas.DrawPath(_clipPath, Paint);
            if (HasBorder())
            {
                _tempCanvas.DrawPath(_clipPath, _strokePaint);
            }
            _tempCanvas.DrawBitmap(_maskBitmap, 0, 0, _clipPaint);

            canvas.DrawBitmap(_tempBitmap, 0, 0, _emptyPaint);

            _clipPaint.SetXfermode(null);
        }

        #endregion

        #region Draw Helpers

        private bool HasBorder()
        {
            return _strokePaint.StrokeWidth > 0;
        }

        private bool HasGradient()
        {
            return _colors != null && _positions != null && _colorPositions != null;
        }

        private bool HasBorderGradient()
        {
            return _strokeColors != null && _strokePositions != null && _strokeColorPositions != null;
        }

        private void EnsureInitialized(int width, int height)
        {
            if (_maskCanvas != null
                && _maskCanvas.Width == width
                && _maskCanvas.Height == height) return;

            using (var config = Bitmap.Config.Argb8888)
            {
                _tempBitmap?.Dispose();
                _tempBitmap = Bitmap.CreateBitmap(width, height, config);
                _tempCanvas?.Dispose();
                _tempCanvas = new Canvas(_tempBitmap);

                _maskBitmap?.Dispose();
                _maskBitmap = Bitmap.CreateBitmap(width, height, config);
                _maskCanvas?.Dispose();
                _maskCanvas = new Canvas(_maskBitmap);
            }
        }

        private void InitializePaints(Canvas canvas)
        {
            if (HasGradient())
            {
                /* Color of paint will be ignored */
                Paint.Color = AColor.White;
                Paint.SetShader(new LinearGradient(
                    canvas.Width * _positions[0],
                    canvas.Height * _positions[1],
                    canvas.Width * _positions[2],
                    canvas.Height * _positions[3],
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

            if (HasBorderGradient())
            {
                _strokePaint.Color = AColor.White;
                _strokePaint.SetShader(new LinearGradient(
                    canvas.Width * _strokePositions[0],
                    canvas.Height * _strokePositions[1],
                    canvas.Width * _strokePositions[2],
                    canvas.Height * _strokePositions[3],
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

        private void InitializeClippingPath(Canvas canvas)
        {
            var cornerRadii = _cornerRadius.ToRadii(_density);

            _clipPath.Reset();
            _clipPath.AddRoundRect(0, 0, canvas.Width, canvas.Height,
                cornerRadii, Path.Direction.Cw);
        }

        #endregion

        #region Public Setters

        public void SetColor(Color color)
        {
            _color = color;

            InvalidateSelf();
        }

        public void SetStroke(double strokeWidth, Color strokeColor)
        {
            _strokeColor = strokeColor;
            _strokePaint.StrokeWidth = (int)(strokeWidth * _density * 2);

            InvalidateSelf();
        }

        public void SetDashedStroke(double dashWidth, double dashGap)
        {
            if (dashWidth <= 0 || dashGap <= 0)
            {
                _strokePaint.SetPathEffect(null);
            }
            else
            {
                _strokePaint.SetPathEffect(new DashPathEffect(new float[]
                {
                    (int) (dashWidth * _density),
                    (int) (dashGap * _density)
                }, 0));
            }

            InvalidateSelf();
        }

        public void SetBorderGradient(IList<GradientStop> gradients, float angle)
        {
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

        public void SetCornerRadius(CornerRadius cornerRadius)
        {
            _cornerRadius = cornerRadius;

            var isUniform = _cornerRadius.IsAllRadius() && !_cornerRadius.IsEmpty();

            var uniformCornerRadius = (int)(_cornerRadius.TopLeft * _density);
            var cornerRadii = _cornerRadius.ToRadii(_density);

            if (isUniform) base.SetCornerRadius(uniformCornerRadius);
            else SetCornerRadii(cornerRadii);

            InvalidateSelf();
        }

        #endregion

        #region LifeCycle

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_clipPath != null)
                {
                    _clipPath.Dispose();
                    _clipPath = null;
                }

                if (_clipPaint != null)
                {
                    _clipPaint.Dispose();
                    _clipPaint = null;
                }

                if (_emptyPaint != null)
                {
                    _emptyPaint.Dispose();
                    _emptyPaint = null;
                }

                if (_strokePaint != null)
                {
                    _strokePaint.Dispose();
                    _strokePaint = null;
                }

                if (_tempBitmap != null)
                {
                    _tempBitmap.Dispose();
                    _tempBitmap = null;
                }

                if (_tempCanvas != null)
                {
                    _tempCanvas.Dispose();
                    _tempCanvas = null;
                }

                if (_maskBitmap != null)
                {
                    _maskBitmap.Dispose();
                    _maskBitmap = null;
                }

                if (_maskCanvas != null)
                {
                    _maskCanvas.Dispose();
                    _maskCanvas = null;
                }

                _colors = null;
                _positions = null;
                _colorPositions = null;

                _strokeColors = null;
                _strokePositions = null;
                _strokeColorPositions = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}