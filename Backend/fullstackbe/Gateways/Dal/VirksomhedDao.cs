using System.Runtime.ConstrainedExecution;

namespace fullstackbe.Gateways.Dal
{
    public class VirksomhedDao
    {
        public int Cvr { get; set; }
        public string Navn { get; set; }
        public string Adresse { get; set; }
        public int Postnummer { get; set; }
        public string By { get; set; }
    }
}
