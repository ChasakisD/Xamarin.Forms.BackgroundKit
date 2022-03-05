using Xamarin.Forms;

namespace XamarinBackgroundKitSample.Models.Issue90
{
    internal class Issue90ItemModel
    {
        public string Data { get; }
        public bool IsFirst { get; }
        public bool IsLast { get; }

        public CornerRadius CornerRadius
        {
            get
            {
                var topRadius = IsFirst ? 20 : 0;
                var bottomRadius = IsLast ? 20 : 0;
                return new CornerRadius(topRadius, topRadius, bottomRadius, bottomRadius);
            }
        }

        public Issue90ItemModel(string data, bool isFirst, bool isLast)
        {
            Data = data;
            IsFirst = isFirst;
            IsLast = isLast;
        }
    }
}
