using Xamarin.Forms;

namespace XamarinBackgroundKitSample.Models
{
    public class ColorSource : BindableObject
    {
        private Color _color;
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public ColorSource(Color color)
        {
            Color = color;
        }
    }
}
