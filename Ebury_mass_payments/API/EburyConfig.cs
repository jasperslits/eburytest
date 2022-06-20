using System;

namespace Ebury_mass_payments
{


    public class EburyConfig
    {
        public string ClientId { get { return configuration["ClientId"] ; }}
        public string ClientSecret { get { return configuration["ClientSecret"]; } }
        public string AccountId { get { return configuration["AccountId"]; } }
        public string RedirectURL { get { return configuration["RedirectURL"]; } }
        public string AccountPassword { get { return configuration["accounts:password"]; } }
        public string AccountEmail { get { return configuration["accounts:name"]; } }


        public const int Timeout = 10;
        public const string BaseAuth = "auth-sandbox.ebury.io";
        public const string BasePayment = "sandbox.ebury.io";

        public const string MassPaymentCreateEndPoint = "https://{BasePayment}/mass-payments?client_id={ClientId}";
        private const string MPIErrorsURL = "https://{BasePayment}/mass-payments/{$mass_payment_id}/errors?client_id={ClientId}";
        private const string MPISummaryURL = "https://{BasePayment}/mass-payments?mass_payment_id={$mass_payment_id}&client_id={ClientId}";
        private const string AuthenticateURL = "https://{BaseAuth}/authenticate?scope=openid&response_type=code&client_id={ClientId}&state={State}&redirect_uri={RedirectURL}";
        public const string LoginURL = $"https://{BaseAuth}/login";
        public const string TokenURL = $"https://{BaseAuth}/token";

        private IConfiguration configuration { get; set; }
  
        private string State { get; set; }

        public string getURL(string type,string mpi_id)
        {
            var uri = (type == "summary") ? MPISummaryURL : MPIErrorsURL;
            uri = uri.Replace("{ClientId}", AccountId);
            uri = uri.Replace("{BasePayment}", BasePayment);

            uri = uri.Replace("{$mass_payment_id}", mpi_id);
            return uri;
        }

        public string getAuthenticateURL()
        {
            var uri = AuthenticateURL.Replace("{BaseAuth}", BaseAuth);
            uri = uri.Replace("{ClientId}", ClientId);
            uri = uri.Replace("{RedirectURL}", RedirectURL);
            return uri;
        }

        public EburyConfig()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().AddJsonFile("ebury_config.json");
            configuration = configurationBuilder.Build();
        }

        public string GetMassPaymentCreateEndPoint()
        {
            var uri = MassPaymentCreateEndPoint.Replace("{ClientId}", AccountId);
            return uri.Replace("{BasePayment}", BasePayment);
           
        }

      
    }
}

