using fullstackbe.Gateways.Cvrapi;
using System.Text.Json;

namespace fullstackbetest
{
    public class CvrapiTest
    {
        [Fact]
        public void UrlFormatterTest()
        {
            // Arrange
            string cvrapiUrlFormatter = @"http://cvrapi.dk/api?search={0:D8}&country=dk";
            int cvr = 89563518;
            string expected = @"http://cvrapi.dk/api?search=89563518&country=dk";
            var dut = new Cvrapi(cvrapiUrlFormatter);

            // Act
            string result = dut.FormatUrl(cvr);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        //[Theory]
        public void GetStringFromJsonElementTest()
        {
            // Arrange
            string cvrapiUrlFormatter = @"http://cvrapi.dk/api?search={0:D8}&country=dk";
            var dut = new Cvrapi(cvrapiUrlFormatter);
            JsonDocument document = JsonDocument.Parse(jsonResponse);
            JsonElement root = document.RootElement;

            // Act
            string result = dut.GetStringFromJsonElement(root, "name");

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal("CARLETTI A/S", result);
        }
        [Fact]
        //[Theory]
        public void GetIntFromJsonElementTest()
        {
            // Arrange
            string cvrapiUrlFormatter = @"http://cvrapi.dk/api?search={0:D8}&country=dk";
            var dut = new Cvrapi(cvrapiUrlFormatter);
            JsonDocument document = JsonDocument.Parse(jsonResponse);
            JsonElement root = document.RootElement;

            // Act
            int result = dut.GetIntFromJsonElement(root, "zipcode");

            // Assert
            Assert.NotEqual(0, result);
            Assert.Equal(8541, result);
        }
        [Fact]
        //[Theory]
        public void ParseJsonTest()
        {
            // Arrange
            string cvrapiUrlFormatter = @"http://cvrapi.dk/api?search={0:D8}&country=dk";
            var dut = new Cvrapi(cvrapiUrlFormatter);

            // Act
            var (name, address, zipcode, city) = dut.ParseJson(jsonResponse);

            // Assert
            Assert.NotEmpty(name);
            Assert.NotEmpty(address);
            Assert.NotEqual(0, zipcode);
            Assert.NotEmpty(city);
            Assert.Equal("CARLETTI A/S", name);
        }

        public string jsonResponse = @"{
   ""vat"": 89563518,
   ""name"": ""CARLETTI A/S"",
   ""address"": ""Grenåvej 641"",
   ""zipcode"": 8541,
   ""city"": ""Skødstrup"",
   ""cityname"": null,
   ""protected"": false,
   ""phone"": 87490200,
   ""email"": null,
   ""fax"": null,
   ""startdate"": ""23/04 - 1980"",
   ""enddate"": null,
   ""employees"": 199,
   ""addressco"": null,
   ""industrycode"": 108200,
   ""industrydesc"": ""Fremstilling af kakao, chokolade og sukkervarer"",
   ""companycode"": 60,
   ""companydesc"": ""Aktieselskab"",
   ""creditstartdate"": null,
   ""creditbankrupt"": false,
   ""creditstatus"": null,
   ""owners"": null,
   ""productionunits"": [
      {
         ""pno"": 1000356440,
         ""main"": false,
         ""name"": ""CARLETTI A/S"",
         ""address"": ""Baldersbuen 45-49"",
         ""zipcode"": 2640,
         ""city"": ""Hedehusene"",
         ""cityname"": ""Baldersbr"",
         ""protected"": false,
         ""phone"": 46590044,
         ""email"": null,
         ""fax"": null,
         ""startdate"": ""01/01 - 1988"",
         ""enddate"": ""31/12 - 2007"",
         ""employees"": ""10-19"",
         ""addressco"": null,
         ""industrycode"": 158400,
         ""industrydesc"": ""Chokolade- og sukkervarefabrikker""
      },
      {
         ""pno"": 1002821681,
         ""main"": true,
         ""name"": ""CARLETTI A/S"",
         ""address"": ""Grenåvej 641"",
         ""zipcode"": 8541,
         ""city"": ""Skødstrup"",
         ""cityname"": null,
         ""protected"": false,
         ""phone"": null,
         ""email"": null,
         ""fax"": null,
         ""startdate"": ""07/12 - 1979"",
         ""enddate"": null,
         ""employees"": 176,
         ""addressco"": null,
         ""industrycode"": 108200,
         ""industrydesc"": ""Fremstilling af kakao, chokolade og sukkervarer""
      },
      {
         ""pno"": 1024433907,
         ""main"": false,
         ""name"": ""CARLETTI A/S"",
         ""address"": ""Dianavej 9A"",
         ""zipcode"": 7100,
         ""city"": ""Vejle"",
         ""cityname"": null,
         ""protected"": false,
         ""phone"": null,
         ""email"": null,
         ""fax"": null,
         ""startdate"": ""01/01 - 2019"",
         ""enddate"": null,
         ""employees"": 23,
         ""addressco"": null,
         ""industrycode"": 108200,
         ""industrydesc"": ""Fremstilling af kakao, chokolade og sukkervarer""
      }
   ],
   ""t"": 100,
   ""version"": 6
}";
    }
}