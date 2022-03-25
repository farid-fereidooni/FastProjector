using System;
using FastProjector.Contracts;

namespace FastProjector.Ioc
{
    public sealed class SingletonServiceFactory : ServiceFactory
    {
        public SingletonServiceFactory(Func<IServiceResolver, object> factoryMethod, Type serviceType) : base(
            factoryMethod, serviceType)
        {
        }

        public override object CreateInstance(IServiceRepository singletonRepository,
            IServiceRepository scopedRepository, IServiceResolver serviceResolver)
        {
            var service = singletonRepository.Get(ServiceType);
            if (service is not null)
            {
                return service;
            }

            var newInstance = FactoryMethod(serviceResolver);
            singletonRepository.Add(ServiceType, newInstance);
            return newInstance;
        }
    }
}