using SourceCreationHelper.Interfaces;

namespace FastProjector.Contracts
{
    internal interface IProjectionInitializerGenerator
    {
        INamespaceSourceText Generate();
    }
}