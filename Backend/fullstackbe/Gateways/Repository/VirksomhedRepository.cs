using fullstackbe.Core.Domain;
using fullstackbe.Gateways.Dal;
using fullstackbe.Gateways.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
        Task<VirksomhedDao> Get(int cvr);

        /// <summary>
        /// Opretter en ny virksomhed, hvis den ikke er i databasen.
        /// </summary>
        /// <param name="virksomhed"></param>
        /// <returns>Ny virksomhed</returns>
        Task<VirksomhedDao> Create(VirksomhedDao virksomhed);

        /// <summary>
        /// Opdaterer eksisterende virksomhed, hvis den eksisterer
        /// </summary>
        /// <param name="virksomhed"></param>
        /// <returns></returns>
        Task<VirksomhedDao> Update(VirksomhedDao virksomhed);

        /// <summary>
        /// Sletter en eksisterende virksomhed, hvis den eksisterer
        /// </summary>
        /// <param name="cvr">Cvrnr på virksomheden der slettes</param>
        /// <returns>Sand når virksomheden er slettet, ellers falsk</returns>
        Task<bool> Delete(int cvr);

    }

    public class VirksomhedRepository : IVirksomhedRepository
    {
        public VirksomhedRepository(AppDbContext db)
        {
            db.Database.EnsureCreated();
        }

        public async Task<IEnumerable<VirksomhedDao>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<VirksomhedDao> Get(int cvr)
        {
            throw new NotImplementedException();
        }

        public async Task<VirksomhedDao> Create(VirksomhedDao virksomhed)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(int cvr)
        {
            throw new NotImplementedException();
        }

        public async Task<VirksomhedDao> Update(VirksomhedDao virksomhed)
        {
            throw new NotImplementedException();
        }
    }
}
