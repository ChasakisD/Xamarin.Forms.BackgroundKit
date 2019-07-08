using System;
using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Controls.Base;
using IBorderElement = XamarinBackgroundKit.Abstractions.IBorderElement;

namespace XamarinBackgroundKit.Controls
{
    public class Background : BindableObject, IMaterialVisualElement
    {
        #region Bindable Properties

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
		public GradientBrush GradientBrush
        {
            get => (GradientBrush)GetValue(GradientBrushProperty);
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

        public static readonly BindableProperty BorderStyleProperty = BorderElement.BorderStyleProperty;

        /// <summary>
        /// Gets or sets the Border Width of the Border
        /// </summary>
		public BorderStyle BorderStyle
        {
            get => (BorderStyle)GetValue(BorderStyleProperty);
            set => SetValue(BorderStyleProperty, value);
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
        public GradientBrush BorderGradientBrush
        {
            get => (GradientBrush)GetValue(BorderGradientBrushProperty);
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

        #endregion

        public event EventHandler<EventArgs> InvalidateGradientRequested;
        public event EventHandler<EventArgs> InvalidateBorderGradientRequested;

        public Background()
        {
            ((IGradientElement)this).OnGradientBrushPropertyChanged(null, GradientBrush);
            ((IBorderElement)this).OnBorderGradientBrushPropertyChanged(null, BorderGradientBrush);
        }

        #region IElevation Implementation

        void IElevationElement.OnElevationPropertyChanged(double oldValue, double newValue) { }

        void IElevationElement.OnTranslationZPropertyChanged(double oldValue, double newValue) { }

        #endregion

        #region ICornerElement Implementation

        void ICornerElement.OnCornerRadiusPropertyChanged(CornerRadius oldValue, CornerRadius newValue) { }

        #endregion

        #region IGradientElement Implementation

        void IGradientElement.OnGradientBrushPropertyChanged(GradientBrush oldValue, GradientBrush newValue)
        {
            if (oldValue != null)
            {
                oldValue.InvalidateGradientRequested -= OnInvalidateGradientRequested;
            }

            if (newValue != null)
            {
                newValue.InvalidateGradientRequested += OnInvalidateGradientRequested;
            }

            OnInvalidateGradientRequested(this, EventArgs.Empty);
        }

        private void OnInvalidateGradientRequested(object sender, EventArgs e)
        {
            InvalidateGradientRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region IBorderElement Implementation

        void IBorderElement.OnBorderColorPropertyChanged(Color oldValue, Color newValue) { }

        void IBorderElement.OnBorderWidthPropertyChanged(double oldValue, double newValue) { }

        void IBorderElement.OnBorderStylePropertyChanged(BorderStyle oldValue, BorderStyle newValue) { }

        void IBorderElement.OnDashGapPropertyChanged(double oldValue, double newValue) { }

        void IBorderElement.OnDashWidthPropertyChanged(double oldValue, double newValue) { }

        void IBorderElement.OnBorderGradientBrushPropertyChanged(GradientBrush oldValue, GradientBrush newValue)
        {
            if (oldValue != null)
            {
                oldValue.InvalidateGradientRequested -= OnInvalidateBorderGradientRequested;
            }

            if (newValue != null)
            {
                newValue.InvalidateGradientRequested += OnInvalidateBorderGradientRequested;
            }

            OnInvalidateBorderGradientRequested(this, EventArgs.Empty);
        }

        private void OnInvalidateBorderGradientRequested(object sender, EventArgs e)
        {
            InvalidateBorderGradientRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
