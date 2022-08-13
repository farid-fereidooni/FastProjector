using System.Collections.Generic;
using System.Linq;
using FastProjector.Contracts;
using FastProjector.Helpers;
using FastProjector.Models;
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

            namespaceSource.AddClass(CreateQueryableProjectionMetadataClass());
            namespaceSource.AddClass(CreateProjectionMetadataClass());

            var classSource = CreateInitializerClass();

            var allModelMaps = _mapRepository.GetAll();


            var projectionMethods = allModelMaps.Select(from =>
                {
                    var source = CreateProjectionCreationMethod(from.Key, from.Value);
                    classSource.AddMethod(source);
                    return source;
                }
            );

            classSource.AddMethod(CreateInitializerMethod(projectionMethods));

            return namespaceSource;
        }

        private static IClassSourceText CreateInitializerClass()
        {
            return SourceCreator
                .CreateClass(ProjectionInitializerClassName, AccessModifier.@public)
                .AddField(
                    SourceCreator
                        .CreateField(AccessModifier.@private, $"IEnumerable<{ProjectionMetadataClassName}>",
                            "_allProjections")
                        .AddInitializer(SourceCreator.CreateSource($"new List<{ProjectionMetadataClassName}>"))
                );
        }

        private static IMethodSourceText CreateInitializerMethod(IEnumerable<IMethodSourceText> projectionMethods)
        {
            var methodSource = SourceCreator.CreateMethod(AccessModifier.@public,
                $"IEnumerable<{ProjectionMetadataClassName}>",
                ProjectionInitializerMethodName);

            foreach (var projectionMethod in projectionMethods)
            {
                methodSource.AddSource($"_allProjections.Add({projectionMethod.Name}());");
            }

            return methodSource;
        }

        private IMethodSourceText CreateProjectionCreationMethod(string sourceFullName,
            IEnumerable<ModelMap> modelMaps)
        {
            var modelMapMethodSource = SourceCreator.CreateMethod(AccessModifier.@private,
                $"IEnumerable<{ProjectionMetadataClassName}>", CreateMethodNameFromFullname(sourceFullName));

            foreach (var modelMap in modelMaps)
            {

                if (!TryCreateMappingSource(modelMap, out var mappingSource))
                    continue;
                
                var sourceFullname = modelMap.SourceType.FullName;
                var destinationFullname = modelMap.DestinationType.FullName;
                var destinationNormalizedForVariable = CreateMethodNameFromFullname(destinationFullname);

                modelMapMethodSource.AddSource(SourceCreator.CreateSource($@"
                    Expression<Func<{sourceFullname}, {destinationFullname}>> To_{destinationNormalizedForVariable} =
                        source => {mappingSource};

                    yield return new {ProjectionMetadataClassName}(
                         {QueryableProjectionMetadataClassName}(To_{destinationNormalizedForVariable})
                    );                         
                "));
            }

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

        private static IClassSourceText CreateQueryableProjectionMetadataClass()
        {
            return SourceCreator.CreateClass(QueryableProjectionMetadataClassName, AccessModifier.@public)
                .AddProperty(
                    SourceCreator.CreateProperty(AccessModifier.@public, "Expression", "QueryableExpression", true)
                )
                .AddConstructor(
                    SourceCreator.CreateConstructor(AccessModifier.@public, QueryableProjectionMetadataClassName)
                        .AddParameter("Expression", "queryableExpression")
                        .AddSource(SourceCreator.CreateSource(@"
                                QueryableExpression = queryableExpression;
                            "))
                );
        }

        private static IClassSourceText CreateProjectionMetadataClass()
        {
            return SourceCreator.CreateClass(ProjectionMetadataClassName, AccessModifier.@public)
                .AddProperty(
                    SourceCreator.CreateProperty(AccessModifier.@public, QueryableProjectionMetadataClassName,
                        QueryableProjectionMetadataClassName, true)
                )
                .AddConstructor(
                    SourceCreator.CreateConstructor(AccessModifier.@public, ProjectionMetadataClassName)
                        .AddParameter(QueryableProjectionMetadataClassName,
                            QueryableProjectionMetadataClassName.ToLowerFirstChar())
                        .AddSource(SourceCreator.CreateSource(@$"
                                {QueryableProjectionMetadataClassName} = {QueryableProjectionMetadataClassName.ToLowerFirstChar()};
                            "))
                );
        }

        private static string CreateMethodNameFromFullname(string objectFullname)
        {
            return objectFullname.Replace(".", "_");
        }
    }
}