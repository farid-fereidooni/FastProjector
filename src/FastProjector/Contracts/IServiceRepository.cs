using System;

namespace FastProjector.Contracts
{
    public interface IServiceRepository
    {
        TService Get<TService>() where TService : class;
        void Add<TService>(object service) where TService : class;
        public object Get(Type serviceType);

        public void Add(Type serviceType, object service);
    }
}