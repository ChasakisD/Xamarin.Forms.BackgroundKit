using Android.Graphics;
using Xamarin.Forms.Platform.Android;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Extensions;
using AColor = Android.Graphics.Color;

namespace XamarinBackgroundKit.Android.GradientProviders
{
    public class LinearGradientProvider : BaseGradientProvider<LinearGradientBrush>
    {
        private int[] _colors;
        private float[] _positions;
        private float[] _colorPositions;

        public override bool HasGradient { get; protected set; }
        
        public override void OnGradientSet(LinearGradientBrush gradientBrush)
        {
            if (gradientBrush == null || gradientBrush.Gradients == null || gradientBrush.Gradients.Count < 2)
            {
                _colors = null;
                _positions = null;
                _colorPositions = null;

                HasGradient = false;
                return;
            }

            HasGradient = true;

            var positions = gradientBrush.Angle.ToStartEndPoint();

            for (var i = 0; i < positions.Length; i++)
            {
                if (!(positions[i] > 1)) continue;
                positions[i] = 1;
            }

            _positions = positions;
            _colors = new int[gradientBrush.Gradients.Count];
            _colorPositions = new float[gradientBrush.Gradients.Count];
            for (var i = 0; i < gradientBrush.Gradients.Count; i++)
            {
                _colors[i] = gradientBrush.Gradients[i].Color.ToAndroid();
                _colorPositions[i] = gradientBrush.Gradients[i].Offset;
            }
        }

        public override void DrawGradient(Paint paint, int width, int height)
        {
            if (!HasGradient) return;

            /* Color of paint will be ignored */
            paint.Color = AColor.White;
            paint.SetShader(new LinearGradient(
                width * _positions[0],
                height * _positions[1],
                width * _positions[2],
                height * _positions[3],
                _colors,
                _colorPositions,
                Shader.TileMode.Clamp));
        }

        public override void ClearGradient(Paint paint, int width, int height)
        {
            InvalidateArrays();

            paint.SetShader(null);
        }

        public override void Dispose(bool disposing)
        {
            InvalidateArrays();
        }

        private void InvalidateArrays()
        {
            _colors = null;
            _positions = null;
            _colorPositions = null;
        }
    }
}
