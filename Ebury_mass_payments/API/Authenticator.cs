using System;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Ebury_mass_payments.API.Responses;

namespace Ebury_mass_payments
{


    public class Authenticator

    {
        private static readonly HttpClient client = new HttpClient();

        private string authtoken { get; set; }
        private string accesstoken { get; set; }
        private string state { get; set; }

        private List<string> messages { get;set;}

        private TokenCache tc { get; set; }

        public Authenticator()
        {
            messages = new();
        }

        public static string GenerateState(int length = 8)
        {
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
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
            if (String.IsNullOrEmpty(state))
            {
                state = GenerateState(8);
            }
            return state;
        }

        public List<string> GetMessages()
        {
            return messages;
        }


        public bool IsAuthorized()
        {
            return String.IsNullOrEmpty(authtoken);
        }

        public async Task<bool> Authenticate()
        {
            tc = new TokenCache();
            var cachedToken = tc.GetCache();
            if (cachedToken != null)
            {
                accesstoken = cachedToken;
                messages.Add("Using cached token " + accesstoken);
                return true;
            }
            else
            {
                messages.Add("No valid token found");
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

            var res = await ObtainAccessToken();
            if (!res)
            {
                messages.Add("ObtainAccessToken call failed");
            }
            messages.Add("Token is " + accesstoken + " and length is " + accesstoken.Length);

            await tc.SetCache(accesstoken);
            return true;
        }

        private async Task<bool> Authenticate_call()
        {

            var uri = "{authenticate_uri}/authenticate?scope=openid&response_type=code&client_id=$client_id&state=$state&redirect_uri=$redirect_uri";
            uri = uri.Replace("{authenticate_uri}", EburyConfig.AuthenticateURL);
            uri = uri.Replace("{baseAuth}", EburyConfig.baseAuth);
            uri = uri.Replace("$client_id", EburyConfig.client_id);
            uri = uri.Replace("$redirect_uri", EburyConfig.redirect_uri);
            uri = uri.Replace("$state", GetState());
            messages.Add("Authenticate URL = " + uri);

            var getRequest = new HttpRequestMessage(HttpMethod.Get, uri);


            var getResponse = await client.SendAsync(getRequest);
            Console.Write((int)getResponse.StatusCode);

            var content = await getResponse.Content.ReadAsStringAsync();
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
                messages.Add("Authenticate body " + responseBody);

            }

            return true;

        }

        public async Task<bool> Login()
        {
            var values = new Dictionary<string, string>
            {
                { "client_id",EburyConfig.client_id },
                { "state",GetState() },
                { "email","ebury.api@alight.com" },
                { "password","Alight2022" }
            };

            var data = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(EburyConfig.AuthenticateURL.Replace("{baseAuth}", EburyConfig.baseAuth) + "/login", data);

            authtoken = QueryHelpers.ParseQuery(response.RequestMessage.RequestUri.Query)["Code"];
            messages.Add("Obtained Access Token = " + authtoken);



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
                string responseBody = await response.Content.ReadAsStringAsync();
                messages.Add("== Return for /login ==");
             //   messages.Add(responseBody);

            }
            return true;
        }

        public async Task<bool> ObtainAccessToken()
        {
            messages.Add("== getAccessToken for " + authtoken + "==");
            var values = new Dictionary<string, string>
            {
                { "grant_type" , "authorization_code"},
                { "code" , authtoken},
                { "redirect_uri" , EburyConfig.redirect_uri}
            };
            var data = new FormUrlEncodedContent(values);
            client.DefaultRequestHeaders.Clear();
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(EburyConfig.client_id + ":" + EburyConfig.client_secret);
            string val = System.Convert.ToBase64String(plainTextBytes);
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + val);

            var response = await client.PostAsync(EburyConfig.AuthenticateURL.Replace("{baseAuth}", EburyConfig.baseAuth) + "/token", data);
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
                accesstoken = ar.access_token;
                return !string.IsNullOrEmpty(accesstoken);
            }

        }

        public string GetAccessToken()
        {

            return accesstoken;
        }

        public string getAuthToken()
        {


            if (String.IsNullOrWhiteSpace(authtoken))
            {
                return "0c1a4cb51783459c92c06fe8907e538b";
            }
            else
            {
                return authtoken;
            }
        }
    }
}
