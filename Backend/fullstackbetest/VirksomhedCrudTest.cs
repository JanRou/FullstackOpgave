using fullstackbe.Core.Application;
using fullstackbe.Core.Domain;
using fullstackbe.Gateways.Cvrapi;
using fullstackbe.Gateways.Dal;
using fullstackbe.Gateways.Repository;
using Microsoft.Extensions.DependencyModel.Resolution;
using Moq;

namespace fullstackbetest
{
    public class VirksomhedCrudTest 
    {
        [Fact]
        public async Task GetAllTest()
        {
            // Arrange
            var daos = new List<VirksomhedDao> {
                new VirksomhedDao(28106661, "Skødstrup Tandklinik ApS.", "Grenåvej 728", 8541, "Skødstrup"),
                new VirksomhedDao(38276255, "TeamKey ApS", adresse: "Søndersøparken 19F, 1. 3.", 8800, "Viborg"),
                new VirksomhedDao(42954616, "STILLING KØL & EL ApS", "Niels Bohrs Vej 15A", 8660, "Skanderborg"),
                new VirksomhedDao(12345678, "Elis ApS", adresse: "Elishøj 14", 8541, "Skødstrup"),
                new VirksomhedDao(17477994, "Risskov El & VVS & Ventilation A/S", "Ved Skoven 45, 1", 8541, "Skødstrup")
            };
            
            var cvrapiMock = new Mock<ICvrapi>();
            var repoMock = new Mock<IVirksomhedRepository>();
            repoMock.Setup(r => r.GetAll()).ReturnsAsync(daos);

            var dut = new VirksomhedCrud(repoMock.Object, cvrapiMock.Object);

            // Act
            var result = await dut.GetAll();

            // Assert
            repoMock.Verify();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            var resultList = result.ToList();
            Assert.Equal(daos[0].Cvr, resultList[0].Cvr);
            Assert.Equal(daos[daos.Count-1].By, resultList[resultList.Count-1].By);
        }

        [Fact]
        public async Task CreateOkTest()
        {
            // Arrange
            int cvr = 28106661;
            var dao = new VirksomhedDao(28106661, "Skødstrup Tandklinik ApS.", "Grenåvej 728", 8541, "Skødstrup");
            
            var repoMock = new Mock<IVirksomhedRepository>();
            repoMock.Setup(r => r.Get(cvr)).ReturnsAsync((VirksomhedDao)null);
            repoMock.Setup(r => r.Create(It.IsAny<VirksomhedDao>())).ReturnsAsync(dao);

            var virksomhed = new Virksomhed(28106661, "Skødstrup Tandklinik ApS.", "Grenåvej 728", 8541, "Skødstrup");
            var cvrapiMock = new Mock<ICvrapi>();
            cvrapiMock.Setup(c => c.Get(cvr, CancellationToken.None)).ReturnsAsync(virksomhed);

            var dut = new VirksomhedCrud(repoMock.Object, cvrapiMock.Object);

            // Act
            var result = await dut.Create(cvr, CancellationToken.None);

            // Assert
            repoMock.Verify();
            cvrapiMock.Verify();
            Assert.NotNull(result);
            Assert.Equal(dao.Cvr, result.Cvr);
            Assert.Equal(dao.By, result.By);
        }

        [Fact]
        public async Task CreateFailCvrAlradyExistsTest()
        {
            // Arrange
            int cvr = 28106661;
            var dao = new VirksomhedDao(28106661, "Skødstrup Tandklinik ApS.", "Grenåvej 728", 8541, "Skødstrup");

            var repoMock = new Mock<IVirksomhedRepository>();
            repoMock.Setup(r => r.Get(cvr)).ReturnsAsync(dao);

            var cvrapiMock = new Mock<ICvrapi>();

            var dut = new VirksomhedCrud(repoMock.Object, cvrapiMock.Object);

            // Act
            var result = await dut.Create(cvr, CancellationToken.None);

            // Assert
            repoMock.Verify();
            cvrapiMock.Verify();
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateFailCvrapiReturnsNullTest()
        {
            // Arrange
            int cvr = 28106661;
            var dao = new VirksomhedDao(28106661, "Skødstrup Tandklinik ApS.", "Grenåvej 728", 8541, "Skødstrup");

            var repoMock = new Mock<IVirksomhedRepository>();
            repoMock.Setup(r => r.Get(cvr)).ReturnsAsync((VirksomhedDao)null);

            var cvrapiMock = new Mock<ICvrapi>();
            cvrapiMock.Setup(c => c.Get(cvr, CancellationToken.None)).ReturnsAsync((Virksomhed)null);

            var dut = new VirksomhedCrud(repoMock.Object, cvrapiMock.Object);

            // Act
            var result = await dut.Create(cvr, CancellationToken.None);

            // Assert
            repoMock.Verify();
            cvrapiMock.Verify();
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateOkTest()
        {
            // Arrange
            var dao = new VirksomhedDao(28106661, "Skødstrup Tandklinik ApS.", "Grenåvej 728", 8541, "Skødstrup");
            var nyDao = new VirksomhedDao(28106661, "Skødstrup Tandklinik ApS.", "Grenåvej 827", 8541, "Skødstrup");
            var nyVirksomhed = new Virksomhed(28106661, "Skødstrup Tandklinik ApS.", "Grenåvej 827", 8541, "Skødstrup");
            var repoMock = new Mock<IVirksomhedRepository>();
            repoMock.Setup(r => r.Get(nyVirksomhed.Cvr)).ReturnsAsync(dao);
            repoMock.Setup(r => r.Update(It.IsAny<VirksomhedDao>())).ReturnsAsync(nyDao);

            var cvrapiMock = new Mock<ICvrapi>();

            var dut = new VirksomhedCrud(repoMock.Object, cvrapiMock.Object);

            // Act
            var result = await dut.Update( nyVirksomhed);

            // Assert
            repoMock.Verify();
            cvrapiMock.Verify();
            Assert.NotNull(result);
            Assert.Equal(nyVirksomhed.Cvr, result.Cvr);
            Assert.Equal(nyVirksomhed.Adresse, result.Adresse);
        }

        [Fact]
        public async Task UpdateFailsCvrDontExistsTest()
        {
            // Arrange
            var nyVirksomhed = new Virksomhed(28106661, "Skødstrup Tandklinik ApS.", "Grenåvej 827", 8541, "Skødstrup");
            var repoMock = new Mock<IVirksomhedRepository>();
            repoMock.Setup(r => r.Get(nyVirksomhed.Cvr)).ReturnsAsync((VirksomhedDao)null);

            var cvrapiMock = new Mock<ICvrapi>();

            var dut = new VirksomhedCrud(repoMock.Object, cvrapiMock.Object);

            // Act
            var result = await dut.Update(nyVirksomhed);

            // Assert
            repoMock.Verify();
            cvrapiMock.Verify();
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteOkTest()
        {
            // Arrange
            int cvr = 28106661;
            var dao = new VirksomhedDao(28106661, "Skødstrup Tandklinik ApS.", "Grenåvej 728", 8541, "Skødstrup");
            var repoMock = new Mock<IVirksomhedRepository>();
            repoMock.Setup(r => r.Get(cvr)).ReturnsAsync(dao);
            repoMock.Setup(r => r.Delete(cvr)).ReturnsAsync(true);

            var cvrapiMock = new Mock<ICvrapi>();

            var dut = new VirksomhedCrud(repoMock.Object, cvrapiMock.Object);

            // Act
            var result = await dut.Delete(cvr);

            // Assert
            repoMock.Verify();
            cvrapiMock.Verify();
            Assert.True(result);
        }


    }
}
