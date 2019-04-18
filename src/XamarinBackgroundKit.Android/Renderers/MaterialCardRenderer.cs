using Android.Content;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Material.Android;
using Xamarin.Forms.Platform.Android;
using XamarinBackgroundKit.Android.Renderers;
using XamarinBackgroundKit.Controls;
using AView = Android.Views.View;
using Size = Xamarin.Forms.Size;

[assembly: ExportRenderer(typeof(MaterialCard), typeof(MaterialCardRenderer))]
namespace XamarinBackgroundKit.Android.Renderers
{
    [Preserve(AllMembers = true)]
    public class MaterialCardRenderer : 
        CardView, 
        IViewRenderer, 
        IVisualElementRenderer, 
        IEffectControlProvider,
        AView.IOnClickListener
    {
        private bool _disposed;
        private bool _isClickListenerSet;
        private int? _defaultLabelFor;

        private VisualElementTracker _visualElementTracker;
        private VisualElementPackager _visualElementPackager;

        #region IVisualElementRenderer Properties

        private MaterialCard _element;
        protected MaterialCard Element
        {
            get => _element;
            set
            {
                if (_element == value) return;
                var oldElement = _element;
                _element = value;

                OnElementChanged(new ElementChangedEventArgs<MaterialCard>(oldElement, _element));
            }
        }

        AView IVisualElementRenderer.View => this;
        ViewGroup IVisualElementRenderer.ViewGroup => this;
        VisualElement IVisualElementRenderer.Element => Element;
        VisualElementTracker IVisualElementRenderer.Tracker => _visualElementTracker;

        public event EventHandler<VisualElementChangedEventArgs> ElementChanged;
        public event EventHandler<PropertyChangedEventArgs> ElementPropertyChanged;

        #endregion

        public MaterialCardRenderer(Context context) : base(MaterialContextThemeWrapper.Create(context)) { }

        #region IViewRenderer Implementation

        void IViewRenderer.MeasureExactly()
        {
            if (Element == null) return;

            if (Element.Width <= 0 || Element.Height <= 0) return;

            var widthPixels = (int)Context.ToPixels(Element.Width);
            var heightPixels = (int)Context.ToPixels(Element.Height);

            var widthMeasureSpec = MeasureSpec.MakeMeasureSpec(widthPixels, MeasureSpecMode.Exactly);
            var heightMeasureSpec = MeasureSpec.MakeMeasureSpec(heightPixels, MeasureSpecMode.Exactly);

            Measure(widthMeasureSpec, heightMeasureSpec);
        }

        #endregion

        #region IVisualRenderer Implementation

        void IVisualElementRenderer.SetLabelFor(int? id)
        {
            if (_defaultLabelFor == null)
                _defaultLabelFor = ViewCompat.GetLabelFor(this);

            ViewCompat.SetLabelFor(this, (int)(id ?? _defaultLabelFor));
        }

        void IVisualElementRenderer.UpdateLayout()
        {
            _visualElementTracker?.UpdateLayout();
        }

        void IVisualElementRenderer.SetElement(VisualElement element)
        {
            Element = element as MaterialCard;

            if (Element == null || string.IsNullOrEmpty(Element.AutomationId)) return;

            ContentDescription = Element.AutomationId;
        }

        SizeRequest IVisualElementRenderer.GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            return new SizeRequest(new Size(Context.ToPixels(20), Context.ToPixels(20)));
        }

        #endregion

        #region IEffectControlProvider Implementation

        void IEffectControlProvider.RegisterEffect(Effect effect)
        {
            if (!(effect is PlatformEffect platformEffect)) return;

            platformEffect.SetContainer(this);
        }

        #endregion

        #region Element Changed

        protected virtual void OnElementChanged(ElementChangedEventArgs<MaterialCard> e)
        {
            this.EnsureId();

            EffectUtilities.RegisterEffectControlProvider(this, e.OldElement, e.NewElement);

            ElementChanged?.Invoke(this, new VisualElementChangedEventArgs(e.OldElement, e.NewElement));

            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged -= OnElementPropertyChanged;
            }

            if (e.NewElement == null) return;

            PreventCornerOverlap = true;

            using (var rippleTypedValue = new TypedValue())
            {
                Context.Theme.ResolveAttribute(global::Android.Resource.Attribute.SelectableItemBackground, rippleTypedValue, true);

                Foreground = ContextCompat.GetDrawable(Context, rippleTypedValue.ResourceId);
            }

            if (_visualElementTracker == null)
            {
                _visualElementTracker = new MaterialVisualElementTracker(this);
                _visualElementPackager = new VisualElementPackager(this);
                _visualElementPackager.Load();
            }

            UpdateAll();

            e.NewElement.PropertyChanged += OnElementPropertyChanged;
        }

        #endregion

        #region Layout Children

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            if (Element == null) return;

            var children = ((IElementController)Element).LogicalChildren;
            foreach (var child in children)
            {
                if (!(child is VisualElement visualElement)) continue;

                Platform.GetRenderer(visualElement)?.UpdateLayout();
            }
        }

        #endregion

        #region Property Changed

        protected virtual void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_disposed) return;

            ElementPropertyChanged?.Invoke(this, e);

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
            Focusable = Element.IsFocusable;
        }

        private void UpdateIsClickable()
        {
            if (_isClickListenerSet && !Element.IsClickable)
            {
                Clickable = false;
                SetOnClickListener(null);
                _isClickListenerSet = false;
            }
            else if (!_isClickListenerSet && Element.IsClickable)
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
                    Element?.OnPressed();
                    break;
                case MotionEventActions.Cancel:
                    Element?.OnCancelled();
                    Element?.OnReleasedOrCancelled();
                    break;
                case MotionEventActions.Up:
                    Element?.OnReleased();
                    Element?.OnReleasedOrCancelled();
                    break;
            }

            return base.OnTouchEvent(e);
        }

        #endregion

        #region Click Handling

        public void OnClick(AView v)
        {
            Element?.OnClicked();
        }

        #endregion

        #region LifeCycle

        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;

            _disposed = true;

            if (disposing)
            {
                EffectUtilities.UnregisterEffectControlProvider(this, Element);

                if (_isClickListenerSet)
                {
                    SetOnClickListener(null);
                }

                if (_visualElementTracker != null)
                {
                    _visualElementTracker.Dispose();
                    _visualElementTracker = null;
                }

                if (_visualElementPackager != null)
                {
                    _visualElementPackager.Dispose();
                    _visualElementPackager = null;
                }

                for (var i = 0; i < ChildCount; i++)
                {
                    GetChildAt(i)?.Dispose();
                }

                if (Element != null)
                {
                    Element.PropertyChanged -= OnElementPropertyChanged;

                    ((IVisualElementRenderer)this).SetElement(null);
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}