using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Extensions;
using Color = Xamarin.Forms.Color;
using AColor = Android.Graphics.Color;

namespace XamarinBackgroundKit.Android.Renderers
{
    public class GradientStrokeDrawable : PaintDrawable
    {
        private Context _context;

        private Path _clipPath;
        private Paint _clipPaint;
        private Paint _emptyPaint;
        private Paint _strokePaint;
        private Bitmap _maskBitmap;
        private Bitmap _tempBitmap;
        private Canvas _maskCanvas;
        private Canvas _tempCanvas;

        private CornerRadius _cornerRadius;

        private int[] _colors;
        private float[] _positions;
        private float[] _colorPositions;

        private int[] _strokeColors;
        private float[] _strokePositions;
        private float[] _strokeColorPositions;

        public GradientStrokeDrawable(Context context, IMaterialVisualElement background = null)
        {
            Initialize();

            _context = context;

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

        public void SetColor(Color color)
        {
            if (color == Color.Default) return;

            Paint.Color = color.ToAndroid();
            
            InvalidateSelf();
        }

        public void SetStroke(double strokeWidth, Color strokeColor)
        {
            _strokePaint.StrokeWidth = (int) _context.ToPixels(strokeWidth) * 2;

            if (_strokeColors == null)
            {
                _strokePaint.Color = strokeColor.ToAndroid();
            }

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
                    (int) _context.ToPixels(dashWidth), 
                    (int) _context.ToPixels(dashGap)
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
            _strokeColors = gradients.Select(x => (int) x.Color.ToAndroid()).ToArray();
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

            var uniformCornerRadius = _context.ToPixels(_cornerRadius.TopLeft);
            var cornerRadii = _cornerRadius.ToRadii(_context.Resources.DisplayMetrics.Density);

            if (isUniform) base.SetCornerRadius(uniformCornerRadius);
            else SetCornerRadii(cornerRadii);

            InvalidateSelf();
        }
        
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
            if (_colors != null && _positions != null && _colorPositions != null)
            {
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
            }

            if (_strokeColors != null && _strokePositions != null && _strokeColorPositions != null)
            {
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
            }
        }

        private void InitializeClippingPath(Canvas canvas)
        {
            var cornerRadii = _cornerRadius.ToRadii(_context.Resources.DisplayMetrics.Density);

            _clipPath.Reset();
            _clipPath.AddRoundRect(0, 0, canvas.Width, canvas.Height,
                cornerRadii, Path.Direction.Cw);
        }

        private void DrawApi28(Canvas canvas)
        {
            var width = Bounds.Width();
            var height = Bounds.Height();

            if (width <= 0 || height <= 0) return;

            EnsureInitialized(width, height);

            _clipPaint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.DstIn));

            _maskCanvas.DrawColor(AColor.Transparent, PorterDuff.Mode.Multiply);
            _maskCanvas.DrawPath(_clipPath, _emptyPaint);

            _tempCanvas.DrawColor(AColor.Transparent, PorterDuff.Mode.Multiply);
            _tempCanvas.DrawPath(_clipPath, Paint);
            _tempCanvas.DrawPath(_clipPath, _strokePaint);
            _tempCanvas.DrawBitmap(_maskBitmap, 0, 0, _clipPaint);

            canvas.DrawBitmap(_tempBitmap, 0, 0, _emptyPaint);

            _clipPaint.SetXfermode(null);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_clipPath != null)
                {
                    _clipPath.Dispose();
                    _clipPath = null;
                }

                if(_clipPaint != null)
                {
                    _clipPaint.Dispose();
                    _clipPaint = null;
                }

                if(_emptyPaint != null)
                {
                    _emptyPaint.Dispose();
                    _emptyPaint = null;
                }

                if (_strokePaint != null)
                {
                    _strokePaint.Dispose();
                    _strokePaint = null;
                }

                if(_tempBitmap != null)
                {
                    _tempBitmap.Dispose();
                    _tempBitmap = null;
                }

                if(_tempCanvas != null)
                {
                    _tempCanvas.Dispose();
                    _tempCanvas = null;
                }

                if(_maskBitmap != null)
                {
                    _maskBitmap.Dispose();
                    _maskBitmap = null;
                }

                if(_maskCanvas != null)
                {
                    _maskCanvas.Dispose();
                    _maskCanvas = null;
                }

                _context = null;

                _colors = null;
                _positions = null;
                _colorPositions = null;
                
                _strokeColors = null;
                _strokePositions = null;
                _strokeColorPositions = null;
            }

            base.Dispose(disposing);
        }

        public class Builder
        {
            private readonly Context _context;
            private IMaterialVisualElement _materialVisualElement;

            public Builder(Context context)
            {
                _context = context;
            }

            public Builder SetMaterialElement(IMaterialVisualElement materialVisualElement)
            {
                _materialVisualElement = materialVisualElement;
                return this;
            }

            public GradientStrokeDrawable Build()
            {
                return new GradientStrokeDrawable(_context, _materialVisualElement);
            }
        }
    }
}