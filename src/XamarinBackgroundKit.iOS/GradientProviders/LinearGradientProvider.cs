using System;
using CoreGraphics;
using Xamarin.Forms.Platform.iOS;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Extensions;

namespace XamarinBackgroundKit.iOS.GradientProviders
{
    public class LinearGradientProvider : BaseGradientProvider<LinearGradientBrush>
    {
        private CGColor[] _colors;
        private float[] _positions;
        private nfloat[] _colorPositions;

        public override bool HasGradient { get; protected set; }

        public override void OnGradientSet(LinearGradientBrush gradientBrush)
        {
            if (gradientBrush == null || gradientBrush == null || gradientBrush.Gradients.Count < 2)
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
            _colors = new CGColor[gradientBrush.Gradients.Count];
            _colorPositions = new nfloat[gradientBrush.Gradients.Count];
            for (var i = 0; i < gradientBrush.Gradients.Count; i++)
            {
                _colors[i] = gradientBrush.Gradients[i].Color.ToCGColor();
                _colorPositions[i] = gradientBrush.Gradients[i].Offset;
            }
        }

        public override void DrawGradient(CGContext ctx, CGRect bounds)
        {
            if (!HasGradient) return;

            var startPoint = new CGPoint(_positions[0] * bounds.Width, _positions[1] * bounds.Height);
            var endPoint = new CGPoint(_positions[2] * bounds.Width, _positions[3] * bounds.Height);

            var gradient = new CGGradient(CGColorSpace.CreateDeviceRGB(), _colors, _colorPositions);

            ctx.DrawLinearGradient(gradient, startPoint, endPoint, CGGradientDrawingOptions.None);
        }

        public override void ClearGradient(CGContext ctx, CGRect bounds)
        {
            InvalidateArrays();
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
