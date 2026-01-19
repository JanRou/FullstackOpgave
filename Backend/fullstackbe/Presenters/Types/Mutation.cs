using fullstackbe.Core.Application;
using fullstackbe.Core.Domain;

namespace fullstackbe.Presenters.Types
{
    [MutationType]
    public static class Mutations
    {
        [Mutation]
        public static async Task<VirksomhedPayload> OpretVirksomhed(
            VirksomhedInType input
            , IVirksomhedCrud virksomhedCrud
            , CancellationToken cancellationToken)
        {
            var virksomhedOut = await virksomhedCrud.Create(input.Cvr);
            // TODO Fejl håndtering
            return new VirksomhedPayload(virksomhedOut);
        }

        [Mutation]
        public static async Task<bool> SletVirksomhed(
            VirksomhedInType input
            , IVirksomhedCrud virksomhedCrud
            , CancellationToken cancellationToken)
        {
            bool result = await virksomhedCrud.Delete(input.Cvr);
            return result;
        }

    }
}
