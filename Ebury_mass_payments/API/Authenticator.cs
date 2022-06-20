using System;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Ebury_mass_payments.API.Responses;

namespace Ebury_mass_payments
{


    public class Authenticator

    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private readonly EburyConfig EConfig = new();

        private string Authtoken { get; set; }
        private string Accesstoken { get; set; }
        private string State { get; set; }

        private List<string> messages { get;set;}

        private TokenCache Tc { get; set; }

        public Authenticator()
        {
            messages = new();
        }

        public static string GenerateState(int length = 8)
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var Charsarr = new char[length];
            var random = new Random();

            for (int i = 0; i < Charsarr.Length; i++)
            {
                Charsarr[i] = characters[random.Next(characters.Length)];
            }
            return new String(Charsarr);
        }

        public string GetState()
        {
            if (String.IsNullOrEmpty(State))
            {
                State = GenerateState(8);
            }
            return State;
        }

        public List<string> GetMessages()
        {
            messages.AddRange(Tc.GetMessages());
            return messages;
        }

        public async Task<bool> Authenticate()
        {
            Tc = new TokenCache();
            string cachedToken = Tc.GetCache();
            if (cachedToken != null)
            {
                Accesstoken = cachedToken;
                messages.Add($"Using cached token {Accesstoken}");
                return true;
            }
            else
            {
                messages.Add("No valid token or expired token found");
            }

            if (!await Authenticate_call())
            {
                messages.Add("Authenticate call failed");
                return false;
            }
            if (!await Login())
            {
                messages.Add("Login call failed");
            }

            bool res = await ObtainAccessToken();
            if (!res)
            {
                messages.Add("ObtainAccessToken call failed");
            }
            messages.Add($"Token is {Accesstoken} and length is {Accesstoken.Length}");

            await Tc.SetCache(Accesstoken);
            return true;
        }

        private async Task<bool> Authenticate_call()
        {

            var uri = EConfig.getAuthenticateURL();

            uri = uri.Replace("{State}", GetState());
            messages.Add($"Authenticate URL = {uri}");
            var getRequest = new HttpRequestMessage(HttpMethod.Get, uri);

            var getResponse = await HttpClient.SendAsync(getRequest);
            getResponse.EnsureSuccessStatusCode();
            if (!getResponse.IsSuccessStatusCode)
            {
                if (getResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    messages.Add("Unauthorized for authenticate");
                }
            }
            else
            {
                string responseBody = await getResponse.Content.ReadAsStringAsync();
                messages.Add($"Authenticate body {responseBody}");

            }

            return true;

        }

        public async Task<bool> Login()
        {
            var values = new Dictionary<string, string>
            {
                { "client_id",EConfig.ClientId },
                { "state",GetState() },
                { "email",EConfig.AccountEmail },
                { "password",EConfig.AccountPassword }
            };

            var data = new FormUrlEncodedContent(values);
            var response = await HttpClient.PostAsync(EburyConfig.LoginURL, data);
            Authtoken = QueryHelpers.ParseQuery(response.RequestMessage.RequestUri.Query)["Code"];
            messages.Add($"Obtained Access Token = {Authtoken}");



            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    messages.Add("Unauthorized for /login");
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    messages.Add("Forbidden for /login");
                }
                return false;
            }
            else
            {
   
                messages.Add("== Return for /login ==");

            }
            return true;
        }

        public async Task<bool> ObtainAccessToken()
        {
            messages.Add($"== getAccessToken for {Authtoken} ==");
            var values = new Dictionary<string, string>
            {
                { "grant_type" , "authorization_code"},
                { "code" , Authtoken},
                { "redirect_uri" , EConfig.RedirectURL}
            };
            var data = new FormUrlEncodedContent(values);
            HttpClient.DefaultRequestHeaders.Clear();
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(EConfig.ClientId + ":" + EConfig.ClientSecret);
            string val = System.Convert.ToBase64String(plainTextBytes);
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + val);

            var response = await HttpClient.PostAsync(EburyConfig.TokenURL, data);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    messages.Add("Unauthorized for /token");
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    messages.Add("Forbidden for /token");
                }
                return false;
            }
            else
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                AccessResponse ar = JsonSerializer.Deserialize<AccessResponse>(responseBody);
                Accesstoken = ar.access_token;
                return !string.IsNullOrEmpty(Accesstoken);
            }

        }

        public string GetAccessToken()
        {

            return Accesstoken;
        }

        
    }
}
