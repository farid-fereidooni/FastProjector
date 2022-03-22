// using FastProjector.MapGenerator.Proccessing;
// using FastProjector.MapGenerator.Proccessing.Contracts;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace FastProjector.MapGenerator.Ioc
// {
//     public static class IocExtension
//     {
//         public static void AddServices(this IServiceCollection services)
//         {
//             services.AddSingleton<IMapCache, MapCache>();
//             services.AddScoped<ICastingService, CastingService>();
//             services.AddScoped<IProjectionRequestProcessor, ProjectionRequestProcessor>();
//         }
//         
//     }
// }