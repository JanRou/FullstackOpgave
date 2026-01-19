using fullstackbe.Core.Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace fullstackbe.Core.Application
{
    public interface IVirksomhedCrud
    {
        /// <summary>
        /// Henter alle virksomheder i databasen
        /// </summary>
        /// <returns>Liste med virksomheder</returns>
        Task<IEnumerable<Virksomhed>> GetAll();

        /// <summary>
        /// Opretter en ny virksomhed, hvis den ikke er i databasen.
        /// </summary>
        /// <param name="virksomhed"></param>
        /// <returns>Ny virksomhed</returns>
        Task<Virksomhed?> Create(int cvr);

        /// <summary>
        /// Opdaterer eksisterende virksomhed, hvis den eksisterer
        /// </summary>
        /// <param name="virksomhed"></param>
        /// <returns></returns>
        Task<Virksomhed> Update(IVirksomhed virksomhed);

        /// <summary>
        /// Sletter en eksisterende virksomhed, hvis den eksisterer
        /// </summary>
        /// <param name="cvr">Cvrnr på virksomheden der slettes</param>
        /// <returns>Sand når virksomheden er slettet, ellers falsk</returns>
        Task<bool> Delete(int cvr);
    }

    public class VirksomhedCrud : IVirksomhedCrud
    {
        public static List<Virksomhed> virksomheder = new List<Virksomhed>();

        // Til at teste med for sjov
        public VirksomhedCrud()
        {
            if (virksomheder.Count == 0)
            {
                virksomheder.Add(new Virksomhed(42954616, "STILLING KØL & EL ApS", "Niels Bohrs Vej 15A", 8660, "Skanderborg"));
                virksomheder.Add(new Virksomhed(17477994, "Risskov El & VVS & Ventilation A/S", "Ved Skoven 45, 1", 8541, "Skødstrup"));
                virksomheder.Add(new Virksomhed(28106661, "Skødstrup Tandklinik ApS.", "Grenåvej 728", 8541, "Skødstrup"));
                virksomheder.Add(new Virksomhed(51261040, "TeamKey ApS", adresse: "Søndersøparken 19F, 1. 3.", 8800, "Viborg"));
            }
        }

        public async Task<IEnumerable<Virksomhed>> GetAll()
        {
            // Hent alle virksomheder fra databasen
            // map fra dao til dto med db
        
            return virksomheder;
        }

        public async Task<Virksomhed?> Create(int cvr)
        {
            //1. Slå virksomheden op, hvis den ikke er der trin 2, ellers ud med fejl
            Virksomhed? result = null;
            bool notExists = ! virksomheder.Any( v => v.Cvr == cvr);
            if (notExists)
            {
                //2. Hent virksomhedsdata fra cvrapi
                // TODO kod rigtigt med et cvrapi kald
                result = new Virksomhed(cvr, "navn", "adresse", 1000, "by");
                //3. Gem i database
                virksomheder.Add(result);
                // TODO map fra dao til dto med db
            }

            //(4. Returner ny virksomhed), hvis den ikke var der i forvejen
            return result;
        }

        public async Task<Virksomhed> Update(IVirksomhed virksomhed)
        {
            //1. Slå virksomheden op, hvis den er der trin 2, ellers ud med fejl
            //2. Opdater virksomheden med cvrnr i databasen
            // map fra dao til dto med db
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(int cvr)
        {
            //1. Slå virksomheden op, hvis den er der trin 2, ellers ud med fejl
            bool result = virksomheder.Any(v => v.Cvr == cvr);
            //2. Slet virksomheden fra databasen 
            if (result)
            {
                virksomheder.RemoveAll(v => v.Cvr == cvr);
            }
            // map fra dao til dto med db
            return result;
        }

    }
}
