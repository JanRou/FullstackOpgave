using fullstackbe.Core.Domain;
using System.Net;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;

namespace fullstackbe.Gateways.Cvrapi
{
    public interface ICvrapi
    {
        /// <summary>
        /// Henter en virksomhed givet et cvr-nummer ellers null
        /// </summary>
        /// <param name="cvr"></param>
        /// <returns>Virksomhed eller null</returns>
        Task<Virksomhed?> Get(int cvr, CancellationToken cancellationToken);
    }

    public class Cvrapi : ICvrapi
    {
        private string urlFormatter;

        public Cvrapi(string urlFormatString)
        {
            urlFormatter = urlFormatString;
        }

        public string FormatUrl(int cvr)
        {
            return string.Format(urlFormatter, cvr);
        }

        public string GetStringFromJsonElement(JsonElement root, string element)
        {
            string? str = root.GetProperty(element).ValueKind == JsonValueKind.String ?
                            root.GetProperty(element).GetString() : "";

            return str != null ? str : "";
        }

        public int GetIntFromJsonElement(JsonElement root, string element)
        {
            // TODO tallet kan være i en tekststreng, så kan man konvertere den ...
            return root.GetProperty(element).ValueKind == JsonValueKind.Number ?
                            root.GetProperty(element).GetInt32() : 0;
        }

        public (string name, string address, int zipcode, string city) ParseJson(string json)
        {
            Virksomhed result = null;
            JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;
            return (GetStringFromJsonElement(root, "name")
                    , GetStringFromJsonElement(root, "address")
                    , GetIntFromJsonElement(root, "zipcode")
                    , GetStringFromJsonElement(root, "city"));

        }
        public async Task<Virksomhed?> Get(int cvr, CancellationToken cancellationToken)
        {
            Virksomhed? result = null;
            using (var httpClient = new HttpClient()) {
                try
                {                    
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "FullstackOpgave"+cvr.ToString());
                    string url = FormatUrl(cvr);
                    var httpResponse = await httpClient.GetAsync( url, cancellationToken);
                    // hmm man er afhængig af at de formatere JSON ens, da konverteringen er striks uden opsætning
                    if (httpResponse != null)
                    {
                        string responseBody = await httpResponse.Content.ReadAsStringAsync();
                        var (name, address, zipcode, city) = ParseJson(responseBody);
                        result = new Virksomhed(cvr, name, address, zipcode, city);
                    }
                }
                catch (Exception ex) 
                { 
                    // Annulleret eller fejl
                }
            }
            return result;
        }
    }
}
