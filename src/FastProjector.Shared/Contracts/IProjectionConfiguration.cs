namespace FastProjector.Shared.Contracts
{
    public interface IProjectionConfiguration
    {
        IProjectionDestinations From<TSource>();
    }
}