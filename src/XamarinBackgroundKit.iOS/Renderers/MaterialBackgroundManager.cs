﻿using System;
using System.ComponentModel;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using MaterialComponents;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Controls.Base;
using XamarinBackgroundKit.Effects;
using XamarinBackgroundKit.iOS.Extensions;
using MButton = MaterialComponents.Button;

namespace XamarinBackgroundKit.iOS.Renderers
{
    public class MaterialBackgroundManager : IDisposable
    {
        private bool _disposed;
        private VisualElement _visualElement;
        private IVisualElementRenderer _renderer;
        private IMaterialVisualElement _backgroundElement;

        private InkTouchController _inkTouchController;
        
        private readonly PropertyChangedEventHandler _propertyChangedHandler;

        public MaterialBackgroundManager(IVisualElementRenderer renderer)
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

        public void SetBackgroundElement(IMaterialVisualElement oldElement, IMaterialVisualElement newElement)
        {
            if (oldElement != null)
            {
                oldElement.PropertyChanged -= _propertyChangedHandler;
                oldElement.InvalidateGradientRequested -= InvalidateGradientsRequested;
                oldElement.InvalidateBorderGradientRequested -= InvalidateBorderGradientsRequested;
            }

            _backgroundElement = newElement;
            if (newElement == null) return;

            newElement.PropertyChanged += _propertyChangedHandler;
            newElement.InvalidateGradientRequested += InvalidateGradientsRequested;
            newElement.InvalidateBorderGradientRequested += InvalidateBorderGradientsRequested;

            if (oldElement == null)
            {
                _renderer.NativeView.FindLayerOfType<GradientStrokeLayer>();
                
                UpdateColor();
                UpdateGradients();
                UpdateBorder();
                UpdateElevation();
                UpdateCornerRadius();
                UpdateRipple();
                InvalidateClipToBounds();
                return;
            }
            
            var eps = Math.Pow(10, -10);

            if (oldElement.Color != newElement.Color)
                UpdateColor();

            if (Math.Abs(oldElement.Elevation - newElement.Elevation) > eps)
                UpdateElevation();

            if (oldElement.GradientBrush != newElement.GradientBrush)
                UpdateGradients();

            if (oldElement.BorderColor != newElement.BorderColor
                || Math.Abs(oldElement.BorderWidth - newElement.BorderWidth) > eps
                || oldElement.BorderGradientBrush != newElement.BorderGradientBrush)
                UpdateBorder();

            if (oldElement.CornerRadius != newElement.CornerRadius)
                UpdateCornerRadius();

            InvalidateClipToBounds();
        }

        public void InvalidateLayer()
        {
            var layer = _renderer.NativeView.FindLayerOfType<GradientStrokeLayer>();
            if (layer == null) return;

            layer.Frame = _renderer.NativeView.Bounds;

            InvalidateClipToBounds();
            EnsureRippleOnFront();
        }
        
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

        private void OnRendererElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_renderer == null || _disposed) return;

            if (e.PropertyName == BorderElement.BorderColorProperty.PropertyName
                     || e.PropertyName == BorderElement.BorderWidthProperty.PropertyName
                     || e.PropertyName == BorderElement.DashGapProperty.PropertyName
                     || e.PropertyName == BorderElement.DashWidthProperty.PropertyName) UpdateBorder();
            else if (e.PropertyName == CornerElement.CornerRadiusProperty.PropertyName) UpdateCornerRadius();
            else if (e.PropertyName == Background.ColorProperty.PropertyName
                     || e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName) UpdateColor();
            else if (e.PropertyName == Background.IsRippleEnabledProperty.PropertyName
                     || e.PropertyName == Background.RippleColorProperty.PropertyName) UpdateRipple();
            else if (e.PropertyName == ElevationElement.ElevationProperty.PropertyName) UpdateElevation();
            else if (e.PropertyName == Background.IsClippedToBoundsProperty.PropertyName) InvalidateClipToBounds();
        }

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
                    mButton.SetElevation((float) _backgroundElement.Elevation, UIControlState.Normal);
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
        }

        public void InvalidateClipToBounds()
        {
            if (_renderer.NativeView == null) return;
            if (_renderer.NativeView.Layer != null)
            {
                /*
                 * MDCInkView
                 * From https://github.com/material-components/material-components-ios-codelabs/blob/master/MDC-111/Swift/Starter/Pods/MaterialComponents/components/Ink/src/MDCInkView.m
                 * MDCInkView uses SuperView's ShadowPath in order to mask the ripple
                 * So we calculate the rounded corners path and we set it to the ShadowPath
                 * but with ShadowOpacity to 0 in order to not overlap the MDCShadowLayer
                 */

                _renderer.NativeView.Layer.ShadowPath?.Dispose();
                if (_backgroundElement.IsRippleEnabled)
                {
                    _renderer.NativeView.Layer.ShadowOpacity = 0;
                    _renderer.NativeView.Layer.ShadowPath = BackgroundKit
                        .GetRoundCornersPath(_renderer.NativeView.Layer.Bounds, _backgroundElement.CornerRadius).CGPath;
                }
                else
                {
                    _renderer.NativeView.Layer.ShadowPath = null;
                }
            }
            
            if (_renderer.NativeView.Subviews == null) return;
            foreach (var subView in _renderer.NativeView.Subviews)
            {
                ApplyMaskToView(subView, _renderer.NativeView.Bounds);
            }
        }

        private void ApplyMaskToView(UIView view, CGRect bounds)
        {
            if (view?.Layer == null) return;

            view.Layer.Mask?.Dispose();
            if (_backgroundElement.IsClippedToBounds && 
                view.Layer.Sublayers?.FirstOrDefault(l => l is GradientStrokeLayer) == null)
            {
                view.Layer.Mask = new CAShapeLayer
                {
                    Frame = bounds,
                    Path = BackgroundKit
                        .GetRoundCornersPath(bounds, _backgroundElement.CornerRadius).CGPath
                };
                view.Layer.MasksToBounds = true;
            }
            else
            {
                view.Layer.Mask = null;
                view.Layer.MasksToBounds = false;
            }
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

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            _disposed = true;

            if (!disposing) return;
            SetVisualElement(_visualElement, null);

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
    }
}