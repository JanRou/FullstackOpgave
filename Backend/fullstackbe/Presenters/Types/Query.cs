using fullstackbe.Core.Application;
using fullstackbe.Core.Domain;

namespace fullstackbe.Presenters.Types
{
    [QueryType]
    public static class Query
    {
        [Query]
        public static async Task<IEnumerable<Virksomhed>> HentAlleVirksomheder(
              IVirksomhedCrud virksomhedCrud
            , CancellationToken cancellationToken)
        {
            return await virksomhedCrud.GetAll(); // TODO (cancellationToken);
        }
    }
}
