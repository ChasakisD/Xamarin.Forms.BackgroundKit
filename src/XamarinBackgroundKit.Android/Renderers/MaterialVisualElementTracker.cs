using Android.Content;
using Android.Content.Res;
using Android.Support.Design.Card;
using Android.Support.Design.Chip;
using System;
using System.ComponentModel;
using Android.Graphics.Drawables;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Android.Extensions;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Controls.Base;
using XamarinBackgroundKit.Extensions;

namespace XamarinBackgroundKit.Android.Renderers
{
    public class MaterialVisualElementTracker : VisualElementTracker
	{
        private bool _disposed;
        private Context _context;
		private VisualElement _visualElement;
		private IVisualElementRenderer _renderer;
        private IMaterialVisualElement _backgroundElement;
        private GradientStrokeDrawable _lastGradientStrokeDrawable;

        private readonly PropertyChangedEventHandler _propertyChangedHandler;

		public MaterialVisualElementTracker(IVisualElementRenderer renderer) : base(renderer)
		{
			_renderer = renderer ?? throw new ArgumentNullException(nameof(renderer), "Renderer cannot be null");

			_propertyChangedHandler = OnRendererElementPropertyChanged;

			_context = renderer.View.Context;
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
                _context = null;
                oldElement.PropertyChanged -= _propertyChangedHandler;
                oldElement.InvalidateGradientRequested -= InvalidateGradientsRequested;
                oldElement.InvalidateBorderGradientRequested -= InvalidateBorderGradientsRequested;
            }

			_backgroundElement = newElement;
            if (newElement == null) return;
            
            _context = _renderer.View.Context;
            newElement.PropertyChanged += _propertyChangedHandler;
            newElement.InvalidateGradientRequested += InvalidateGradientsRequested;
            newElement.InvalidateBorderGradientRequested += InvalidateBorderGradientsRequested;
            
            if (oldElement == null)
            {
                UpdateBackground();
                UpdateElevation();
                UpdateTranslationZ();
                return;
            }

            var eps = Math.Pow(10, -10);

            if (oldElement.Color != newElement.Color)
                UpdateColor();

            if (Math.Abs(oldElement.Elevation - newElement.Elevation) > eps)
                UpdateElevation();

            if (Math.Abs(oldElement.TranslationZ - newElement.TranslationZ) > eps)
                UpdateTranslationZ();

            if (Math.Abs(oldElement.Angle - newElement.Angle) > eps
                || oldElement.GradientType != newElement.GradientType
                || oldElement.Gradients.AreEqual(newElement.Gradients))
                UpdateGradients();

            if (oldElement.BorderColor != newElement.BorderColor
                || Math.Abs(oldElement.BorderWidth - newElement.BorderWidth) > eps
                || Math.Abs(oldElement.BorderAngle - newElement.BorderAngle) > eps
                || oldElement.BorderGradientType != newElement.BorderGradientType
                || oldElement.BorderGradients.AreEqual(newElement.BorderGradients))
                UpdateBorder();

            if (oldElement.CornerRadius != newElement.CornerRadius)
                UpdateCornerRadius();
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

			if (e.PropertyName == GradientElement.AngleProperty.PropertyName
				|| e.PropertyName == GradientElement.GradientsProperty.PropertyName
				|| e.PropertyName == GradientElement.GradientTypeProperty.PropertyName) UpdateGradients();
			else if (e.PropertyName == BorderElement.BorderColorProperty.PropertyName
				|| e.PropertyName == BorderElement.BorderWidthProperty.PropertyName
                || e.PropertyName == BorderElement.BorderAngleProperty.PropertyName
                || e.PropertyName == BorderElement.BorderGradientsProperty.PropertyName
                || e.PropertyName == BorderElement.BorderGradientTypeProperty.PropertyName
				|| e.PropertyName == BorderElement.DashGapProperty.PropertyName
				|| e.PropertyName == BorderElement.DashWidthProperty.PropertyName) UpdateBorder();
			else if (e.PropertyName == CornerElement.CornerRadiusProperty.PropertyName) UpdateCornerRadius();
			else if (e.PropertyName == Background.ColorProperty.PropertyName
                    || e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName) UpdateColor();
			else if (e.PropertyName == Background.IsRippleEnabledProperty.PropertyName
				|| e.PropertyName == Background.RippleColorProperty.PropertyName) UpdateRipple();
			else if (e.PropertyName == ElevationElement.ElevationProperty.PropertyName) UpdateElevation();
			else if (e.PropertyName == ElevationElement.TranslationZProperty.PropertyName) UpdateTranslationZ();
        }

        private void UpdateBackground()
        {
            if (_renderer?.View == null || _backgroundElement == null) return;
            if (_renderer.View is MaterialCardView || _renderer.View is Chip) return;

            _renderer.View.Background?.Dispose();

            _lastGradientStrokeDrawable = new GradientStrokeDrawable(_context, _backgroundElement);

            if (_backgroundElement.IsRippleEnabled)
            {
                _renderer.View.Background = new RippleDrawable(
                    ColorStateList.ValueOf(_backgroundElement.RippleColor.ToAndroid()),
                    _lastGradientStrokeDrawable,
                    null);
            }
            else
            {
                _renderer.View.Background = _lastGradientStrokeDrawable;
            }
        }

        private void UpdateColor()
		{
			if (_renderer?.View == null || _backgroundElement == null || _visualElement == null) return;
		
			var color = _backgroundElement.Color == Color.Default
				? _visualElement.BackgroundColor == Color.Default
					? Color.Default
					: _visualElement.BackgroundColor
				: _backgroundElement.Color;

			if (color == Color.Default) return;

			switch (_renderer.View)
			{
				case MaterialCardView mCardView:
					mCardView.CardBackgroundColor = ColorStateList.ValueOf(color.ToAndroid());
					break;
				case Chip mChip:
					mChip.ChipBackgroundColor = ColorStateList.ValueOf(color.ToAndroid());
					break;
				default:
					_renderer.View.SetColor(color);
					break;
			}
		}

		private void UpdateGradients()
		{
			if (_renderer?.View == null || _backgroundElement == null) return;
			if (_renderer.View is MaterialCardView || _renderer.View is Chip) return;
			
			_renderer.View.SetGradient(_backgroundElement);
		}

		private void UpdateBorder()
		{
			if (_renderer?.View == null || _backgroundElement == null) return;
			
			switch (_renderer.View)
			{
				case MaterialCardView mCardView:
					mCardView.SetBorder(_context, _backgroundElement);
					break;
				case Chip mChip:
					mChip.SetBorder(_context, _backgroundElement);
					break;
				default:
					_renderer.View.SetBorder(_backgroundElement);
					break;
			}
		}

		private void UpdateCornerRadius()
		{
			if (_renderer?.View == null || _backgroundElement == null) return;
			
			switch (_renderer.View)
			{
				case MaterialCardView mCardView:
					mCardView.SetCornerRadius(_context, _backgroundElement);
					break;
				case Chip mChip:
					mChip.SetCornerRadius(_context, _backgroundElement);
					break;
				default:
					_renderer.View.SetCornerRadius(_backgroundElement);
					break;
			}
		}

        private void UpdateRipple()
        {
            if (_renderer?.View == null || _backgroundElement == null) return;
            if (_renderer.View is MaterialCardView || _renderer.View is Chip) return;

            switch (_renderer.View?.Background)
            {
                case RippleDrawable _ when !_backgroundElement.IsRippleEnabled:
                    _renderer.View.Background?.Dispose();
                    _renderer.View.Background = _lastGradientStrokeDrawable;
                    break;
                case GradientStrokeDrawable _ when _backgroundElement.IsRippleEnabled:
                    _renderer.View.Background =
                        new RippleDrawable(ColorStateList.ValueOf(_backgroundElement.RippleColor.ToAndroid()),
                            _lastGradientStrokeDrawable, null);
                    break;
                case RippleDrawable oldRippleDrawable:
                    oldRippleDrawable.SetColor(ColorStateList.ValueOf(_backgroundElement.RippleColor.ToAndroid()));
                    break;
            }
        }

        private void UpdateTranslationZ()
        {
            _renderer?.View?.SetTranslationZ(_context, _backgroundElement);
        }

		private void UpdateElevation()
		{
			if (_renderer?.View == null) return;

			switch (_renderer.View)
			{
				case MaterialCardView mCardView:
					mCardView.SetElevation(_context, _backgroundElement);
					break;
				default:
					_renderer.View.SetElevation(_context, _backgroundElement);
					break;
			}
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

                    _context = null;
                    _renderer = null;
                    _lastGradientStrokeDrawable = null;
                }
			}

			base.Dispose(disposing);
		}
	}
}
