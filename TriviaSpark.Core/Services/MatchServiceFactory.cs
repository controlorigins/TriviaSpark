namespace TriviaSpark.Core.Services;

public class MatchServiceFactory(
    Dictionary<Models.MatchMode,
        Func<Services.IMatchService>> serviceFactories) : Services.IMatchServiceFactory
{
    public Services.IMatchService CreateMatchService(Models.MatchMode mode)
    {
        if (serviceFactories.TryGetValue(mode, out Func<Services.IMatchService> serviceFactory))
        {
            return serviceFactory();
        }
        throw new ArgumentException($"Invalid match mode: {mode}", nameof(mode));
    }
}
