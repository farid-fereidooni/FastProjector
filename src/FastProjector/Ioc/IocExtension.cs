using FastProjector.Contracts;
using FastProjector.Models.Casting;
using FastProjector.Processing;
using FastProjector.Repositories;
using FastProjector.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FastProjector.Ioc
{
    public static class IocExtension
    {
        public static IContainer AddServices(this IContainer container)
        {
            container.AddSingleton<IMapRepository>(c => new MapRepository());
            container.AddSingleton<ICastingService>(c => new CastingService(DefaultCastingConfigurations.GetConfigurations()));
            container.AddSingleton<IVariableNameGenerator>(c => new VariableNameGenerator());
            
            container.AddTransient<IModelMapService>(c =>
                new ModelMapService(c.GetService<IMapRepository>(), c.GetService<ICastingService>(),
                    c.GetService<IVariableNameGenerator>()));
            
            container.AddScoped<IProjectionRequestProcessor>(c =>
                new ProjectionRequestProcessor(c.GetService<IModelMapService>()));

            return container;

        }
    }
}