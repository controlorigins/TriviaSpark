using TriviaSpark.Core.Match;
using TriviaSpark.Web.Areas.Identity.Services;

namespace TriviaSpark.Core.Interfaces
{
    public class MatchServiceFactory : IMatchServiceFactory
    {
        private readonly Dictionary<MatchMode, Func<IMatchService>> _serviceFactories;

        public MatchServiceFactory(Dictionary<MatchMode, Func<IMatchService>> serviceFactories)
        {
            _serviceFactories = serviceFactories;
        }

        public IMatchService CreateMatchService(MatchMode mode)
        {
            if (_serviceFactories.TryGetValue(mode, out Func<IMatchService> serviceFactory))
            {
                return serviceFactory();
            }

            throw new ArgumentException($"Invalid match mode: {mode}", nameof(mode));
        }
    }
}
