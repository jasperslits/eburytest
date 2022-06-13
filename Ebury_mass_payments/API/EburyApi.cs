using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using Ebury_mass_payments.API.Responses;

namespace Ebury_mass_payments
{
    public class EburyApi
    {
        private static readonly HttpClient client = new HttpClient();

       private string access_token { get; set; }
       private string mpi_id { get; set; }

        private List<String> messages { get; set; }
 

        public EburyApi(string token)
        {

            messages = new();
            access_token = token;
            
        }

        public async Task SendPayments(MassPaymentInstruction mpi)
        {
            await PostJsonHttpClient(EburyConfig.MassPaymentCreateEndPoint,mpi);

        }

        public async Task GetPayments()
        {
            var uri = EburyConfig.MPI_Errors;
            client.DefaultRequestHeaders.Clear();
            mpi_id = "ef8595d0-cc66-495b-8a88-d04e1ac6f7af";
            uri = uri.Replace("{client_id}", EburyConfig.account_id);
            uri = uri.Replace("{$mass_payment_id}", mpi_id);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
            var response = await client.SendAsync(request);
        }

        public List<String> getMessages()
        {
            return messages;
        }

        private async Task PostJsonHttpClient(string uri, MassPaymentInstruction payload)
        {
            uri = uri.Replace("{client_id}", EburyConfig.account_id);
            JsonSerializerOptions options = new()

            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            string jsonString = JsonSerializer.Serialize(payload,options);
            messages.Add("JSON " + jsonString);
            client.DefaultRequestHeaders.Clear();

            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(jsonString, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
            var response = await client.SendAsync(request);
            messages.Add("Making call to URI " + uri + " with access token " + access_token);
            string responseBody = await response.Content.ReadAsStringAsync();
            
            messages.Add(responseBody);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                messages.Add(responseBody);
                Console.WriteLine(responseBody);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                MassPaymentResponse mpr = JsonSerializer.Deserialize<MassPaymentResponse>(responseBody);
                mpi_id = mpr.mass_payment_id;
                messages.Add(mpi_id);
                Console.WriteLine(mpr.mass_payment_id);
            }


            //            { "code":"INVALID_REQUEST","details":"{\"payment_instructions\": {\"0\": {\"bank_name\": [\"Missing data for required field.\"], \"bank_address\": [\"Missing data for required field.\"], \"bank_code\": [\"Missing data for required field.\"], \"value_date\": [\"Missing data for required field.\"], \"account_number\": [\"Missing data for required field.\"], \"payment_reference\": [\"Missing data for required field.\"]}, \"1\": {\"bank_name\": [\"Missing data for required field.\"], \"bank_address\": [\"Missing data for required field.\"], \"bank_code\": [\"Missing data for required field.\"], \"value_date\": [\"Missing data for required field.\"], \"account_number\": [\"Missing data for required field.\"], \"payment_reference\": [\"Missing data for required field.\"]}}}","message":"The request is invalid, please correct. Check details for more info"}

            //  response.EnsureSuccessStatusCode();




            // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
            //       messages.Add("Token = " + access_token);
            //   client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));




            /*


            postResponse.EnsureSuccessStatusCode();

            if (!postResponse.IsSuccessStatusCode)
            {
               

                if (postResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    messages.Add("Unauthorized for "+uri);
                }
                if (postResponse.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    messages.Add("Forbidden for " + uri);
              
                }
                string responseBody = await postResponse.Content.ReadAsStringAsync();
                messages.Add(responseBody);
            } else
            {
                string responseBody = await postResponse.Content.ReadAsStringAsync();
                messages.Add(responseBody);
            }

            //     messages.Add(postResponse);
            //     postResponse.EnsureSuccessStatusCode();
            */
        }



        

        
    }
}

