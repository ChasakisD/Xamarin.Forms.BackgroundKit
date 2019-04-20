using System;
using System.ComponentModel;
using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Controls.Base;
using XamarinBackgroundKit.Extensions;

namespace XamarinBackgroundKit.Controls
{
    /// <inheritdoc cref="ContentView" />
    /// <summary>
    /// The way a Material Content View Should be.
    /// </summary>
    [DesignTimeVisible(true)]
    public class MaterialContentView : ContentView, IFocusableElement, IClickableElement, IBackgroundElement
	{
        #region Bindable Properties

        public static readonly BindableProperty BackgroundProperty = BackgroundElement.BackgroundProperty;

        /// <summary>
        /// Gets or sets whether the Background Property of MaterialContentView
        /// </summary>
		public Background Background
        {
            get => (Background)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

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

        public static readonly BindableProperty IsCornerRadiusHalfHeightProperty = BindableProperty.Create(
            nameof(IsCornerRadiusHalfHeight), typeof(bool), typeof(MaterialContentView), false);

        public bool IsCornerRadiusHalfHeight
        {
            get => (bool)GetValue(IsCornerRadiusHalfHeightProperty);
            set => SetValue(IsCornerRadiusHalfHeightProperty, value);
        }

        #endregion

        #region Setup Circle

        private bool _blockFirstTime = true;

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            InvalidateCircle(width, height);
            InvalidateCornerRadiusHalfHeight(height);
        }

        public virtual void InvalidateCircle(double width, double height)
        {
            if (!IsCircle) return;

            var desiredCircleSize = Math.Max(width, height);

            InvalidateCircleCorner(desiredCircleSize / 2);

            /*
             * HACK THE BOX:
             * In iOS, while in the Layout Process, at first it calculates the paddings,
             * and second it measures the View. So the actual width and height comes at the second
             * time. So it will update the circle dimensions according to latest ones.
             * This hack prevents to set WidthRequest with only the Padding.
             * If we make this mistake, our view, will have Width = Padding.Left + Padding.Right.
             */
            if (Device.RuntimePlatform == Device.iOS)
            {
                if (_blockFirstTime)
                {
                    _blockFirstTime = false;
                    return;
                }
            }

            InvalidateCircleDimensions(desiredCircleSize);
        }

        public virtual void InvalidateCircleCorner(double desiredCornerSize)
        {
            if (Background == null) return;

            if (Math.Abs(Background.CornerRadius.TopLeft - desiredCornerSize) < 0.0001) return;

            Background.CornerRadius = desiredCornerSize;
        }

        public virtual void InvalidateCircleDimensions(double desiredCircleSize)
        {
            var threshold = Math.Pow(10, -15);
            if (Math.Abs(WidthRequest - HeightRequest) < threshold && WidthRequest > 0) return;

            if (Math.Abs(WidthRequest - desiredCircleSize) > threshold)
            {
                WidthRequest = desiredCircleSize;
            }

            if (Math.Abs(HeightRequest - desiredCircleSize) > threshold)
            {
                HeightRequest = desiredCircleSize;
            }
        }

        public virtual void InvalidateCornerRadiusHalfHeight(double height)
        {
            if (!IsCornerRadiusHalfHeight || Background == null) return;

            var threshold = Math.Pow(10, -15);
            var desiredCornerRadius = height / 2d;

            if (Math.Abs(Background.CornerRadius.TopLeft - desiredCornerRadius) < threshold) return;

            Background.CornerRadius = desiredCornerRadius;
        }


        #endregion

        #region IBackgroundElement Implementation

        void IBackgroundElement.OnBackgroundChanged(Background oldValue, Background newValue)
        {
            newValue.SetBinding(BindingContextProperty, new Binding("BindingContext", source: this));
        }

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
