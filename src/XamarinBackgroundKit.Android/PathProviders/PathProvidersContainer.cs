using System;
using System.Collections.Generic;
using XamarinBackgroundKit.Shapes;

namespace XamarinBackgroundKit.Android.PathProviders
{
    public static class PathProvidersContainer
    {
        private static bool _isInitialized;

        private static readonly Lazy<Dictionary<Type, Func<IPathProvider>>> FactoriesLazy =
            new Lazy<Dictionary<Type, Func<IPathProvider>>>(() => new Dictionary<Type, Func<IPathProvider>>());

        private static Dictionary<Type, Func<IPathProvider>> Factories => FactoriesLazy.Value;

        public static void Init()
        {
            _isInitialized = true;

            Register<Arc>(() => new ArcPathProvider());
            Register<Rect>(() => new RectPathProvider());
            Register<Circle>(() => new CirclePathProvider());
            Register<Diagonal>(() => new DiagonalPathProvider());
            Register<Triangle>(() => new TrianglePathProvider());
            Register<RoundRect>(() => new RoundRectPathProvider());
            Register<CornerClip>(() => new CornerClipPathProvider());
        }

        public static IPathProvider Resolve<TShape>() where TShape : IBackgroundShape
        {
            return Resolve(typeof(TShape));
        }

        public static IPathProvider Resolve(Type shapeType)
        {
            if (!_isInitialized)
            {
                Init();
            }

            if (!Factories.ContainsKey(shapeType))
                throw new Exception("Not found registered PathProvider");

            return Factories[shapeType]();
        }

        public static void Register<TShape>(Func<IPathProvider> pathProviderFactory) where TShape : IBackgroundShape
        {
            if (Factories.ContainsKey(typeof(TShape)))
            {
                Factories[typeof(TShape)] = pathProviderFactory;
                return;
            }

            Factories.Add(typeof(TShape), pathProviderFactory);
        }
    }
}
