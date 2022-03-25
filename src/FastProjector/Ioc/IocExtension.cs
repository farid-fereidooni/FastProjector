using FastProjector.Contracts;
using FastProjector.Processing;
using FastProjector.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FastProjector.Ioc
{
    public static class IocExtension
    {
        public static IContainer AddServices(this IContainer container)
        {
            container.AddSingleton<IMapCache>(c => new MapCache());
            container.AddSingleton<ICastingService>(c => new CastingService());
            container.AddSingleton<IVariableNameGenerator>(c => new VariableNameGenerator());
            
            container.AddTransient<IModelMapService>(c =>
                new ModelMapService(c.GetService<IMapCache>(), c.GetService<ICastingService>(),
                    c.GetService<IVariableNameGenerator>()));
            
            container.AddScoped<IProjectionRequestProcessor>(c =>
                new ProjectionRequestProcessor(c.GetService<IModelMapService>()));

            return container;

        }
    }
}