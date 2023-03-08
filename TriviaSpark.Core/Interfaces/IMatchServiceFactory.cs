using TriviaSpark.Core.Match;
using TriviaSpark.Web.Areas.Identity.Services;

namespace TriviaSpark.Core.Interfaces
{
    public interface IMatchServiceFactory
    {
        IMatchService CreateMatchService(MatchMode mode);
    }
}
