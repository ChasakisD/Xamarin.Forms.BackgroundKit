using System;
using System.ComponentModel;
using CoreAnimation;
using MaterialComponents;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Controls.Base;
using XamarinBackgroundKit.Effects;
using XamarinBackgroundKit.iOS.Extensions;
using XamarinBackgroundKit.Shapes;
using MButton = MaterialComponents.Button;

namespace XamarinBackgroundKit.iOS.Renderers
{
    public class MaterialBackgroundManager : IDisposable
    {
        private bool _disposed;
        private VisualElement _visualElement;
        private MaterialShapeManager _shapeManager;
        private IBackgroundShape _defaultShape;
        private IVisualElementRenderer _renderer;
        private IMaterialVisualElement _backgroundElement;

        private InkTouchController _inkTouchController;

        public MaterialBackgroundManager(IVisualElementRenderer renderer)
        {
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer), "Renderer cannot be null");
            _renderer.ElementChanged += OnRendererElementChanged;

            _defaultShape = new RoundRect();
            _shapeManager = new MaterialShapeManager();

            SetVisualElement(null, _renderer.Element);
        }

        #region Element Setters

        private void SetVisualElement(VisualElement oldElement, VisualElement newElement)
        {
            if(oldElement != null)
            {
                oldElement.PropertyChanged -= OnElementPropertyChanged;
            }

            if(newElement != null)
            {
                newElement.PropertyChanged += OnElementPropertyChanged;
            }

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
                    oldMaterialVisualElement = oldElement == null ? null : BackgroundEffect.GetBackground(oldElement);
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
                    newMaterialVisualElement = newElement == null ? null : BackgroundEffect.GetBackground(newElement);
                    break;
            }

            _visualElement = newElement;

            SetBackgroundElement(oldMaterialVisualElement, newMaterialVisualElement);
        }

        public void SetBackgroundElement(IMaterialVisualElement oldMaterialElement, IMaterialVisualElement newMaterialElement)
        {
            if (oldMaterialElement != null)
            {
                oldMaterialElement.PropertyChanged -= OnBackgroundPropertyChanged;
                oldMaterialElement.InvalidateGradientRequested -= InvalidateGradientsRequested;
                oldMaterialElement.InvalidateBorderGradientRequested -= InvalidateBorderGradientsRequested;
            }

            _backgroundElement = newMaterialElement;
            if (newMaterialElement == null) return;

            newMaterialElement.PropertyChanged += OnBackgroundPropertyChanged;
            newMaterialElement.InvalidateGradientRequested += InvalidateGradientsRequested;
            newMaterialElement.InvalidateBorderGradientRequested += InvalidateBorderGradientsRequested;

            if (oldMaterialElement == null)
            {
                _renderer.NativeView.FindLayerOfType<GradientStrokeLayer>();
                
                UpdateColor();
                UpdateGradients();
                UpdateBorder();
                UpdateElevation();
                UpdateCornerRadius();
                UpdateRipple();
                InvalidateClipToBounds();
                SetShape(null, false);
                return;
            }
            
            var eps = Math.Pow(10, -10);

            if (oldMaterialElement.Color != newMaterialElement.Color)
                UpdateColor();

            if (Math.Abs(oldMaterialElement.Elevation - newMaterialElement.Elevation) > eps)
                UpdateElevation();

            if (oldMaterialElement.GradientBrush != newMaterialElement.GradientBrush)
                UpdateGradients();

            if (oldMaterialElement.BorderColor != newMaterialElement.BorderColor
                || Math.Abs(oldMaterialElement.BorderWidth - newMaterialElement.BorderWidth) > eps
                || oldMaterialElement.BorderGradientBrush != newMaterialElement.BorderGradientBrush)
                UpdateBorder();

            if (oldMaterialElement.CornerRadius != newMaterialElement.CornerRadius)
                UpdateCornerRadius();

            InvalidateClipToBounds();
        }

        #endregion

        #region Property Changed

        private void InvalidateGradientsRequested(object sender, EventArgs e)
        {
            UpdateGradients();
        }

        private void InvalidateBorderGradientsRequested(object sender, EventArgs e)
        {
            UpdateBorder();
        }

        private void OnRendererElementChanged(object sender, VisualElementChangedEventArgs e)
        {
            SetVisualElement(e.OldElement, e.NewElement);
        }

        private void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_renderer == null || _disposed) return;

            if (e.PropertyName == BackgroundElement.BackgroundProperty.PropertyName)
                SetVisualElement(_visualElement, _renderer.Element);
        }

        private void OnBackgroundPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_renderer == null || _disposed) return;

            if (e.PropertyName == BorderElement.BorderColorProperty.PropertyName
                     || e.PropertyName == BorderElement.BorderWidthProperty.PropertyName
                     || e.PropertyName == BorderElement.DashGapProperty.PropertyName
                     || e.PropertyName == BorderElement.DashWidthProperty.PropertyName
                     || e.PropertyName == BorderElement.BorderStyleProperty.PropertyName) UpdateBorder();
            else if (e.PropertyName == CornerElement.CornerRadiusProperty.PropertyName) UpdateCornerRadius();
            else if (e.PropertyName == Background.ColorProperty.PropertyName
                     || e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName) UpdateColor();
            else if (e.PropertyName == Background.IsRippleEnabledProperty.PropertyName
                     || e.PropertyName == Background.RippleColorProperty.PropertyName) UpdateRipple();
            else if (e.PropertyName == ElevationElement.ElevationProperty.PropertyName) UpdateElevation();
        }

        #endregion

        #region Background Handling

        private void UpdateRipple()
        {
            if (_renderer.NativeView is Card || _renderer.NativeView is ChipView || _renderer.NativeView is MButton) return;

            if (_backgroundElement.IsRippleEnabled)
            {
                if (_inkTouchController == null)
                {
                    _inkTouchController = new InkTouchController(_renderer.NativeView);
                    _inkTouchController.AddInkView();
                }

                InvalidateClipToBounds();

                if (_inkTouchController?.DefaultInkView == null) return;
                _inkTouchController.DefaultInkView.UsesLegacyInkRipple = false;

                if (_backgroundElement.RippleColor != Color.Default)
                {
                    _inkTouchController.DefaultInkView.InkColor = _backgroundElement.RippleColor.ToUIColor();
                }
            }
            else if (_inkTouchController != null && !_backgroundElement.IsRippleEnabled)
            {
                _inkTouchController.CancelInkTouchProcessing();
                _inkTouchController?.DefaultInkView?.RemoveFromSuperview();
                _inkTouchController?.Dispose();
                _inkTouchController = null;
            }
        }

        private void UpdateColor()
        {
            if (_renderer?.NativeView == null || _backgroundElement == null || _visualElement == null) return;

            var color = _backgroundElement.Color == Color.Default
                ? _visualElement.BackgroundColor == Color.Default
                    ? Color.Default
                    : _visualElement.BackgroundColor
                : _backgroundElement.Color;

            switch (_renderer.NativeView)
            {
                case Card mCard:
                    if (color == Color.Default) return;
                    using (var themer = new SemanticColorScheme())
                    {
                        themer.SurfaceColor = _backgroundElement.Color.ToUIColor();
                        CardsColorThemer.ApplySemanticColorScheme(themer, mCard);
                    }
                    break;
                case ChipView mChip:
                    if (color == Color.Default) return;
                    mChip.SetBackgroundColor(_backgroundElement.Color.ToUIColor(), UIControlState.Normal);
                    break;
                case MButton mButton:
                    if (color == Color.Default) return;
                    mButton.SetBackgroundColor(_backgroundElement.Color.ToUIColor());
                    break;
                default:
                    _renderer.NativeView.BackgroundColor = UIColor.Clear;

                    if (color == Color.Default) return;
                    _renderer.NativeView.SetColor(_backgroundElement.Color);
                    break;
            }
        }

        public void UpdateGradients()
        {
            if (_renderer?.NativeView == null) return;

            /* Chip does not accept background changes and does not support gradient */
            if (_renderer.NativeView is ChipView || _renderer.NativeView is MButton) return;

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
                case MButton mButton:
                    mButton.SetElevation((float)_backgroundElement.Elevation, UIControlState.Normal);
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
                    if (_defaultShape is RoundRect rRect)
                    {
                        rRect.CornerRadius = _backgroundElement.CornerRadius;
                    }
                    InvalidateClipToBounds();
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

            InvalidateClipToBounds();
        }

        #endregion

        #region Invalidation

        public void SetShape(IBackgroundShape shape, bool overwrite = true)
        {
            _shapeManager?.SetShape(_renderer, overwrite ? shape : _defaultShape);
        }

        public void InvalidateLayer()
        {
            var layer = _renderer.NativeView.FindLayerOfType<GradientStrokeLayer>();
            if (layer == null) return;

            //Disable CALayer Animations
            CATransaction.Begin();
            CATransaction.DisableActions = true;
            layer.Frame = _renderer.NativeView.Bounds;
            CATransaction.Commit();
            
            layer.SetNeedsDisplay();

            InvalidateClipToBounds();
            EnsureRippleOnFront();
        }

        public void InvalidateClipToBounds()
        {
            if (_renderer.NativeView == null || _backgroundElement == null) return;

            _shapeManager.Invalidate();
            _shapeManager.InvalidateInkLayer(_backgroundElement.IsRippleEnabled);
        }      

        public void EnsureRippleOnFront()
        {
            if (_renderer.NativeView?.Subviews == null || _renderer.NativeView.Subviews.Length <= 0) return;

            foreach (var subView in _renderer.NativeView.Subviews)
            {
                if (!(subView is InkView)) continue;
                _renderer.NativeView.BringSubviewToFront(subView);
            }
        }

        #endregion

        #region LifeCycle

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed || !disposing) return;

            _disposed = true;

            SetVisualElement(_visualElement, null);

            _defaultShape = null;
            if (_shapeManager != null)
            {
                _shapeManager.Dispose();
                _shapeManager = null;
            }

            if (_renderer != null)
            {
                _renderer.ElementChanged -= OnRendererElementChanged;
                _renderer = null;
            }

            if (_inkTouchController != null)
            {
                _inkTouchController.CancelInkTouchProcessing();
                _inkTouchController?.Dispose();
                _inkTouchController = null;
            }
        }

        #endregion
    }
}