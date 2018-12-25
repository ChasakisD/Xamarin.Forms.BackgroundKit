using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Xamarin.Forms.Platform.Android;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Extensions;
using Color = Xamarin.Forms.Color;

namespace XamarinBackgroundKit.Android.Renderers
{
    public class GradientStrokeDrawable : PaintDrawable
    {
        private Paint _strokePaint;

        public GradientStrokeDrawable()
        {
            Initialize();
        }

        public void Initialize()
        {
            _strokePaint = new Paint
            {
                Dither = true,
                AntiAlias = true
            };
            _strokePaint.SetStyle(Paint.Style.Stroke);
        }

        public void SetStroke(int strokeWidth, Color strokeColor)
        {
            _strokePaint.StrokeWidth = strokeWidth;
            _strokePaint.Color = strokeColor.ToAndroid();
            InvalidateSelf();
        }

        public void SetGradient(IList<GradientStop> gradients, float angle)
        {
            var positions = angle.ToStartEndPoint();

            for (var i = 0; i < positions.Length; i++)
            {
                if (!(positions[i] > 1)) continue;
                positions[i] = 1;
            }

            SetShaderFactory(new GradientShaderFactory
            {
                Positions = positions,
                ColorPositions = gradients.Select(x => x.Offset).ToArray(),
                Colors = gradients.Select(x => (int)x.Color.ToAndroid()).ToArray(),
            });
        }

        protected override void OnDraw(Shape shape, Canvas canvas, Paint paint)
        {
            base.OnDraw(shape, canvas, paint);

            shape.Draw(canvas, _strokePaint);
        }

        private class GradientShaderFactory : ShaderFactory
        {
            public int[] Colors { private get; set; }
            public float[] Positions { private get; set; }
            public float[] ColorPositions { private get; set; }

            public override Shader Resize(int width, int height)
            {
                return new LinearGradient(
                    width * Positions[0],
                    height * Positions[1],
                    width * Positions[2],
                    height * Positions[3],
                    Colors,
                    ColorPositions,
                    Shader.TileMode.Clamp);
            }
        }
    }
}