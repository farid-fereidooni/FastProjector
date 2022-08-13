using FastProjector.Contracts;
using FastProjector.Helpers;
using SourceCreationHelper;
using SourceCreationHelper.Core;
using SourceCreationHelper.Interfaces;
using static FastProjector.Constants.Constants;

namespace FastProjector.Services.Processing
{
    internal class ProjectorClassGenerator : IProjectorClassGenerator
    {
        private const string ProjectionEntriesName = "_projections";
        
        public INamespaceSourceText Generate(ISourceText initializationSource)
        {
            var namespaceSource = SourceCreator.CreateNamespace(BaseGenerationNamespace);

            var projectorConstructor = SourceCreator.CreateConstructor(AccessModifier.@public, PublicApiClassName)
                .SetAsStatic()
                .AddSource(initializationSource);

            var projectorClassSource = CreateProjectorClass(projectorConstructor);
            namespaceSource.AddClass(projectorClassSource);

            projectorClassSource.AddProperty(CreateProjectionDictionary());

            projectorClassSource.AddMethod(CreateQueryableFetchFunction());

            return null;
        }

        private IMethodSourceText CreateQueryableFetchFunction()
        {
           return SourceCreator.CreateMethod(AccessModifier.@public, "Expression<Func<TSource, TDestination>>",
                    "GetQueryableExpression<TSource, TDestination>")
                .AddSource(@$"

                        var sourceFullName = typeof(TSource).FullName;
                        var destinationFullName = typeof(TDestination).FullName;

                        if (!{ProjectionEntriesName}.TryGetValue(sourceFullName, out var destinationProjections)) return null;
            
                        return destinationProjections.TryGetValue(destinationFullName, out var projectionMetadata) 
                            ? projectionMetadata.QueryableExpression as Expression<Func<TSource, TDestination>> 
                            : null;  
                ");
        }

        private IPropertySourceText CreateProjectionDictionary()
        {
            return SourceCreator.CreateProperty(AccessModifier.@private,
                $"Dictionary<string, Dictionary<string, {QueryableProjectionMetadataClassName}>>", ProjectionEntriesName, true);
        }

        private IClassSourceText CreateProjectorClass(IConstructorSourceText constructorSource)
        {
            return SourceCreator.CreateClass(PublicApiClassName, AccessModifier.@public)
                .SetAsStatic()
                .AddConstructor(constructorSource);
        }
    }
}