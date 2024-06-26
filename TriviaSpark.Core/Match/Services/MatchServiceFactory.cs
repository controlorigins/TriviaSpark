using TriviaSpark.Core.Match.Models;

namespace TriviaSpark.Core.Match.Services
{
    public class MatchServiceFactory : IMatchServiceFactory
    {
        private readonly Dictionary<MatchMode, Func<Services.IMatchService>> _serviceFactories;

        public MatchServiceFactory(Dictionary<MatchMode, Func<Services.IMatchService>> serviceFactories)
        {
            _serviceFactories = serviceFactories;
        }

        public Services.IMatchService CreateMatchService(MatchMode mode)
        {
            if (_serviceFactories.TryGetValue(mode, out Func<Services.IMatchService> serviceFactory))
            {
                return serviceFactory();
            }

            throw new ArgumentException($"Invalid match mode: {mode}", nameof(mode));
        }
    }
}
