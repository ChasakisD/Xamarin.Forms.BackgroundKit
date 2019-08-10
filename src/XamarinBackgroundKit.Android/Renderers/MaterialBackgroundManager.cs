using System;
using System.ComponentModel;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Button;
using Android.Support.Design.Card;
using Android.Support.Design.Chip;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Android.Extensions;
using XamarinBackgroundKit.Controls;
using XamarinBackgroundKit.Controls.Base;
using XamarinBackgroundKit.Effects;
using XamarinBackgroundKit.Extensions;
using AApp = Android.App.Application;
using AColor = Android.Graphics.Color;
using AView = Android.Views.View;
using Color = Xamarin.Forms.Color;

namespace XamarinBackgroundKit.Android.Renderers
{
    public class MaterialBackgroundManager : IDisposable
    {
        private bool _areDefaultsSet;
        private bool _defaultFocusable;
        private bool _defaultClickable;

        private bool _disposed;
        private Context _context;
        private AView _nativeView;
        private VisualElement _visualElement;
        private IVisualElementRenderer _renderer;
        private IMaterialVisualElement _backgroundElement;

        private static double _density = -1;

        private static readonly int[][] ButtonStates =
        {
            new [] { global::Android.Resource.Attribute.StateEnabled },
            new int[] { }
        };

        public MaterialBackgroundManager(IVisualElementRenderer renderer)
        {
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer), "Renderer cannot be null");

            _renderer.ElementChanged += OnRendererElementChanged;

            SetVisualElement(null, _renderer.Element);
        }

        #region Element Setters

        private void ResolveNativeView()
        {
            switch (_renderer)
            {
                case IBorderVisualElementRenderer borderRenderer:
                    _nativeView = borderRenderer.View;
                    break;
                case IButtonLayoutRenderer buttonRenderer:
                    _nativeView = buttonRenderer.View;
                    break;
                case EntryRenderer entryRenderer:
                    _nativeView = entryRenderer.Control;
                    break;
                default:
                    _nativeView = _renderer.View;
                    break;
            }
        }

        private void SetVisualElement(VisualElement oldElement, VisualElement newElement)
        {
            if (oldElement != null)
            {
                _context = null;
                _nativeView = null;
                oldElement.PropertyChanged -= OnElementPropertyChanged;
            }

            if (newElement != null)
            {
                ResolveNativeView();
                _context = _nativeView.Context;
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

        public void SetBackgroundElement(IMaterialVisualElement oldElement, IMaterialVisualElement newElement)
        {
            if (oldElement != null)
            {
                oldElement.PropertyChanged -= OnRendererPropertyChanged;
                oldElement.InvalidateGradientRequested -= InvalidateGradientsRequested;
                oldElement.InvalidateBorderGradientRequested -= InvalidateBorderGradientsRequested;
            }

            _backgroundElement = newElement;

            if (newElement == null) return;

            newElement.PropertyChanged += OnRendererPropertyChanged;
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

            if (oldElement.GradientBrush != newElement.GradientBrush)
                UpdateGradients();

            if (oldElement.BorderColor != newElement.BorderColor
                || oldElement.BorderStyle != newElement.BorderStyle
                || Math.Abs(oldElement.BorderWidth - newElement.BorderWidth) > eps
                || oldElement.BorderGradientBrush != newElement.BorderGradientBrush)
                UpdateBorder();

            if (oldElement.CornerRadius != newElement.CornerRadius)
                UpdateCornerRadius();
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

        private void OnRendererPropertyChanged(object sender, PropertyChangedEventArgs e)
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
            else if (e.PropertyName == ElevationElement.TranslationZProperty.PropertyName) UpdateTranslationZ();
        }

        #endregion

        #region Background Handling

        private void UpdateBackground()
        {
            if (_nativeView == null || _backgroundElement == null) return;
            if (_nativeView is MaterialCardView || _nativeView is Chip || _nativeView is MaterialButton) return;

            if (!_areDefaultsSet)
            {
                _areDefaultsSet = true;
                _defaultClickable = _nativeView.Clickable;
                _defaultFocusable = _nativeView.Focusable;
            }

            _nativeView.Background?.Dispose();
            _nativeView.Background = new GradientStrokeDrawable(GetDensity(), _backgroundElement);

            UpdateRipple();
            InvalidateOutline();
        }

        private void UpdateColor()
        {
            if (_nativeView == null || _backgroundElement == null || _visualElement == null) return;

            var color = _backgroundElement.Color == Color.Default
                ? _visualElement.BackgroundColor == Color.Default
                    ? Color.Default
                    : _visualElement.BackgroundColor
                : _backgroundElement.Color;

            if (color == Color.Default) return;

            switch (_nativeView)
            {
                case MaterialCardView mCardView:
                    mCardView.CardBackgroundColor = ColorStateList.ValueOf(color.ToAndroid());
                    break;
                case Chip mChip:
                    mChip.ChipBackgroundColor = ColorStateList.ValueOf(color.ToAndroid());
                    break;
                case MaterialButton mButton:
                    var primaryColor = color.ToAndroid();
                    var alphaPrimaryColor = new AColor(
                        primaryColor.R, primaryColor.G, primaryColor.B, (byte)(0.12 * 255));
                    ViewCompat.SetBackgroundTintList(mButton,
                        new ColorStateList(ButtonStates, new int[] { primaryColor, alphaPrimaryColor }));
                    break;
                default:
                    _nativeView.SetColor(color);
                    break;
            }
        }

        private void UpdateGradients()
        {
            if (_nativeView == null || _backgroundElement == null) return;
            if (_nativeView is MaterialCardView || _nativeView is Chip || _nativeView is MaterialButton) return;

            _nativeView.SetGradient(_backgroundElement);
        }

        private void UpdateBorder()
        {
            if (_nativeView == null || _backgroundElement == null) return;

            switch (_nativeView)
            {
                case MaterialCardView mCardView:
                    mCardView.SetBorder(_context, _backgroundElement);
                    break;
                case Chip mChip:
                    mChip.SetBorder(_context, _backgroundElement);
                    break;
                case MaterialButton mButton:
                    mButton.SetBorder(_context, _backgroundElement);
                    break;
                default:
                    _nativeView.SetBorder(_backgroundElement);
                    break;
            }
        }

        private void UpdateCornerRadius()
        {
            if (_nativeView == null || _backgroundElement == null) return;

            switch (_nativeView)
            {
                case MaterialCardView mCardView:
                    mCardView.SetCornerRadius(_context, _backgroundElement);
                    break;
                case Chip mChip:
                    mChip.SetCornerRadius(_context, _backgroundElement);
                    break;
                case MaterialButton mButton:
                    mButton.SetCornerRadius(_context, _backgroundElement);
                    break;
                default:
                    _nativeView.SetCornerRadius(_backgroundElement);
                    InvalidateOutline();
                    break;
            }
        }

        private void UpdateRipple()
        {
            if (Build.VERSION.SdkInt <= BuildVersionCodes.LollipopMr1) return;

            if (_nativeView == null || _backgroundElement == null) return;
            if (_nativeView is MaterialCardView || _nativeView is Chip || _nativeView is MaterialButton) return;

            switch (_nativeView?.Foreground)
            {
                case RippleDrawable _ when !_backgroundElement.IsRippleEnabled:
                    _nativeView.Foreground?.Dispose();
                    _nativeView.Foreground = null;
                    _nativeView.Clickable = _defaultClickable;
                    _nativeView.Focusable = _defaultFocusable;
                    break;
                case RippleDrawable oldRippleDrawable:
                    oldRippleDrawable.SetColor(ColorStateList.ValueOf(_backgroundElement.RippleColor.ToAndroid()));
                    break;
                case null when _backgroundElement.IsRippleEnabled:
                    var maskDrawable = new GradientDrawable();
                    maskDrawable.SetShape(ShapeType.Rectangle);
                    maskDrawable.SetColor(new AColor(0, 0, 0, 255));
                    maskDrawable.SetCornerRadii(_backgroundElement.CornerRadius.ToRadii(
                        _context.Resources.DisplayMetrics.Density));
                    var rippleColorStateList = ColorStateList.ValueOf(_backgroundElement.RippleColor.ToAndroid());
                    _nativeView.Foreground = new RippleDrawable(rippleColorStateList, null, maskDrawable);
                    _nativeView.Clickable = true;
                    _nativeView.Focusable = true;
                    break;
            }
        }

        private void UpdateTranslationZ()
        {
            _nativeView?.SetTranslationZ(_context, _backgroundElement);
        }

        private void UpdateElevation()
        {
            if (_nativeView == null) return;

            switch (_nativeView)
            {
                case MaterialCardView mCardView:
                    mCardView.SetElevation(_context, _backgroundElement);
                    break;
                default:
                    _nativeView.SetElevation(_context, _backgroundElement);
                    break;
            }
        }

        #endregion

        #region Invalidation

        private void InvalidateOutline()
        {
            var cornerRadii = _backgroundElement.CornerRadius.ToRadii(_context.Resources.DisplayMetrics.Density);

            _nativeView.OutlineProvider?.Dispose();
            _nativeView.OutlineProvider = new CornerOutlineProvider(cornerRadii);

            _nativeView.ClipToOutline = true;
        }

        #endregion

        #region LifeCycle

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

            if (_renderer == null) return;
            _renderer.ElementChanged -= OnRendererElementChanged;

            _context = null;
            _nativeView = null;
            _renderer = null;
        }

        #endregion

        #region Helpers

        private static double GetDensity()
        {
            if (_density > 0) return _density;

            using (var displayMetrics = new DisplayMetrics())
            {
                var windowService = AApp.Context.GetSystemService(Context.WindowService)
                    ?.JavaCast<IWindowManager>();

                using (var display = windowService?.DefaultDisplay)
                {
                    if (display == null) return _density;

                    display.GetRealMetrics(displayMetrics);
                    _density = displayMetrics.Density;
                    return _density;
                }
            }
        }

        #endregion
    }
}
