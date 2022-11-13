using System;

namespace FastProjector.Shared.Contracts
{
    public interface IProjectionDestinations
    {
        IProjectionMetadata To<TDestination>();
        IProjectionMetadata To(Type type);
    }
}