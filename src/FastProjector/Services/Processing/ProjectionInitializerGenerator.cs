using System.Collections.Generic;
using System.Linq;
using FastProjector.Contracts;
using FastProjector.Models;
using FastProjector.Shared.Contracts;
using FastProjector.Shared.Models;
using SourceCreationHelper;
using SourceCreationHelper.Core;
using SourceCreationHelper.Interfaces;
using static FastProjector.Constants.Constants;

namespace FastProjector.Services.Processing
{
    internal class ProjectionInitializerGenerator : IProjectionInitializerGenerator
    {
        private readonly IMapRepository _mapRepository;
        private readonly IModelMapService _modelMapService;

        public ProjectionInitializerGenerator(IMapRepository mapRepository, IModelMapService modelMapService)
        {
            _mapRepository = mapRepository;
            _modelMapService = modelMapService;
        }

        public INamespaceSourceText Generate()
        {
            var namespaceSource = SourceCreator.CreateNamespace(BaseGenerationNamespace);

            namespaceSource.AddUsing("FastProjector.Shared.Models");
            namespaceSource.AddUsing("FastProjector.Shared.Contracts");
            namespaceSource.AddUsing("System.Linq.Expressions");

            var classSource = CreateInitializerClass();

            var allModelMaps = _mapRepository.GetAll();

            var projectionMethods = allModelMaps.Select(from =>
                {
                    var source = CreateProjectionCreationMethod(from.Key, from.Value);
                    classSource.AddMethod(source);
                    return new ConfigurationItem(from.Key, source);
                }
            );

            classSource.AddMethod(CreateInitializerMethod(projectionMethods));

            namespaceSource.AddClass(classSource);
            return namespaceSource;
        }

        private static IClassSourceText CreateInitializerClass()
        {
            return SourceCreator.CreateClass(ProjectionInitializerClassName, AccessModifier.@public)
                .SetAsStatic();
        }

        private static IMethodSourceText CreateInitializerMethod(IEnumerable<ConfigurationItem> configurations)
        {
            var methodSource = SourceCreator.CreateMethod(AccessModifier.@public,
                nameof(IProjectionConfiguration), ProjectionInitializerMethodName)
                .SetAsStatic();

            methodSource.AddSource(SourceCreator.CreateAssignment(
                SourceCreator.CreateSource("var configuration"),
                SourceCreator.CreateSource($"new {nameof(ProjectionConfiguration)}();")
            ));

            foreach (var config in configurations)
                methodSource.AddSource(
                    $"configuration.AddConfig(\"{config.ProjectionSource}\", {config.MethodSource.Name}());");

            methodSource.AddSource("return configuration;");

            return methodSource;
        }

        private IMethodSourceText CreateProjectionCreationMethod(string sourceFullname,
            IEnumerable<ModelMap> modelMaps)
        {
            var modelMapMethodSource = SourceCreator
                .CreateMethod(AccessModifier.@private, nameof(IProjectionDestinations),
                    CreateMethodNameFromFullname(sourceFullname))
                .SetAsStatic();
                

            var destinationVariableDefinitionSource = SourceCreator.CreateAssignment(
                SourceCreator.CreateSource("var destinations"),
                SourceCreator.CreateSource($"new {nameof(ProjectionDestinations)}();")
            );

            modelMapMethodSource.AddSource(destinationVariableDefinitionSource);

            foreach (var modelMap in modelMaps)
            {
                if (!TryCreateMappingSource(modelMap, out var mappingSource))
                    continue;

                var destinationFullname = modelMap.DestinationType.FullName;
                var destinationNormalizedForVariable = CreateMethodNameFromFullname(destinationFullname);

                modelMapMethodSource.AddSource(SourceCreator.CreateSource($@"
                    Expression<Func<{sourceFullname}, {destinationFullname}>> to_{destinationNormalizedForVariable} =
                        source => {mappingSource};

                    destinations.{nameof(ProjectionDestinations.AddDestination)}
                    (
                        ""{destinationFullname}"",
                        new {ProjectionMetadataClassName}(new {nameof(IQueryableProjectionMetadata).TrimStart('I')}(to_{destinationNormalizedForVariable}))
                    );                
                "));
            }

            modelMapMethodSource.AddSource(SourceCreator.CreateSource("return destinations;"));

            return modelMapMethodSource;
        }

        private bool TryCreateMappingSource(ModelMap modelMap, out ISourceText sourceText)
        {
            try
            {
                sourceText = modelMap.CreateMappingSource(_modelMapService, SourceCreator.CreateSource("source"));
                return true;
            }
            catch
            {
                sourceText = default;
                return false;
            }
        }

        private static string CreateMethodNameFromFullname(string objectFullname)
        {
            return objectFullname.Replace(".", "_");
        }

        private struct ConfigurationItem
        {
            public ConfigurationItem(string projectionSource, IMethodSourceText methodSource)
            {
                ProjectionSource = projectionSource;
                MethodSource = methodSource;
            }

            public readonly string ProjectionSource;
            public readonly IMethodSourceText MethodSource;
        }
    }
}