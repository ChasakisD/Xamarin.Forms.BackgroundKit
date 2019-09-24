using System;
using System.Collections.Generic;
using XamarinBackgroundKit.Shapes;

namespace XamarinBackgroundKit.iOS.PathProviders
{
    public static class PathProvidersContainer
    {
        private static readonly Lazy<Dictionary<Type, Func<IPathProvider>>> FactoriesLazy =
            new Lazy<Dictionary<Type, Func<IPathProvider>>>(() => new Dictionary<Type, Func<IPathProvider>>());

        private static Dictionary<Type, Func<IPathProvider>> Factories => FactoriesLazy.Value;

        public static void Init()
        {
            Register<Arc>(() => new ArcPathProvider());
            Register<Rect>(() => new RectPathProvider());
            Register<Diagonal>(() => new DiagonalPathProvider());
            Register<RoundRect>(() => new RoundRectPathProvider());
        }

        public static IPathProvider Resolve<TShape>() where TShape : IBackgroundShape
        {
            return Resolve(typeof(TShape));
        }

        public static IPathProvider Resolve(Type shapeType)
        {
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
