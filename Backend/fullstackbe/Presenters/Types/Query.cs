using fullstackbe.Core.Application;
using fullstackbe.Core.Domain;

namespace fullstackbe.Presenters.Types
{
    [QueryType]
    public static class Query
    {
        public static Book GetBook()
            => new Book("C# in depth.", new Author("Jon Skeet"));

        [Query]
        public static async Task<IEnumerable<Virksomhed>> HentAlleVirksomheder(
              IVirksomhedCrud virksomhedCrud
            , CancellationToken cancellationToken)
        {
            return await virksomhedCrud.GetAll(); // TODO (cancellationToken);
        }
    }
}
