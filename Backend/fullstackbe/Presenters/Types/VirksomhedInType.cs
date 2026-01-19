using fullstackbe.Core.Domain;

namespace fullstackbe.Presenters.Types
{    
    public record VirksomhedInType(int Cvr);

    public class VirksomhedPayload(Virksomhed virksomhed)
    {
        public Virksomhed Virksomhed { get; } = virksomhed;
    }

}
