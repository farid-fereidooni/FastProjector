using SourceCreationHelper.Interfaces;

namespace FastProjector.Contracts
{
    internal interface IProjectorClassGenerator
    {
        INamespaceSourceText Generate();
    }
}