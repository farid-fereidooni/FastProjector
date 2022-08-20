namespace FastProjector.Shared.Contracts
{
    public interface IProjectionDictionary
    {
        IProjectionDestinations From<TSource>();
    }
}