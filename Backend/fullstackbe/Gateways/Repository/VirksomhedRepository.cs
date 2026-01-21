using fullstackbe.Core.Domain;
using fullstackbe.Gateways.Dal;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Runtime.ConstrainedExecution;

namespace fullstackbe.Gateways.Repository
{
    public interface IVirksomhedRepository
    {
        /// <summary>
        /// Henter alle virksomheder i databasen
        /// </summary>
        /// <returns>Liste med virksomheder</returns>
        Task<IEnumerable<VirksomhedDao>> GetAll();

        /// <summary>
        /// Henter en virksomhed i databasen
        /// </summary>
        /// <returns>Virksomheden ellers null</returns>
        Task<VirksomhedDao?> Get(int cvr);

        /// <summary>
        /// Opretter en ny virksomhed, hvis den ikke er i databasen.
        /// </summary>
        /// <param name="virksomhed"></param>
        /// <returns>Ny virksomhed</returns>
        Task<VirksomhedDao?> Create(VirksomhedDao virksomhed);

        /// <summary>
        /// Opdaterer eksisterende virksomhed, hvis den eksisterer
        /// </summary>
        /// <param name="virksomhed"></param>
        /// <returns></returns>
        Task<VirksomhedDao?> Update(VirksomhedDao virksomhed);

        /// <summary>
        /// Sletter en eksisterende virksomhed, hvis den eksisterer
        /// </summary>
        /// <param name="cvr">Cvrnr på virksomheden der slettes</param>
        /// <returns>Sand når virksomheden er slettet, ellers falsk</returns>
        Task<bool> Delete(int cvr);

    }

    public class VirksomhedRepository : IVirksomhedRepository
    {
        private AppDbContext db;

        public VirksomhedRepository(AppDbContext db)
        {
            db.Database.EnsureCreated();
            this.db = db;
        }

        public async Task<IEnumerable<VirksomhedDao>> GetAll()
        {
            return db.Virksomheds;
        }

        public async Task<VirksomhedDao?> Get(int cvr)
        {
            return db.Virksomheds.Where(v => v.Cvr == cvr).FirstOrDefault();
        }

        public async Task<VirksomhedDao> Create(VirksomhedDao virksomhed)
        {
            // Repository har kontrolleret at virksomheden er ny, ellers vil en dublet fejle pga. primary key dublet
            db.Add(virksomhed);  
            await db.SaveChangesAsync();
            return virksomhed;
        }

        public async Task<bool> Delete(int cvr)
        {
            var virksomhed = db.Virksomheds.Where(v => v.Cvr == cvr).FirstOrDefault();
            bool result = virksomhed != null;
            if (result)
            {
                db.Remove(virksomhed);
                await db.SaveChangesAsync();
            }
            return result;
        }

        public async Task<VirksomhedDao?> Update(VirksomhedDao virksomhed)
        {
            var existVirksomhed = db.Virksomheds.Where(v => v.Cvr == virksomhed.Cvr).FirstOrDefault();

            if (existVirksomhed != null)
            {
                existVirksomhed.Navn = virksomhed.Navn;
                existVirksomhed.Adresse = virksomhed.Adresse;
                existVirksomhed.Postnummer = virksomhed.Postnummer;
                existVirksomhed.By = virksomhed.By;
                await db.SaveChangesAsync();
            }
            return existVirksomhed;
        }
    }
}
