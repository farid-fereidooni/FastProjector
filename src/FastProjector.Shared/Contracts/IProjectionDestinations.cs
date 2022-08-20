namespace FastProjector.Shared.Contracts
{
    public interface IProjectionDestinations
    {
        IProjectionMetadata To<TDestination>();
    }
}