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

        private string access_token { get; set; }
        private string mpi_id { get; set; }

        private List<String> messages { get; set; }
        private List<String> ErrorMessages { get; set; }


        public EburyApi(string token)
        {

            messages = new();
            access_token = token;

        }

        public async Task<List<EburyPaymentsError>> GetPayments(string mpi_id = "")
        {
            var uri = EburyConfig.MPI_Errors;
            client.DefaultRequestHeaders.Clear();
            uri = uri.Replace("{basePayment}", EburyConfig.basePayment);
            uri = uri.Replace("{client_id}", EburyConfig.account_id);
            uri = uri.Replace("{$mass_payment_id}", mpi_id);
            messages.Add("Get Payment Details URL is " + uri);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<EburyPaymentsError>>(responseBody);
            }
            else
            {
                messages.Add("Response code is " + response.StatusCode);
                List<EburyPaymentsError> epe = new();
                return epe;
            }
        }

        public List<String> getMessages()
        {
            
            return messages;
        }

        public async Task<String> SendPayments(MassPaymentInstruction mpi, bool simulate = false)
        {

            //   }
            // private async Task PostJsonHttpClient(string uri, MassPaymentInstruction payload)
            //     {
            var uri = EburyConfig.MassPaymentCreateEndPoint;
            uri = uri.Replace("{client_id}", EburyConfig.account_id);
            uri = uri.Replace("{basePayment}", EburyConfig.basePayment);
                      JsonSerializerOptions options = new()
            {
           //     DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            string jsonString = System.Text.Json.JsonSerializer.Serialize(mpi, options);
            messages.Add("JSON " + jsonString);
            if (simulate)
            {
                return "No MPI ID in simulation";
            }
            client.DefaultRequestHeaders.Clear();
            
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(jsonString, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
            var response = await client.SendAsync(request);
            messages.Add("Making call to URI " + uri + " with access token " + access_token);
            string responseBody = await response.Content.ReadAsStringAsync();

        
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                messages.Add(responseBody);
     
                return "NO MPI ID with Bad request";
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                MassPaymentResponse mpr = JsonSerializer.Deserialize<MassPaymentResponse>(responseBody);
                mpi_id = mpr.mass_payment_id;
                messages.Add("Got MPI ID " + mpi_id);
                return mpi_id;
            }
           
            messages.Add(responseBody);
            return "Failure";
        }
    }
}
