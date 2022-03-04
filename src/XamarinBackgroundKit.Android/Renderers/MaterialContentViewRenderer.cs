using Android.Content;
using Android.Graphics;
using Android.Views;
using System.ComponentModel;
using Android.Content.Res;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinBackgroundKit.Android.Renderers;
using XamarinBackgroundKit.Controls;
using AView = Android.Views.View;
using View = Xamarin.Forms.View;

[assembly: ExportRenderer(typeof(MaterialContentView), typeof(MaterialContentViewRenderer))]
namespace XamarinBackgroundKit.Android.Renderers
{
    public class MaterialContentViewRenderer : ViewRenderer, AView.IOnClickListener
	{
        private bool _disposed;
		private bool _isClickListenerSet;

		protected MaterialBackgroundManager BackgroundManager;

        private MaterialContentView ElementController => Element as MaterialContentView;

        public MaterialContentViewRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null) return;

            this.EnsureId();

            if (BackgroundManager == null)
            {
                BackgroundManager = new MaterialBackgroundManager(this);
            }

            UpdateAll();
        }

		#region Clip Subviews

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            if (changed)
            {
                BackgroundManager?.Invalidate();
            }
        }

        protected override void DispatchDraw(Canvas canvas)
        {
            BackgroundManager?.UpdateElevation();

            BackgroundManager?.Draw(this, canvas, () => base.DispatchDraw(canvas));
        }

        #endregion

        #region Property Changed

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (_disposed) return;

            base.OnElementPropertyChanged(sender, e);
            
			if (e.PropertyName == MaterialContentView.IsFocusableProperty.PropertyName) UpdateIsFocusable();
			else if (e.PropertyName == MaterialContentView.IsClickableProperty.PropertyName) UpdateIsClickable();
		}

		private void UpdateAll()
		{
			UpdateIsFocusable();
			UpdateIsClickable();
		}

		private void UpdateIsFocusable()
        {
            Focusable = ElementController.IsFocusable;
		}

		private void UpdateIsClickable()
		{
			if (_isClickListenerSet && !ElementController.IsClickable)
			{
				Clickable = false;
				SetOnClickListener(null);
				_isClickListenerSet = false;
			}
			else if (!_isClickListenerSet && ElementController.IsClickable)
			{
				Clickable = true;
				SetOnClickListener(this);
				_isClickListenerSet = true;
			}
		}

		#endregion

		#region Touch Handling

        public override bool OnTouchEvent(MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    ElementController?.OnPressed();
                    break;
                case MotionEventActions.Cancel:
                    ElementController?.OnCancelled();
                    ElementController?.OnReleasedOrCancelled();
                    break;
                case MotionEventActions.Up:
                    ElementController?.OnReleased();
                    ElementController?.OnReleasedOrCancelled();
                    break;
            }
            
            return base.OnTouchEvent(e);
        }

		#endregion

		#region Click Handling

		public void OnClick(AView v)
		{
            ElementController?.OnClicked();
		}

		#endregion

		#region LifeCycle

		protected override void Dispose(bool disposing)
		{
			if (_disposed) return;

			_disposed = true;

			if (disposing)
			{
                if (BackgroundManager != null)
                {
                    BackgroundManager.Dispose();
                    BackgroundManager = null;
                }

                if (_isClickListenerSet)
				{
					SetOnClickListener(null);
				}
			}

			base.Dispose(disposing);
		}

		#endregion
	}
}
