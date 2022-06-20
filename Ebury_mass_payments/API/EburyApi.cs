using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using Ebury_mass_payments.API.Responses;
using Ebury_mass_payments.Structures;

namespace Ebury_mass_payments
{
    public class EburyApi
    {
        private static readonly HttpClient client = new HttpClient();
        private EburyConfig EConfig = new();

        private string AccessToken { get; set; }
        private string MpiId { get; set; }

        private List<String> messages { get; set; }
        private List<String> ErrorMessages { get; set; }


        public EburyApi(string token)
        {

            messages = new();
            AccessToken = token;

        }

        public async Task<List<EburyPaymentsError>> GetPayments(string mpi_id = "")
        {
            var uri = EConfig.getURL("error",mpi_id);
            client.DefaultRequestHeaders.Clear();
  
            messages.Add($"Get Payment Details URL is {uri}");
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<EburyPaymentsError>>(responseBody);
            }
            else
            {
                messages.Add($"Response code is {response.StatusCode}");
                List<EburyPaymentsError> epe = new();
                return epe;
            }
        }

        public async Task<EburyPaymentsSummary> GetDetails(string mpi_id = "")
        {
            var uri = EConfig.getURL("summary",mpi_id);
            client.DefaultRequestHeaders.Clear();
            messages.Add($"Get Payment Details URL is {uri}");
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
               // Console.WriteLine(responseBody);
                return JsonSerializer.Deserialize<List<EburyPaymentsSummary>>(responseBody).First();
                
            }
            else
            {
                messages.Add($"Response code is {response.StatusCode}");
                EburyPaymentsSummary epe = new();
                return epe;
            }
        }

        public List<String> getMessages()
        {
            
            return messages;
        }

        public async Task<String> SendPayments(MassPaymentInstruction mpi, bool simulate = false)
        {

            var uri = EConfig.GetMassPaymentCreateEndPoint();

            string jsonString = System.Text.Json.JsonSerializer.Serialize(mpi);
            messages.Add($"JSON {jsonString}");
            if (simulate)
            {
                return "No MPI ID in simulation";
            }
            client.DefaultRequestHeaders.Clear();
            
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(jsonString, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var response = await client.SendAsync(request);
            messages.Add($"Making call to URI {uri} with access token {AccessToken}");
            string responseBody = await response.Content.ReadAsStringAsync();

        
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                messages.Add(responseBody);
     
                return "NO MPI ID with Bad request";
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                MassPaymentResponse mpr = JsonSerializer.Deserialize<MassPaymentResponse>(responseBody);
                MpiId = mpr.mass_payment_id;
                messages.Add($"Got MPI ID {MpiId}");
                return MpiId;
            }
           
            messages.Add(responseBody);
            return "Failure";
        }
    }
}
