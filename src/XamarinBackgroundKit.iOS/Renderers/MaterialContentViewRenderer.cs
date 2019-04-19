using CoreAnimation;
using Foundation;
using ObjCRuntime;
using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.iOS.Renderers;

[assembly: ExportRenderer(typeof(MaterialContentView), typeof(MaterialContentViewRenderer))]
namespace XamarinBackgroundKit.iOS.Renderers
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class MaterialContentViewRenderer : UIView, IVisualElementRenderer, IEffectControlProvider
    {
        private bool _disposed;
        private VisualElementTracker _visualElementTracker;
        private VisualElementPackager _visualElementPackager;

        #region IVisualElementRenderer Properties

        private MaterialContentView _element;
        protected MaterialContentView Element
        {
            get => _element;
            set
            {
                if (_element == value) return;
                var oldElement = _element;
                _element = value;

                OnElementChanged(new ElementChangedEventArgs<MaterialContentView>(oldElement, _element));
            }
        }

        UIView IVisualElementRenderer.NativeView => this;
        VisualElement IVisualElementRenderer.Element => Element;
        UIViewController IVisualElementRenderer.ViewController => null;

        public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

        #endregion

        [Export("layerClass")]
        public static Class LayerClass()
        {
            return new Class(typeof(GradientStrokeLayer));
        }

        #region IVisualElementRenderer Implementation	

        void IVisualElementRenderer.SetElement(VisualElement element)
        {
            Element = element as MaterialContentView;

            if (Element == null || string.IsNullOrEmpty(Element.AutomationId)) return;

            AccessibilityIdentifier = Element.AutomationId;
        }

        SizeRequest IVisualElementRenderer.GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return this.GetSizeRequest(widthConstraint, heightConstraint, 44, 44);
        }

        void IVisualElementRenderer.SetElementSize(Size size)
        {
            var (width, height) = size;
            Layout.LayoutChildIntoBoundingRegion(Element, new Rectangle(Element.X, Element.Y, width, height));
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

        protected virtual void OnElementChanged(ElementChangedEventArgs<MaterialContentView> e)
        {
            EffectUtilities.RegisterEffectControlProvider(this, e.OldElement, e.NewElement);

            ElementChanged?.Invoke(this, new VisualElementChangedEventArgs(e.OldElement, e.NewElement));

            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged -= OnElementPropertyChanged;
            }

            if (e.NewElement == null) return;

            if (_visualElementTracker == null)
            {
                _visualElementTracker = new MaterialVisualElementTracker(this);
                _visualElementPackager = new VisualElementPackager(this);
                _visualElementPackager.Load();
            }

            UpdateIsFocusable();

            e.NewElement.PropertyChanged += OnElementPropertyChanged;
        }

        #endregion

        #region Property Changed

        protected virtual void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == MaterialContentView.IsFocusableProperty.PropertyName
                || e.PropertyName == MaterialContentView.IsClickableProperty.PropertyName) UpdateIsFocusable();
        }

        private void UpdateIsFocusable()
        {
            if (Element.IsFocusable && Element.IsClickable)
            {
                //MultipleTouchEnabled = true;
                UserInteractionEnabled = true;
            }
            else
            {
                //MultipleTouchEnabled = false;
                UserInteractionEnabled = false;
            }
        }

        #endregion

        #region Touch Handling

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            if (Element.IsFocusable)
            {
                Element?.OnPressed();
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            if (Element.IsClickable)
            {
                Element?.OnClicked();
            }

            if (Element.IsFocusable)
            {
                Element?.OnReleased();
                Element?.OnReleasedOrCancelled();
            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            if (Element.IsFocusable)
            {
                Element?.OnCancelled();
                Element?.OnReleasedOrCancelled();
            }
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