using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Controls.Base;
using XamarinBackgroundKit.Extensions;
using IBorderElement = XamarinBackgroundKit.Abstractions.IBorderElement;

namespace XamarinBackgroundKit.Controls
{
    /// <inheritdoc cref="ContentView" />
    /// <summary>
    /// The way a Material Content View Should be.
    /// </summary>
    public class MaterialContentView : ContentView, IFocusableElement, IClickableElement, IMaterialVisualElement
	{
		#region Bindable Properties

		#region IElevation Properties

		public static readonly BindableProperty ElevationProperty = ElevationElement.ElevationProperty;

        /// <summary>
        /// Gets or sets the Elevation of the MaterialContentView
        /// </summary>
		public double Elevation
		{
			get => (double)GetValue(ElevationProperty);
			set => SetValue(ElevationProperty, value);
		}

		#endregion

		#region ICornerElement Properties

		public static readonly BindableProperty CornerRadiusProperty = CornerElement.CornerRadiusProperty;

        /// <summary>
        /// Gets or sets the Elevation of the MaterialContentView
        /// </summary>
		public CornerRadius CornerRadius
		{
			get => (CornerRadius)GetValue(CornerRadiusProperty);
			set => SetValue(CornerRadiusProperty, value);
		}

		#endregion

		#region IGradientElement Properties

		public static readonly BindableProperty AngleProperty = GradientElement.AngleProperty;

        /// <summary>
        /// Gets or sets the Angle of the Gradient of the MaterialContentView
        /// </summary>
		public float Angle
		{
			get => (float)GetValue(AngleProperty);
			set => SetValue(AngleProperty, value);
		}

		public static readonly BindableProperty GradientTypeProperty = GradientElement.GradientTypeProperty;

        /// <summary>
        /// Gets or sets the Type of the Gradient of the MaterialContentView
        /// </summary>
		public GradientType GradientType
		{
			get => (GradientType)GetValue(GradientTypeProperty);
			set => SetValue(GradientTypeProperty, value);
		}

		public static readonly BindableProperty GradientsProperty = GradientElement.GradientsProperty;

        /// <summary>
        /// Gets or sets the Gradients of the MaterialContentView
        /// </summary>
		public IList<GradientStop> Gradients
		{
			get => (IList<GradientStop>)GetValue(GradientsProperty);
			set => SetValue(GradientsProperty, value);
		}

		#endregion

		#region IBorderElement Properties

		public static readonly BindableProperty BorderColorProperty = BorderElement.BorderColorProperty;

        /// <summary>
        /// Gets or sets the Border Color of the MaterialContentView
        /// </summary>
		public Color BorderColor
		{
			get => (Color)GetValue(BorderColorProperty);
			set => SetValue(BorderColorProperty, value);
		}

		public static readonly BindableProperty BorderWidthProperty = BorderElement.BorderWidthProperty;

        /// <summary>
        /// Gets or sets the Border Width of the MaterialContentView
        /// </summary>
		public double BorderWidth
		{
			get => (double)GetValue(BorderWidthProperty);
			set => SetValue(BorderWidthProperty, value);
		}

		#endregion

		#region IClickableElement Properties

		public static readonly BindableProperty IsClickableProperty = ClickableElement.IsClickableProperty;

        /// <summary>
        /// Gets or sets whether the MaterialContentView will be Clickable or not
        /// </summary>
		public bool IsClickable
		{
			get => (bool)GetValue(IsClickableProperty);
			set => SetValue(IsClickableProperty, value);
		}

		public static readonly BindableProperty ClickedCommandProperty = ClickableElement.ClickedCommandProperty;

        /// <summary>
        /// Gets or sets the Command that will be executed when Click fires
        /// </summary>
		public Command ClickedCommand
		{
			get => (Command)GetValue(ClickedCommandProperty);
			set => SetValue(ClickedCommandProperty, value);
		}

		public static readonly BindableProperty ClickedCommandParameterProperty = ClickableElement.ClickedCommandParameterProperty;

        /// <summary>
        /// Gets or sets the Command Parameter that will be sent to the ClickedCommand when the Click fires
        /// </summary>
		public object ClickedCommandParameter
		{
			get => GetValue(ClickedCommandParameterProperty);
			set => SetValue(ClickedCommandParameterProperty, value);
		}

		#endregion

		#region IFocusableElement Properties

		public static readonly BindableProperty IsFocusableProperty = FocusableElement.IsFocusableProperty;

        /// <summary>
        /// Gets or sets whether the MaterialContentView will be Focusable or not
        /// </summary>
		public bool IsFocusable
		{
			get => (bool)GetValue(IsFocusableProperty);
			set => SetValue(IsFocusableProperty, value);
		}

		public static readonly BindableProperty PressedCommandProperty = FocusableElement.PressedCommandProperty;

        /// <summary>
        /// Gets or sets the Command that will be executed when the user presses the MaterialContentView
        /// </summary>
		public Command PressedCommand
		{
			get => (Command)GetValue(PressedCommandProperty);
			set => SetValue(PressedCommandProperty, value);
		}

		public static readonly BindableProperty PressedCommandParameterProperty = FocusableElement.PressedCommandParameterProperty;

        /// <summary>
        /// Gets or sets the Command Parameter that will be sent to the PressedCommand when the user presses the MaterialContentView
        /// </summary>
		public object PressedCommandParameter
		{
			get => GetValue(PressedCommandParameterProperty);
			set => SetValue(PressedCommandParameterProperty, value);
		}

		public static readonly BindableProperty ReleasedCommandProperty = FocusableElement.ReleasedCommandProperty;

        /// <summary>
        /// Gets or sets the Command that will be executed when the user releases the MaterialContentView
        /// </summary>
		public Command ReleasedCommand
		{
			get => (Command)GetValue(ReleasedCommandProperty);
			set => SetValue(ReleasedCommandProperty, value);
		}

		public static readonly BindableProperty ReleasedCommandParameterProperty = FocusableElement.ReleasedCommandParameterProperty;

        /// <summary>
        /// Gets or sets the Command Parameter that will be sent to the ReleasedCommand when the user releases the MaterialContentView
        /// </summary>
		public object ReleasedCommandParameter
		{
			get => GetValue(ReleasedCommandParameterProperty);
			set => SetValue(ReleasedCommandParameterProperty, value);
		}

		public static readonly BindableProperty CancelledCommandProperty = FocusableElement.CancelledCommandProperty;

        /// <summary>
        /// Gets or sets the Command that will be executed when the user cancels the MaterialContentView
        /// </summary>
		public Command CancelledCommand
		{
			get => (Command)GetValue(CancelledCommandProperty);
			set => SetValue(CancelledCommandProperty, value);
		}

		public static readonly BindableProperty CancelledCommandParameterProperty = FocusableElement.CancelledCommandParameterProperty;

        /// <summary>
        /// Gets or sets the Command Parameter that will be sent to the CancelledCommand when the user cancels the MaterialContentView
        /// </summary>
		public object CancelledCommandParameter
		{
			get => GetValue(CancelledCommandParameterProperty);
			set => SetValue(CancelledCommandParameterProperty, value);
		}

		public static readonly BindableProperty ReleasedOrCancelledCommandProperty = FocusableElement.ReleasedOrCancelledCommandProperty;

        /// <summary>
        /// Gets or sets the Command that will be executed when the user releases or cancels the MaterialContentView
        /// </summary>
		public Command ReleasedOrCancelledCommand
		{
			get => (Command)GetValue(ReleasedOrCancelledCommandProperty);
			set => SetValue(ReleasedOrCancelledCommandProperty, value);
		}

		public static readonly BindableProperty ReleasedOrCancelledCommandParameterProperty = FocusableElement.ReleasedOrCancelledCommandParameterProperty;

        /// <summary>
        /// Gets or sets the Command Parameter that will be sent to the ReleasedOrCancelledCommand when the user releases or cancels the MaterialContentView
        /// </summary>
		public object ReleasedOrCancelledCommandParameter
		{
			get => GetValue(ReleasedOrCancelledCommandParameterProperty);
			set => SetValue(ReleasedOrCancelledCommandParameterProperty, value);
		}

		#endregion

        public static readonly BindableProperty IsCircleProperty = BindableProperty.Create(
            nameof(IsCircle), typeof(bool), typeof(MaterialContentView), false);

        public bool IsCircle
        {
            get => (bool) GetValue(IsCircleProperty);
            set => SetValue(IsCircleProperty, value);
        }

        #endregion

        private bool _isExecuting;

        private readonly double _threshold = Math.Pow(10, -15);

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            InvalidateCircle(width, height);
        }

        #region Setup Circle
        
        public virtual async void InvalidateCircle(double width, double height)
        {
            if (!IsCircle) return;

            var desiredCircleSize = Math.Max(width, height);

            InvalidateCircleCorner(desiredCircleSize / 2);

            /*
             * HACK THE BOX:
             * In iOS, while in the Layout Process, at first it calculates the paddings,
             * and second it measures the View. So the actual width and height comes at the second
             * time. So only one thread will wait 10ms and it will update the circle dimensions
             * according to latest ones. This hack prevents to set WidthRequest with only the Padding.
             * If we make this mistake, our view, will have Width = Padding.Left + Padding.Right.
             */
            if (Device.RuntimePlatform == Device.iOS)
            {
                if (_isExecuting) return;
                
                /* Block any other threads */
                _isExecuting = true;

                await Task.Delay(10);
                
                if (Math.Abs(Width - width) < _threshold && Math.Abs(Height - height) < _threshold) return;

                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        InvalidateCircleDimensions(Math.Max(Width, Height));
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                });

                _isExecuting = false;
            }
            else
            {
                InvalidateCircleDimensions(desiredCircleSize);
            }
        }
        
        public virtual void InvalidateCircleCorner(double desiredCornerSize)
        {
            if (CornerRadius == desiredCornerSize) return;

            CornerRadius = desiredCornerSize;
        }

        public virtual void InvalidateCircleDimensions(double desiredCircleSize)
        {
            if (Math.Abs(WidthRequest - HeightRequest) < _threshold && WidthRequest > 0) return;

            if (Math.Abs(WidthRequest - desiredCircleSize) > _threshold)
            {
                WidthRequest = desiredCircleSize;
            }

            if (Math.Abs(HeightRequest - desiredCircleSize) > _threshold)
            {
                HeightRequest = desiredCircleSize;
            }
        }

        #endregion

        #region IElevation Implementation

        void IElevationElement.OnElevationPropertyChanged(double oldValue, double newValue) { }

		#endregion

		#region ICornerElement Implementation

		void ICornerElement.OnCornerRadiusPropertyChanged(CornerRadius oldValue, CornerRadius newValue) { }

		#endregion

		#region IGradientElement Implementation

		void IGradientElement.OnAnglePropertyChanged(float oldValue, float newValue) { }

		void IGradientElement.OnGradientTypePropertyChanged(GradientType oldValue, GradientType newValue) { }

		void IGradientElement.OnGradientsPropertyChanged(IList<GradientStop> oldValue, IList<GradientStop> newValue) { }

		#endregion

		#region IBorderElement Implementation

		void IBorderElement.OnBorderColorPropertyChanged(Color oldValue, Color newValue) { }

		void IBorderElement.OnBorderWidthPropertyChanged(double oldValue, double newValue) { }

		#endregion

		#region IFocusableElement Implementation

		public event EventHandler<EventArgs> Pressed;
		public event EventHandler<EventArgs> Released;
		public event EventHandler<EventArgs> Cancelled;
		public event EventHandler<EventArgs> ReleasedOrCancelled;

		[EditorBrowsable(EditorBrowsableState.Never)]
		public void OnPressed()
		{
			Pressed?.Invoke(this, EventArgs.Empty);
			PressedCommand?.CheckAndExecute(PressedCommandParameter);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public void OnReleased()
		{
			Released?.Invoke(this, EventArgs.Empty);
			ReleasedCommand?.CheckAndExecute(ReleasedCommandParameter);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public void OnCancelled()
		{
			Cancelled?.Invoke(this, EventArgs.Empty);
			CancelledCommand?.CheckAndExecute(CancelledCommandParameter);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public void OnReleasedOrCancelled()
		{
			ReleasedOrCancelled?.Invoke(this, EventArgs.Empty);
			ReleasedOrCancelledCommand?.CheckAndExecute(ReleasedOrCancelledCommandParameter);
		}

		public void OnIsFocusablePropertyChanged(bool oldValue, bool newValue) { }

		#endregion

		#region IClickableElement Implementation

		public event EventHandler<EventArgs> Clicked;

		[EditorBrowsable(EditorBrowsableState.Never)]
		public void OnClicked()
		{
			Clicked?.Invoke(this, EventArgs.Empty);
			ClickedCommand?.CheckAndExecute(ClickedCommandParameter);
		}

		public void OnIsClickablePropertyChanged(bool oldValue, bool newValue) { }

		#endregion
	}
}
