using System;
using Android.Graphics;
using Android.Views;
using XamarinBackgroundKit.Android.PathProviders;

namespace XamarinBackgroundKit.Android.OutlineProviders
{
    public class PathOutlineProvider : ViewOutlineProvider
    {
        private readonly WeakReference<IPathProvider> _pathProvider;

        public PathOutlineProvider(IPathProvider pathProvider)
        {
            _pathProvider = new WeakReference<IPathProvider>(pathProvider);
        }

        public override void GetOutline(View view, Outline outline)
        {
            if (!_pathProvider.TryGetTarget(out var pathProvider) || pathProvider.Path == null) return;

            if (pathProvider is RectPathProvider)
            {
                outline.SetRect(0, 0, view.Width, view.Height);
                return;
            }

            if (pathProvider is RoundRectPathProvider roundRectPathProvider)
            {
                /* If corner radius is uniform, use view outline provider */
                if (roundRectPathProvider.CanHandledByOutline)
                {
                    outline.SetRoundRect(0, 0, view.Width, view.Height, roundRectPathProvider.CornerRadii[0]);
                    return;
                }
            }

            var clipPath = pathProvider.CreatePath(view.Width, view.Height);
            if (clipPath == null || !clipPath.IsConvex) return;

            outline.SetConvexPath(clipPath);
        }
    }
}
