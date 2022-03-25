using System;
using FastProjector.Contracts;

namespace FastProjector.Ioc
{
    public abstract class ServiceFactory
    {
        protected readonly Func<IServiceResolver, object> FactoryMethod;
        protected readonly Type ServiceType;

        protected ServiceFactory(Func<IServiceResolver, object> factoryMethod, Type serviceType)
        {
            FactoryMethod = factoryMethod;
            ServiceType = serviceType;
        }


        public abstract object CreateInstance(IServiceRepository singletonRepository,
            IServiceRepository scopedRepository, IServiceResolver serviceResolver);

        public static ServiceFactory CreateSingleton<TService>(Func<IServiceResolver, TService> factory)
            where TService : class
        {
            return new SingletonServiceFactory(factory, typeof(TService));
        }

        public static ServiceFactory CreateScoped<TService>(Func<IServiceResolver, TService> factory)
            where TService : class
        {
            return new ScopedServiceFactory(factory, typeof(TService));
        }

        public static ServiceFactory CreateTransient<TService>(Func<IServiceResolver, TService> factory)
            where TService : class
        {
            return new TransientServiceFactory(factory, typeof(TService));
        }
    }
}