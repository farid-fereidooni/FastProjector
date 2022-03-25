using System;
using System.Collections.Generic;
using FastProjector.Contracts;

namespace FastProjector.Ioc
{

    public class IocContainer : IContainer
    {
        private readonly Dictionary<Type, ServiceFactory> _factories = new();

        private readonly IServiceRepository _singletonRepository = new ServiceRepository();

        public IContainer AddTransient<TService>(Func<IServiceResolver, TService> factory) where TService : class
        {
            _factories.Add(typeof(TService), ServiceFactory.CreateTransient(factory));
            return this;
        }

        public IContainer AddScoped<TService>(Func<IServiceResolver, TService> factory) where TService : class
        {
            _factories.Add(typeof(TService), ServiceFactory.CreateScoped(factory));
            return this;
        }

        public IContainer AddSingleton<TService>(Func<IServiceResolver, TService> factory) where TService : class
        {
            _factories.Add(typeof(TService), ServiceFactory.CreateSingleton(factory));
            return this;
        }

        public IServiceResolver CreateScope()
        {
            return new ServiceResolver(_singletonRepository, _factories);
        }
    }
}