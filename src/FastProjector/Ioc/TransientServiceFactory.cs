using System;
using FastProjector.Contracts;

namespace FastProjector.Ioc
{
    public sealed class TransientServiceFactory : ServiceFactory
    {
        public TransientServiceFactory(Func<IServiceResolver, object> factoryMethod, Type serviceType) : base(
            factoryMethod, serviceType)
        {
        }

        public override object CreateInstance(IServiceRepository singletonRepository,
            IServiceRepository scopedRepository, IServiceResolver serviceResolver)
        {
            return FactoryMethod(serviceResolver);
        }
    }
}