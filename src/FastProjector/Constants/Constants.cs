using FastProjector.Shared;
using FastProjector.Shared.Contracts;

namespace FastProjector.Constants
{
    internal static class Constants
    {
        public const string BaseGenerationNamespace = "FastProjector";
        public static readonly string PublicApiClassName = nameof(IProjector).TrimStart('I');
        public const string PublicApiInterfaceName = nameof(IProjector);
        public const string ProjectorProjectMethodName = nameof(IProjector.Project);
        public const string QueryableProjectionMetadataClassName = "QueryableProjectionMetadata";
        public static readonly string ProjectionMetadataClassName = nameof(IProjectionMetadata).TrimStart('I');
        public const string ProjectionInitializerClassName = "ProjectionInitializer";
        public const string ProjectionInitializerMethodName = "CreateAllProjections";
    }
}