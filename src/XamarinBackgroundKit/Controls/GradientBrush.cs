using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Xamarin.Forms;
using XamarinBackgroundKit.Abstractions;

namespace XamarinBackgroundKit.Controls
{
    [ContentProperty(nameof(Gradients))]
    public abstract class GradientBrush : BindableObject
    {
        public static readonly BindableProperty GradientsProperty = BindableProperty.Create(
            nameof(Gradients), typeof(IList<GradientStop>), typeof(IGradientElement),
            propertyChanged: (b, o, n) => ((GradientBrush)b)?.OnGradientsPropertyChanged((IList<GradientStop>)o, (IList<GradientStop>)n),
            defaultValueCreator: b => new ObservableCollection<GradientStop>());

        /// <summary>
        /// Gets or sets the Gradients of the Background
        /// </summary>
        public IList<GradientStop> Gradients
        {
            get => (IList<GradientStop>)GetValue(GradientsProperty);
            set => SetValue(GradientsProperty, value);
        }

        public static readonly BindableProperty AngleProperty = BindableProperty.Create(
            nameof(Angle), typeof(float), typeof(IGradientElement), 0f,
            propertyChanged: (b, o, n) => ((GradientBrush)b)?.InvalidateRequested());

        /// <summary>
        /// Gets or sets the Angle of the Gradient of the Background
        /// </summary>
        public float Angle
        {
            get => (float)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        public event EventHandler<EventArgs> InvalidateGradientRequested;

        protected GradientBrush()
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

            InvalidateRequested();
        }

        private void GradientsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
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

            InvalidateRequested();
        }

        private void GradientStopPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            InvalidateRequested();   
        }

        public virtual void InvalidateRequested()
        {
            InvalidateGradientRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
