using fullstackbe.Core.Domain;
using fullstackbe.Gateways.Dal;
using fullstackbe.Gateways.Repository;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fullstackbetest
{
    public class VirksomhedRepositoryTest
    {
        public VirksomhedRepositoryTest() { Setup(); }

        public void Dispose() { Teardown(); }

        public AppDbContext Db { get; set; }


        [Fact]
        public async Task GetAllTest()
        {
            // Arrange
            var dut = new VirksomhedRepository(Db);

            // Act
            var result = await dut.GetAll();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetTest()
        {
            // Arrange
            /// Bør allerede være i databasen
            var virksomhed = new VirksomhedDao() {
                Cvr=42954616, Navn="STILLING KØL & EL ApS",
                Adresse = "Niels Bohrs Vej 15A", Postnummer=8660, By="Skanderborg" };
            var dut = new VirksomhedRepository(Db);

            // Act
            var result = await dut.Get(virksomhed.Cvr);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(virksomhed.Cvr, result.Cvr);
            Assert.Equal(virksomhed.Navn, result.Navn);
            Assert.Equal(virksomhed.Adresse, result.Adresse);
            Assert.Equal(virksomhed.Postnummer, result.Postnummer);
            Assert.Equal(virksomhed.By, result.By);
        }

        [Fact]
        public async Task CreateAndDeleteTest()
        {
            // Arrange
            var dut = new VirksomhedRepository(Db);
            var nyVirksomhed = new VirksomhedDao() { 
                Cvr = 9999999,
                Navn = "Test navn",
                Adresse = "Test adresse",
                Postnummer = 9999,
                By = "Testby" 
            };

            // Act
            var resultCreate = await dut.Create(nyVirksomhed);

            // Assert
            Assert.NotNull(resultCreate);
            Assert.Equal(nyVirksomhed.Cvr, resultCreate.Cvr);

            // Fjern oprettet test entry (docker image ville det ikke være nødvendigt)
            // Act
            bool resultDelete = await dut.Delete(nyVirksomhed.Cvr);

            // Assert
            Assert.True(resultDelete);
        }

        [Fact]
        public async Task UpdateAndDeleteTest()
        {
            // Arrange
            var dut = new VirksomhedRepository(Db);
            var nyVirksomhed = new VirksomhedDao() {
                Cvr = 8888888,
                Navn = "Test navn",
                Adresse = "Test adresse",
                Postnummer = 8888,
                By = "Testby "
            };

            var opdateretVirksomhed = new VirksomhedDao() {
                Cvr = 8888888,
                Navn = "Test navn opdateret",
                Adresse = "Test adresse opdateret",
                Postnummer = 8889,
                By = "Testby opdateret"
            };
            // Seed Opret entry til at teste med (docker image ville have haft det)
            var resultCreate = await dut.Create(nyVirksomhed);

            // Act
            var result = await dut.Update(opdateretVirksomhed);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nyVirksomhed.Cvr, result.Cvr); // skal være ens
            Assert.Equal(opdateretVirksomhed.Navn, result.Navn); // hmm theory??

            // Fjern oprettet test entry
            await dut.Delete(nyVirksomhed.Cvr);

        }

        private static readonly object _lock = new();
        public void Setup()
        {
            // TODO hack for at teste databasen. Ikke super smart i et bygge miljø: Brug docker container
            lock (_lock)
            {
                Db = new AppDbContext(
                        new DbContextOptionsBuilder<AppDbContext>()
                            .UseSqlite("Data Source=..\\..\\..\\..\\fullstackbe\\data\\database.db")
                            .LogTo(Console.WriteLine, LogLevel.Information)
                            .Options
                            );
            }
        }

        public void Teardown()
        {
            // Fjern test virksomheder fra databasen. Med et docker image, så er der ikke nødvendigt
            Db.Dispose();
        }

    }
}
