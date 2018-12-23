using System;
using System.ComponentModel;
using System.Linq;
using Android.Content;
using Android.Content.Res;
using Android.Support.Design.Card;
using Android.Support.Design.Chip;
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
		private bool _disposed;
		private Context _context;
		private VisualElement _element;
		private IVisualElementRenderer _renderer;

		private readonly PropertyChangedEventHandler _propertyChangedHandler;

		private IMaterialVisualElement ElementController => _element as IMaterialVisualElement;

		public MaterialVisualElementTracker(IVisualElementRenderer renderer) : base(renderer)
		{
			_renderer = renderer ?? throw new ArgumentNullException(nameof(renderer), "Renderer cannot be null");

			_propertyChangedHandler = OnRendererElementPropertyChanged;

			_context = renderer.View.Context;
			_renderer.ElementChanged += OnRendererElementChanged;

			SetElement(null, _renderer.Element);
		}

		private void SetElement(VisualElement oldElement, VisualElement newElement)
		{
			if (oldElement != null)
			{
				_context = null;
				oldElement.PropertyChanged -= _propertyChangedHandler;
			}

			_element = newElement;
            if (newElement == null) return;
            
            _context = _renderer.View.Context;
            newElement.PropertyChanged += _propertyChangedHandler;

            if (oldElement == null)
            {
                UpdateElevation();
                UpdateGradients();
                UpdateBorder();
                UpdateCornerRadius();
                return;
            }

            if (!(oldElement is IMaterialVisualElement oldSupportElement)) return;
            if (!(newElement is IMaterialVisualElement newSupportElement)) return;

            var eps = Math.Pow(10, -10);

            if (Math.Abs(oldSupportElement.Elevation - newSupportElement.Elevation) > eps)
                UpdateElevation();

            if (Math.Abs(oldSupportElement.Angle - newSupportElement.Angle) > eps
                || oldSupportElement.GradientType != newSupportElement.GradientType
                || oldSupportElement.Gradients.AreEqual(newSupportElement.Gradients))
                UpdateGradients();

            if (oldSupportElement.BorderColor != newSupportElement.BorderColor
                || Math.Abs(oldSupportElement.BorderWidth - newSupportElement.BorderWidth) > eps)
                UpdateBorder();

            if (oldSupportElement.CornerRadius != newSupportElement.CornerRadius)
                UpdateCornerRadius();
        }

		private void OnRendererElementChanged(object sender, VisualElementChangedEventArgs args)
		{
			SetElement(args.OldElement, args.NewElement);
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

			var emptyGradients = ElementController.Gradients == null || !ElementController.Gradients.Any();

			switch (_renderer.View)
			{
				case Chip mChip when emptyGradients:
					mChip.ChipBackgroundColor = ColorStateList.ValueOf(_element.BackgroundColor.ToAndroid());
					return;
				case MaterialCardView mCardView when emptyGradients:
					mCardView.CardBackgroundColor = ColorStateList.ValueOf(_element.BackgroundColor.ToAndroid());
					return;
			}

			/* Chip does not accept background changes and does not support gradient */
			if (_renderer.View is Chip) return;

			_renderer.View.SetGradient(_context, _element);
		}

		private void UpdateElevation()
		{
			if (_renderer?.View == null) return;

			switch (_renderer.View)
			{
				case MaterialCardView mCardView:
					mCardView.SetElevation(_context, ElementController);
					break;
				default:
					_renderer.View.SetElevation(_context, ElementController);
					break;
			}
		}

		private void UpdateCornerRadius()
		{
			if (_renderer?.View == null) return;

			switch (_renderer.View)
			{
				case Chip mChip:
					mChip.SetCornerRadius(_context, ElementController);
					break;
				case MaterialCardView mCardView:
					mCardView.SetCornerRadius(_context, ElementController);
					break;
				default:
					_renderer.View.SetCornerRadius(_context, ElementController);
					break;
			}
		}

		private void UpdateBorder()
		{
			if (_renderer?.View == null) return;

			switch (_renderer.View)
			{
				case Chip mChip:
					mChip.SetBorder(_context, ElementController);
					break;
				case MaterialCardView mCardView:
					mCardView.SetBorder(_context, ElementController);
					break;
				default:
					_renderer.View.SetBorder(_context, _element, ElementController);
					break;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (_disposed) return;

			_disposed = true;

			if (disposing)
			{
				SetElement(_element, null);

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
