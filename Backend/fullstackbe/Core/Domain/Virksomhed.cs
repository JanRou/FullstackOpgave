namespace fullstackbe.Core.Domain
{
    public interface IVirksomhed
    {
        int Cvr { get; }
        string Navn { get; }
        string Adresse { get; }
        int Postnummer { get; }
        string By { get; }
    }

    public class Virksomhed(int cvr, string navn, string adresse, int postnummer, string by) : IVirksomhed
    {
        public int Cvr { get; private set; } = cvr;
        public string Navn { get; private set; } = navn;
        public string Adresse { get; private set; } = adresse;
        public int Postnummer { get; private set; } = postnummer;
        public string By { get; private set; } = by;
    }
}
