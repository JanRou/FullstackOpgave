using fullstackbe.Core.Domain;
using fullstackbe.Gateways.Cvrapi;
using fullstackbe.Gateways.Dal;
using fullstackbe.Gateways.Repository;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Runtime.ConstrainedExecution;

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
        Task<Virksomhed?> Create(int cvr, CancellationToken token);

        /// <summary>
        /// Opdaterer eksisterende virksomhed, hvis den eksisterer
        /// </summary>
        /// <param name="virksomhed"></param>
        /// <returns></returns>
        Task<Virksomhed?> Update(IVirksomhed virksomhed);

        /// <summary>
        /// Sletter en eksisterende virksomhed, hvis den eksisterer
        /// </summary>
        /// <param name="cvr">Cvrnr på virksomheden der slettes</param>
        /// <returns>Sand når virksomheden er slettet, ellers falsk</returns>
        Task<bool> Delete(int cvr);
    }

    public class VirksomhedCrud( IVirksomhedRepository repository, ICvrapi cvrapi) : IVirksomhedCrud
    {
        public async Task<IEnumerable<Virksomhed>> GetAll()
        {            
            // Hent alle virksomheder fra databasen
            var daos = await repository.GetAll();
            
            // map fra dao til dto
            return daos.Select(d => new Virksomhed(d.Cvr, d.Navn, d.Adresse, d.Postnummer, d.By));
        }

        public async Task<Virksomhed?> Create(int cvr, CancellationToken token)
        {
            //1. Slå virksomheden op, hvis den ikke er der trin 2, ellers ud med fejl
            Virksomhed? result = null;
            bool notExists = await repository.Get(cvr) == null ;
            if (notExists)
            {
                //2. Hent virksomhedsdata fra cvrapi                
                result = await cvrapi.Get(cvr, token);
            }
            //3. Gem i database
            if (result != null)
            {
                var dao = new VirksomhedDao(result.Cvr, result.Navn, result.Adresse, result.Postnummer, result.By);
                await repository.Create(dao);
            }
            //4. Returner ny virksomhed, null hvis den var der i forvejen
            return result;
        }

        public async Task<Virksomhed?> Update(IVirksomhed ny)
        {
            Virksomhed? result = null;
            //1. Slå virksomheden op, hvis den er der trin 2, ellers ud med fejl
            var gammel = repository.Get(ny.Cvr);
            if (gammel != null)
            {
                //2. Opdater virksomheden med cvrnr i databasen
                var nyDao = new VirksomhedDao(ny.Cvr, ny.Navn, ny.Adresse, ny.Postnummer, ny.By);
                nyDao = await repository.Update(nyDao);
                if (nyDao != null)
                {
                    result = new Virksomhed(nyDao.Cvr, nyDao.Navn, nyDao.Adresse, nyDao.Postnummer, nyDao.By);
                }
            }
            return result;
        }

        public async Task<bool> Delete(int cvr)
        {
            //1. Slå virksomheden op, hvis den er der trin 2, ellers ud med fejl
            bool result = repository.Get(cvr) != null;            
            if (result)
            {
                //2. Slet virksomheden fra databasen 
                result = await repository.Delete(cvr);
            }
            return result;
        }

    }
}
