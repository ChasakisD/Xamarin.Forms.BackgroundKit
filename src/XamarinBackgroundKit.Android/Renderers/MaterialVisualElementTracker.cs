using Android.Content;
using Android.Content.Res;
using Android.Support.Design.Card;
using Android.Support.Design.Chip;
using Android.Util;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Android.Extensions;
using XamarinBackgroundKit.Controls.Base;
using XamarinBackgroundKit.Extensions;

namespace XamarinBackgroundKit.Android.Renderers
{
    public class MaterialVisualElementTracker : VisualElementTracker
	{
        private int _oldGradientsCount = -1;

        private bool _disposed;
        private Context _context;
		private VisualElement _visualElement;
		private IVisualElementRenderer _renderer;
        private IMaterialVisualElement _backgroundElement;

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
            }

			_backgroundElement = newElement;
            if (newElement == null) return;
            
            _context = _renderer.View.Context;
            newElement.PropertyChanged += _propertyChangedHandler;
            newElement.InvalidateGradientRequested += InvalidateGradientsRequested;

            if (oldElement == null)
            {
                UpdateElevation();
                UpdateTranslationZ();
                UpdateGradients();
                UpdateBorder();
                UpdateCornerRadius();
                return;
            }
            
            var eps = Math.Pow(10, -10);

            if (Math.Abs(oldElement.Elevation - newElement.Elevation) > eps)
                UpdateElevation();

            if (Math.Abs(oldElement.TranslationZ - newElement.TranslationZ) > eps)
                UpdateTranslationZ();

            if (Math.Abs(oldElement.Angle - newElement.Angle) > eps
                || oldElement.GradientType != newElement.GradientType
                || oldElement.Gradients.AreEqual(newElement.Gradients))
                UpdateGradients();

            if (oldElement.BorderColor != newElement.BorderColor
                || Math.Abs(oldElement.BorderWidth - newElement.BorderWidth) > eps)
                UpdateBorder();

            if (oldElement.CornerRadius != newElement.CornerRadius)
                UpdateCornerRadius();
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
				|| e.PropertyName == GradientElement.GradientTypeProperty.PropertyName) UpdateGradients();
			else if (e.PropertyName == BorderElement.BorderColorProperty.PropertyName
				|| e.PropertyName == BorderElement.BorderWidthProperty.PropertyName) UpdateBorder();
			else if (e.PropertyName == ElevationElement.ElevationProperty.PropertyName) UpdateElevation();
			else if (e.PropertyName == CornerElement.CornerRadiusProperty.PropertyName) UpdateCornerRadius();
        }

		private void UpdateGradients()
		{
			if (_renderer?.View == null) return;

            var gradientCount = _backgroundElement?.Gradients?.Count ?? 0;

			switch (_renderer.View)
			{
				case Chip mChip when gradientCount == 0:
					mChip.ChipBackgroundColor = ColorStateList.ValueOf(_visualElement.BackgroundColor.ToAndroid());
					return;
				case MaterialCardView mCardView when gradientCount == 0:
					mCardView.CardBackgroundColor = ColorStateList.ValueOf(_visualElement.BackgroundColor.ToAndroid());
					return;
			}

			/* Chip does not accept background changes and does not support gradient */
			if (_renderer.View is Chip || gradientCount == 0) return;

			_renderer.View.SetGradient(_backgroundElement);

            if(_oldGradientsCount == -1)
            {
                UpdateBorder();
                UpdateCornerRadius();
            }

            _oldGradientsCount = gradientCount;
        }

        private void UpdateTranslationZ()
        {
            if (_renderer?.View == null) return;

            _renderer.View.SetTranslationZ(_context, _backgroundElement);
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

		private void UpdateCornerRadius()
		{
			if (_renderer?.View == null) return;

			switch (_renderer.View)
			{
				case Chip mChip:
					mChip.SetCornerRadius(_context, _backgroundElement);
					break;
				case MaterialCardView mCardView:
					mCardView.SetCornerRadius(_context, _backgroundElement);
					break;
				default:
					_renderer.View.SetCornerRadius(_context, _visualElement, _backgroundElement);
					break;
			}
		}

		private void UpdateBorder()
		{
			if (_renderer?.View == null) return;

			switch (_renderer.View)
			{
				case Chip mChip:
					mChip.SetBorder(_context, _backgroundElement);
					break;
				case MaterialCardView mCardView:
					mCardView.SetBorder(_context, _backgroundElement);
					break;
				default:
					_renderer.View.SetBorder(_context, _visualElement, _backgroundElement);
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
					_renderer = null;
					_context = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
