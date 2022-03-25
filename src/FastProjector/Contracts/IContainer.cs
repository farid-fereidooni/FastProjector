using System;
using FastProjector.Contracts;

namespace FastProjector.Ioc
{
    public interface IContainer
    {
        IContainer AddTransient<TService>(Func<IServiceResolver, TService> factory) where TService : class;
        IContainer AddScoped<TService>(Func<IServiceResolver, TService> factory) where TService : class;
        IContainer AddSingleton<TService>(Func<IServiceResolver, TService> factory) where TService : class;
        IServiceResolver CreateScope();
    }
}