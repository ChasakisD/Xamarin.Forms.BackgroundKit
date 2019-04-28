using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Controls.Base;
using IBorderElement = XamarinBackgroundKit.Abstractions.IBorderElement;

namespace XamarinBackgroundKit.Controls
{
    public class Background : BindableObject, IMaterialVisualElement
    {
        #region IElevation Properties

        public static readonly BindableProperty ElevationProperty = ElevationElement.ElevationProperty;

        /// <summary>
        /// Gets or sets the Elevation of the Background
        /// </summary>
		public double Elevation
        {
            get => (double)GetValue(ElevationProperty);
            set => SetValue(ElevationProperty, value);
        }

        public static readonly BindableProperty TranslationZProperty = ElevationElement.TranslationZProperty;

        /// <summary>
        /// Gets or sets the TranslationZ of the Background
        /// </summary>
		public double TranslationZ
        {
            get => (double)GetValue(TranslationZProperty);
            set => SetValue(TranslationZProperty, value);
        }

        #endregion

        #region ICornerElement Properties

        public static readonly BindableProperty CornerRadiusProperty = CornerElement.CornerRadiusProperty;

        /// <summary>
        /// Gets or sets the Elevation of the Background
        /// </summary>
		public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion

        #region IGradientElement Properties

        public static readonly BindableProperty GradientBrushProperty = GradientElement.GradientBrushProperty;

        /// <summary>
        /// Gets or sets the Angle of the Gradient of the Background
        /// </summary>
		public LinearGradientBrush GradientBrush
        {
            get => (LinearGradientBrush)GetValue(GradientBrushProperty);
            set => SetValue(GradientBrushProperty, value);
        }

        #endregion

        #region IBorderElement Properties

        public static readonly BindableProperty BorderColorProperty = BorderElement.BorderColorProperty;

        /// <summary>
        /// Gets or sets the Border Color of the Border
        /// </summary>
		public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty BorderWidthProperty = BorderElement.BorderWidthProperty;

        /// <summary>
        /// Gets or sets the Border Width of the Border
        /// </summary>
		public double BorderWidth
        {
            get => (double)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }

        public static readonly BindableProperty DashGapProperty = BorderElement.DashGapProperty;

        /// <summary>
        /// Gets or sets the Dash Gap of the Border
        /// </summary>
        public double DashGap
        {
            get => (double)GetValue(DashGapProperty);
            set => SetValue(DashGapProperty, value);
        }

        public static readonly BindableProperty DashWidthProperty = BorderElement.DashWidthProperty;

        /// <summary>
        /// Gets or sets the Dash Width of the Border
        /// </summary>
        public double DashWidth
        {
            get => (double)GetValue(DashWidthProperty);
            set => SetValue(DashWidthProperty, value);
        }

        public static readonly BindableProperty BorderGradientBrushProperty = BorderElement.BorderGradientBrushProperty;

        /// <summary>
        /// Gets or sets the Angle of the Gradient of the Border
        /// </summary>
        public LinearGradientBrush BorderGradientBrush
        {
            get => (LinearGradientBrush)GetValue(BorderGradientBrushProperty);
            set => SetValue(BorderGradientBrushProperty, value);
        }
        
        #endregion

        public static readonly BindableProperty ColorProperty = BindableProperty.Create(
            nameof(Color), typeof(Color), typeof(Background), Color.Default);

        /// <summary>
        /// Gets or sets the Color of the Background
        /// </summary>
        public Color Color
        {
            get => (Color) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        
        public static readonly BindableProperty IsRippleEnabledProperty = BindableProperty.Create(
            nameof(IsRippleEnabled), typeof(bool), typeof(Background), false);

        /// <summary>
        /// Gets or sets the Ripple of the Background
        /// </summary>
        public bool IsRippleEnabled
        {
            get => (bool) GetValue(IsRippleEnabledProperty);
            set => SetValue(IsRippleEnabledProperty, value);
        }
        
        public static readonly BindableProperty RippleColorProperty = BindableProperty.Create(
            nameof(RippleColor), typeof(Color), typeof(Background), Color.Default);

        /// <summary>
        /// Gets or sets the Ripple Color of the Background
        /// </summary>
        public Color RippleColor
        {
            get => (Color) GetValue(RippleColorProperty);
            set => SetValue(RippleColorProperty, value);
        }

        #region IElevation Implementation

        void IElevationElement.OnElevationPropertyChanged(double oldValue, double newValue) { }

        void IElevationElement.OnTranslationZPropertyChanged(double oldValue, double newValue) { }

        #endregion

        #region ICornerElement Implementation

        void ICornerElement.OnCornerRadiusPropertyChanged(CornerRadius oldValue, CornerRadius newValue) { }

        #endregion

        #region IGradientElement Implementation
        
        void IGradientElement.OnGradientBrushPropertyChanged(LinearGradientBrush oldValue, LinearGradientBrush newValue) { }

        #endregion

        #region IBorderElement Implementation

        void IBorderElement.OnBorderColorPropertyChanged(Color oldValue, Color newValue) { }

        void IBorderElement.OnBorderWidthPropertyChanged(double oldValue, double newValue) { }

        void IBorderElement.OnDashGapPropertyChanged(double oldValue, double newValue) { }

        void IBorderElement.OnDashWidthPropertyChanged(double oldValue, double newValue) { }

        void IBorderElement.OnBorderGradientBrushPropertyChanged(LinearGradientBrush oldValue, LinearGradientBrush newValue) { }
        
        #endregion
    }
}
