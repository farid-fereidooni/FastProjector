using System;
using System.Collections.Generic;
using FastProjector.Contracts;

namespace FastProjector.Ioc
{
    public class ServiceRepository : IServiceRepository
    {
        public ServiceRepository()
        {
            ServiceInstances = new Dictionary<Type, object>();
        }

        private Dictionary<Type, object> ServiceInstances { get; }

        public TService Get<TService>() where TService : class
        {
            return Get(typeof(TService)) as TService;
        }

        public void Add<TService>(object service) where TService : class
        {
            Add(typeof(TService), service);
        }


        public object Get(Type serviceType)
        {
            return ServiceInstances.TryGetValue(serviceType, out var service) ? service : null;
        }

        public void Add(Type serviceType, object service)
        {
            if (!ServiceInstances.ContainsKey(serviceType))
            {
                ServiceInstances.Add(serviceType, service);
            }
        }
    }
}