using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;

namespace XamarinBackgroundKit.Controls
{
    public class LinearGradientBrush : BindableObject
    {
        public static readonly BindableProperty AngleProperty = BindableProperty.Create(
            nameof(Angle), typeof(float), typeof(LinearGradientBrush), 0f,
            propertyChanged: (b, o, n) => ((LinearGradientBrush)b)?.InvalidateGradientRequested?.Invoke(b, EventArgs.Empty));

        /// <summary>
        /// Gets or sets the Angle of the Gradient of the Background
        /// </summary>
        public float Angle
        {
            get => (float)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        public static readonly BindableProperty GradientTypeProperty = BindableProperty.Create(
            nameof(GradientType), typeof(GradientType), typeof(LinearGradientBrush), GradientType.Linear,
            propertyChanged: (b, o, n) => ((LinearGradientBrush)b)?.InvalidateGradientRequested?.Invoke(b, EventArgs.Empty));

        /// <summary>
        /// Gets or sets the Type of the Gradient of the Background
        /// </summary>
        public GradientType GradientType
        {
            get => (GradientType)GetValue(GradientTypeProperty);
            set => SetValue(GradientTypeProperty, value);
        }

        public static readonly BindableProperty GradientsProperty = BindableProperty.Create(
            nameof(Gradients), typeof(IList<GradientStop>), typeof(IGradientElement),
            propertyChanged: (b, o, n) => ((LinearGradientBrush)b)?.OnGradientsPropertyChanged((IList<GradientStop>)o, (IList<GradientStop>)n),
            defaultValueCreator: b => new ObservableCollection<GradientStop>());

        /// <summary>
        /// Gets or sets the Gradients of the Background
        /// </summary>
        public IList<GradientStop> Gradients
        {
            get => (IList<GradientStop>)GetValue(GradientsProperty);
            set => SetValue(GradientsProperty, value);
        }

        public event EventHandler<EventArgs> InvalidateGradientRequested;

        public LinearGradientBrush()
        {
            OnGradientsPropertyChanged(null, Gradients);
        }

        private void OnGradientsPropertyChanged(IList<GradientStop> oldValue, IList<GradientStop> newValue)
        {
            if (oldValue != null)
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
    }
}
