using System;
using FastProjector.Contracts;

namespace FastProjector.Ioc
{
    public sealed class ScopedServiceFactory : ServiceFactory
    {
        public ScopedServiceFactory(Func<IServiceResolver, object> factoryMethod, Type serviceType) : base(
            factoryMethod, serviceType)
        {
        }

        public override object CreateInstance(IServiceRepository singletonRepository,
            IServiceRepository scopedRepository, IServiceResolver serviceResolver)
        {
            var service = scopedRepository.Get(ServiceType);
            if (service is not null)
            {
                return service;
            }

            var newInstance = FactoryMethod(serviceResolver);
            scopedRepository.Add(ServiceType, newInstance);
            return newInstance;
        }
    }
}