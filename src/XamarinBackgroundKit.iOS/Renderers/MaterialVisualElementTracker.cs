using MaterialComponents;
using System;
using System.ComponentModel;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Controls.Base;
using XamarinBackgroundKit.Extensions;
using XamarinBackgroundKit.iOS.Extensions;

namespace XamarinBackgroundKit.iOS.Renderers
{
    public class MaterialVisualElementTracker : VisualElementTracker
    {
        private bool _disposed;
        private VisualElement _visualElement;
        private IVisualElementRenderer _renderer;
        private IMaterialVisualElement _backgroundElement;

        private readonly PropertyChangedEventHandler _propertyChangedHandler;

        public MaterialVisualElementTracker(IVisualElementRenderer renderer) : base(renderer)
        {
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer), "Renderer cannot be null");

            _propertyChangedHandler = OnRendererElementPropertyChanged;
            _renderer.ElementChanged += OnRendererElementChanged;

            SetVisualElement(null, _renderer.Element);
        }

        private void SetVisualElement(VisualElement oldElement, VisualElement newElement)
        {
            IMaterialVisualElement oldMaterialVisualElement;
            IMaterialVisualElement newMaterialVisualElement;

            switch (oldElement)
            {
                case IBackgroundElement oldBackgroundElement:
                    oldMaterialVisualElement = oldBackgroundElement.Background;
                    break;
                case IMaterialVisualElement oldMaterialElement:
                    oldMaterialVisualElement = oldMaterialElement;
                    break;
                default:
                    oldMaterialVisualElement = null;
                    break;
            }

            switch (newElement)
            {
                case IBackgroundElement newBackgroundElement:
                    newMaterialVisualElement = newBackgroundElement.Background;
                    break;
                case IMaterialVisualElement newMaterialElement:
                    newMaterialVisualElement = newMaterialElement;
                    break;
                default:
                    newMaterialVisualElement = null;
                    break;
            }

            _visualElement = newElement;

            SetElement(oldMaterialVisualElement, newMaterialVisualElement);
        }

        public void SetElement(IMaterialVisualElement oldElement, IMaterialVisualElement newElement)
        {
            if (oldElement != null)
            {
                oldElement.PropertyChanged -= _propertyChangedHandler;
                oldElement.InvalidateGradientRequested -= InvalidateGradientsRequested;
            }

            _backgroundElement = newElement;
            if (newElement == null) return;

            newElement.PropertyChanged += _propertyChangedHandler;
            newElement.InvalidateGradientRequested += InvalidateGradientsRequested;

            if (oldElement == null)
            {
                UpdateClipToBounds();

                InvalidateLayer();
                return;
            }
            
            var eps = Math.Pow(10, -10);

            if (Math.Abs(oldElement.Elevation - newElement.Elevation) > eps)
                UpdateElevation();

            if (Math.Abs(oldElement.Angle - newElement.Angle) > eps
                || oldElement.GradientType != newElement.GradientType
                || oldElement.Gradients.AreEqual(newElement.Gradients))
                UpdateGradients();

            if (oldElement.BorderColor != newElement.BorderColor
                || Math.Abs(oldElement.BorderWidth - newElement.BorderWidth) > eps)
                UpdateBorder();

            if (oldElement.CornerRadius != newElement.CornerRadius)
                UpdateCornerRadius();

            UpdateClipToBounds();
        }

        public void InvalidateLayer()
        {
            UpdateGradients();
            UpdateBorder();
            UpdateCornerRadius();
            UpdateElevation();
        }
        
        private void InvalidateGradientsRequested(object sender, EventArgs e)
        {
            UpdateGradients();
        }

        private void OnRendererElementChanged(object sender, VisualElementChangedEventArgs e)
        {
            SetVisualElement(e.OldElement, e.NewElement);
        }

        private void OnRendererElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_renderer == null || _disposed) return;

            if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName
                || e.PropertyName == GradientElement.AngleProperty.PropertyName
                || e.PropertyName == GradientElement.GradientsProperty.PropertyName
                || e.PropertyName == GradientElement.GradientTypeProperty.PropertyName
                || e.PropertyName == Background.ColorProperty.PropertyName) UpdateGradients();
            else if (e.PropertyName == BorderElement.BorderColorProperty.PropertyName
                || e.PropertyName == BorderElement.BorderWidthProperty.PropertyName
                || e.PropertyName == BorderElement.BorderGradientsProperty.PropertyName
                || e.PropertyName == BorderElement.BorderGradientTypeProperty.PropertyName
                || e.PropertyName == BorderElement.DashGapProperty.PropertyName
                || e.PropertyName == BorderElement.DashWidthProperty.PropertyName) UpdateBorder();
            else if (e.PropertyName == ElevationElement.ElevationProperty.PropertyName) UpdateElevation();
            else if (e.PropertyName == CornerElement.CornerRadiusProperty.PropertyName) UpdateCornerRadius();
            else if (e.PropertyName == Layout.IsClippedToBoundsProperty.PropertyName) UpdateClipToBounds();
        }

        public void UpdateGradients()
        {
            if (_renderer?.NativeView == null) return;

            var emptyGradients = _backgroundElement.Gradients == null || !_backgroundElement.Gradients.Any();

            switch (_renderer.NativeView)
            {
                case Card mCard when emptyGradients:
                    using (var themer = new SemanticColorScheme())
                    {
                        themer.SurfaceColor = _backgroundElement.Color.ToUIColor();
                        CardsColorThemer.ApplySemanticColorScheme(themer, mCard);
                    }
                    return;
                case ChipView mChip when emptyGradients:
                    mChip.SetBackgroundColor(_backgroundElement.Color.ToUIColor(), UIControlState.Normal);
                    return;
                case UIView view when emptyGradients:
                    view.BackgroundColor = _backgroundElement.Color.ToUIColor();
                    return;
            }

            /* Chip does not accept background changes and does not support gradient */
            if (_renderer.NativeView is ChipView) return;

            _renderer.NativeView.SetGradient(_backgroundElement);
        }

        public void UpdateElevation()
        {
            if (_renderer?.NativeView == null) return;

            switch (_renderer.NativeView)
            {
                case Card mCard:
                    mCard.SetElevation(_backgroundElement);
                    break;
                default:
                    _renderer.NativeView.SetMaterialElevation(_backgroundElement);
                    break;
            }
        }

        public void UpdateCornerRadius()
        {
            if (_renderer?.NativeView == null) return;

            switch (_renderer.NativeView)
            {
                case Card mCard:
                    mCard.SetCornerRadius(_backgroundElement);
                    break;
                default:
                    _renderer.NativeView.SetCornerRadius(_backgroundElement);
                    break;
            }
        }

        public void UpdateBorder()
        {
            if (_renderer?.NativeView == null) return;

            switch (_renderer.NativeView)
            {
                case ChipView mChip:
                    mChip.SetBorder(_backgroundElement);
                    break;
                case Card mCardView:
                    mCardView.SetBorder(_backgroundElement);
                    break;
                default:
                    _renderer.NativeView.SetBorder(_backgroundElement);
                    break;
            }
        }

        private void UpdateClipToBounds()
        {
            if (_renderer?.NativeView == null || !(_visualElement is Layout layout)) return;

            _renderer.NativeView.ClipsToBounds = layout.IsClippedToBounds;
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;

            _disposed = true;

            if (disposing)
            {
                SetVisualElement(_visualElement, null);

                if (_renderer != null)
                {
                    _renderer.ElementChanged -= OnRendererElementChanged;
                    _renderer = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}