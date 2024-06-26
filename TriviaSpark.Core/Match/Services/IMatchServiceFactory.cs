using TriviaSpark.Core.Match.Models;

namespace TriviaSpark.Core.Match.Services
{
    public interface IMatchServiceFactory
    {
        Services.IMatchService CreateMatchService(MatchMode mode);
    }
}
