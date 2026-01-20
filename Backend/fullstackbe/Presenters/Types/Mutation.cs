using fullstackbe.Core.Application;
using fullstackbe.Core.Domain;

namespace fullstackbe.Presenters.Types
{
    [MutationType]
    public static class Mutations
    {
        [Mutation]
        public static async Task<VirksomhedPayload> OpretVirksomhed(
              int cvr
            , IVirksomhedCrud virksomhedCrud
            , CancellationToken cancellationToken)
        {
            var virksomhedOut = await virksomhedCrud.Create(cvr);
            // TODO Fejl håndtering
            return new VirksomhedPayload(virksomhedOut);
        }

        [Mutation]
        public static async Task<VirksomhedPayload> OpdaterVirksomhed(
            VirksomhedInType input
            , IVirksomhedCrud virksomhedCrud
            , CancellationToken cancellationToken)
        {
            var virksomhedIn = new Virksomhed(input.Cvr, input.Navn, input.Adresse, input.Postnummer, input.By);
            var virksomhedOut = await virksomhedCrud.Update(virksomhedIn);
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
