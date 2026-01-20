using fullstackbe.Core.Domain;

namespace fullstackbe.Presenters.Types
{    
    public record VirksomhedInType(int Cvr, string Navn, string Adresse, int Postnummer, string By);

    public class VirksomhedPayload(Virksomhed virksomhed)
    {
        public Virksomhed Virksomhed { get; } = virksomhed;
    }

}
