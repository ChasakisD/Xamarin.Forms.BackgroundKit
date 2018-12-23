using Xamarin.Forms;

namespace XamarinBackgroundKit.Controls
{
    /// <inheritdoc />
    /// <summary>
    /// Creates a GradientStop for the Gradient
    /// </summary>
	public class GradientStop : BindableObject
	{
		public static readonly BindableProperty ColorProperty = BindableProperty.Create(
			nameof(Color), typeof(Color), typeof(GradientStop), Color.White);

        /// <summary>
        /// Gets or sets the Color of the GradientStop
        /// </summary>
		public Color Color
		{
			get => (Color)GetValue(ColorProperty);
			set => SetValue(ColorProperty, value);
		}

		public static readonly BindableProperty OffsetProperty = BindableProperty.Create(
			nameof(Offset), typeof(float), typeof(GradientStop), 0f);

        /// <summary>
        /// Gets or sets the Offset of the GradientStop. Must in [0,1]
        /// </summary>
		public float Offset
		{
			get => (float)GetValue(OffsetProperty);
			set => SetValue(OffsetProperty, value);
		}

        /// <summary>
        /// Creates a new GradientStop
        /// </summary>
		public GradientStop() { }

        /// <summary>
        /// Creates a new GradientStop
        /// </summary>
        /// <param name="color">The Color of the GradientStop</param>
        /// <param name="offset">The Offset of the GradientStop. Must in [0,1]</param>
		public GradientStop(Color color, float offset)
		{
			Color = color;
			Offset = offset;
		}

        /// <summary>
        /// Whether or not two GradientStops are equal
        /// </summary>
        /// <param name="obj">The second GradientStop</param>
        /// <returns></returns>
		public override bool Equals(object obj)
		{
			if (!(obj is GradientStop dest)) return false;

			return Color == dest.Color && System.Math.Abs(Offset - dest.Offset) < 0.00001;
		}

		public override int GetHashCode()
		{
			return -1200350280 + Color.GetHashCode();
		}
	}
}