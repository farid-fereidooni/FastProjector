namespace FastProjector.Contracts
{
    public interface IServiceResolver
    {
        TService GetService<TService>()
            where TService : class;

        TService GetRequiredService<TService>()
            where TService : class;
    }
}