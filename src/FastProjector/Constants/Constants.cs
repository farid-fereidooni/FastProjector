using FastProjector.Shared;
using FastProjector.Shared.Contracts;

namespace FastProjector.Constants
{
    internal static class Constants
    {
        public const string BaseGenerationNamespace = "FastProjector";
        public static readonly string ProjectionMetadataClassName = nameof(IProjectionMetadata).TrimStart('I');
        public const string ProjectionInitializerClassName = "ProjectionInitializer";
        public const string ProjectionInitializerMethodName = "CreateProjections";
    }
}