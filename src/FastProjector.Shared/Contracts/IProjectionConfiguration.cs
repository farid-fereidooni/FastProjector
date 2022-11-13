using System;

namespace FastProjector.Shared.Contracts
{
    public interface IProjectionConfiguration
    {
        IProjectionDestinations From<TSource>();
        IProjectionDestinations From(Type type);
    }
}