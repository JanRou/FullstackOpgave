using System.Runtime.ConstrainedExecution;

namespace fullstackbe.Gateways.Dal
{
    public class VirksomhedDao
    {
        public VirksomhedDao() {}
        public VirksomhedDao(int cvr, string navn, string adresse, int postnummer, string by ) 
        {
            Cvr= cvr;
            Navn= navn;
            Adresse= adresse;
            Postnummer= postnummer;
            By= by;
        }

        public int Cvr { get; set; }
        public string Navn { get; set; }
        public string Adresse { get; set; }
        public int Postnummer { get; set; }
        public string By { get; set; }
    }
}
