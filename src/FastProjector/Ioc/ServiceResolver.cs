using System;
using System.Collections.Generic;
using FastProjector.Contracts;

namespace FastProjector.Ioc
{
    public class ServiceResolver : IServiceResolver
    {
        private readonly IServiceRepository _singletonRepository;
        private readonly Dictionary<Type, ServiceFactory> _serviceFactories;
        private readonly IServiceRepository _scopedRepository;

        public ServiceResolver(IServiceRepository singletonRepository,
            Dictionary<Type, ServiceFactory> serviceFactories)
        {
            _singletonRepository = singletonRepository;
            _serviceFactories = serviceFactories;
            _scopedRepository = new ServiceRepository();
        }

        public TService GetService<TService>()
            where TService : class
        {
            if (_serviceFactories.TryGetValue(typeof(TService), out var service))
            {
                return service.CreateInstance(_singletonRepository, _scopedRepository, this) as TService;
            }

            return null;
        }

        public TService GetRequiredService<TService>()
            where TService : class
        {
            var service = GetService<TService>();
            if (service == null)
                throw new InvalidOperationException("The required service does not exist in container");
            return service;
        }
    }
}