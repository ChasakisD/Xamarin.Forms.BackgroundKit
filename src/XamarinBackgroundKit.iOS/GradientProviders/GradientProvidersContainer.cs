using System;
using System.Collections.Generic;
using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKit.iOS.GradientProviders
{
    public static class GradientProvidersContainer
    {
        private static bool _isInitialized;

        private static readonly Lazy<Dictionary<Type, Func<IGradientProvider>>> FactoriesLazy =
            new Lazy<Dictionary<Type, Func<IGradientProvider>>>(() => new Dictionary<Type, Func<IGradientProvider>>());

        private static Dictionary<Type, Func<IGradientProvider>> Factories => FactoriesLazy.Value;

        public static void Init()
        {
            _isInitialized = true;

            Register<LinearGradientBrush>(() => new LinearGradientProvider());
        }

        public static IGradientProvider Resolve<TGradientBrush>() where TGradientBrush : GradientBrush
        {
            return Resolve(typeof(TGradientBrush));
        }

        public static IGradientProvider Resolve(Type shapeType)
        {
            if (!_isInitialized)
            {
                Init();
            }

            if (!Factories.ContainsKey(shapeType))
                throw new Exception("Not found registered PathProvider");

            return Factories[shapeType]();
        }

        public static void Register<TGradientBrush>(Func<IGradientProvider> gradientProviderFactory) where TGradientBrush : GradientBrush
        {
            if (Factories.ContainsKey(typeof(TGradientBrush)))
            {
                Factories[typeof(TGradientBrush)] = gradientProviderFactory;
                return;
            }

            Factories.Add(typeof(TGradientBrush), gradientProviderFactory);
        }
    }
}
