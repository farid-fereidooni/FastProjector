using FastProjector.Contracts;
using FastProjector.Models.Casting;
using FastProjector.Repositories;
using FastProjector.Services;
using FastProjector.Services.Processing;

namespace FastProjector.Ioc
{
    public static class IocExtension
    {
        public static IContainer AddServices(this IContainer container)
        {
            container.AddSingleton<IMapRepository>(c => new MapRepository());
            container.AddSingleton<ICastingService>(c =>
                new CastingService(DefaultCastingConfigurations.GetConfigurations()));
            container.AddSingleton<IVariableNameGenerator>(c => new VariableNameGenerator());

            container.AddTransient<IModelMapService>(c =>
                new ModelMapService(c.GetService<IMapRepository>(), c.GetService<ICastingService>(),
                    c.GetService<IVariableNameGenerator>()));

            container.AddScoped<IMapResolverService>(c => new MapResolverService(c.GetService<IMapRepository>()));

            container.AddScoped<IProjectionRequestProcessor>(c =>
                new ProjectionRequestProcessor(c.GetService<IModelMapService>(), c.GetService<IMapRepository>(),
                    c.GetService<IMapResolverService>()));

            //container.AddScoped<IProjectorClassGenerator>(c => new ProjectorClassGenerator());

            container.AddScoped<ProjectionInitializerGenerator>(c =>
                new ProjectionInitializerGenerator(c.GetRequiredService<IMapRepository>(), c.GetRequiredService<IModelMapService>()));

            return container;
        }
    }
}