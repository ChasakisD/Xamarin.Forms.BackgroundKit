using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;
using XamarinBackgroundKit.Controls.Base;
using IBorderElement = XamarinBackgroundKit.Abstractions.IBorderElement;

namespace XamarinBackgroundKit.Controls
{
    public class Background : BindableObject, IMaterialVisualElement
    {
        #region IElevation Properties

        public static readonly BindableProperty ElevationProperty = ElevationElement.ElevationProperty;

        /// <summary>
        /// Gets or sets the Elevation of the Background
        /// </summary>
		public double Elevation
        {
            get => (double)GetValue(ElevationProperty);
            set => SetValue(ElevationProperty, value);
        }

        public static readonly BindableProperty TranslationZProperty = ElevationElement.TranslationZProperty;

        /// <summary>
        /// Gets or sets the TranslationZ of the Background
        /// </summary>
		public double TranslationZ
        {
            get => (double)GetValue(TranslationZProperty);
            set => SetValue(TranslationZProperty, value);
        }

        #endregion

        #region ICornerElement Properties

        public static readonly BindableProperty CornerRadiusProperty = CornerElement.CornerRadiusProperty;

        /// <summary>
        /// Gets or sets the Elevation of the Background
        /// </summary>
		public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion

        #region IGradientElement Properties

        public static readonly BindableProperty AngleProperty = GradientElement.AngleProperty;

        /// <summary>
        /// Gets or sets the Angle of the Gradient of the Background
        /// </summary>
		public float Angle
        {
            get => (float)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        public static readonly BindableProperty GradientTypeProperty = GradientElement.GradientTypeProperty;

        /// <summary>
        /// Gets or sets the Type of the Gradient of the Background
        /// </summary>
		public GradientType GradientType
        {
            get => (GradientType)GetValue(GradientTypeProperty);
            set => SetValue(GradientTypeProperty, value);
        }

        public static readonly BindableProperty GradientsProperty = GradientElement.GradientsProperty;

        /// <summary>
        /// Gets or sets the Gradients of the Background
        /// </summary>
		public IList<GradientStop> Gradients
        {
            get => (IList<GradientStop>)GetValue(GradientsProperty);
            set => SetValue(GradientsProperty, value);
        }

        #endregion

        #region IBorderElement Properties

        public static readonly BindableProperty BorderColorProperty = BorderElement.BorderColorProperty;

        /// <summary>
        /// Gets or sets the Border Color of the Background
        /// </summary>
		public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty BorderWidthProperty = BorderElement.BorderWidthProperty;

        /// <summary>
        /// Gets or sets the Border Width of the Background
        /// </summary>
		public double BorderWidth
        {
            get => (double)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }

        #endregion

        public event EventHandler<EventArgs> InvalidateGradientRequested;

        public Background()
        {
            ((IGradientElement)this).OnGradientsPropertyChanged(null, Gradients);
        }
               
        #region GradientStop Changed

        private void GradientsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            InvalidateGradientRequested?.Invoke(this, EventArgs.Empty);

            if (e.OldItems != null)
            {
                foreach (var oldItem in e.OldItems)
                {
                    if (!(oldItem is GradientStop oldStop)) continue;

                    oldStop.PropertyChanged -= GradientStopPropertyChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems)
                {
                    if (!(newItem is GradientStop newStop)) continue;

                    newStop.PropertyChanged += GradientStopPropertyChanged;
                }
            }
        }

        private void GradientStopPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InvalidateGradientRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region IElevation Implementation

        void IElevationElement.OnElevationPropertyChanged(double oldValue, double newValue) { }

        void IElevationElement.OnTranslationZPropertyChanged(double oldValue, double newValue) { }

        #endregion

        #region ICornerElement Implementation

        void ICornerElement.OnCornerRadiusPropertyChanged(CornerRadius oldValue, CornerRadius newValue) { }

        #endregion

        #region IGradientElement Implementation

        void IGradientElement.OnAnglePropertyChanged(float oldValue, float newValue) { }

        void IGradientElement.OnGradientTypePropertyChanged(GradientType oldValue, GradientType newValue) { }

        void IGradientElement.OnGradientsPropertyChanged(IList<GradientStop> oldValue, IList<GradientStop> newValue)
        {
            if(oldValue != null)
            {
                if (oldValue is INotifyCollectionChanged oldCollection)
                {
                    oldCollection.CollectionChanged -= GradientsCollectionChanged;
                }

                foreach (var oldStop in oldValue)
                {
                    oldStop.PropertyChanged -= GradientStopPropertyChanged;
                }
            }

            if (newValue == null) return;

            if (newValue is INotifyCollectionChanged newCollection)
            {
                newCollection.CollectionChanged += GradientsCollectionChanged;
            }

            foreach (var newStop in newValue)
            {
                newStop.PropertyChanged += GradientStopPropertyChanged;
            }
        }

        #endregion

        #region IBorderElement Implementation

        void IBorderElement.OnBorderColorPropertyChanged(Color oldValue, Color newValue) { }

        void IBorderElement.OnBorderWidthPropertyChanged(double oldValue, double newValue) { }

        #endregion
    }
}
