using System;

namespace Ebury_mass_payments
{


    public class EburyConfig
    {
        public string ClientId { get { return configuration["ClientId"] ; }}
        public string ClientSecret { get { return configuration["ClientSecret"]; } }
        public string AccountId { get { return configuration["AccountId"]; } }
        public string AccountPassword { get { return configuration["accounts:password"]; } }
        public string AccountEmail { get { return configuration["accounts:name"]; } }


        public const int Timeout = 10;
        public const string BaseAuth = "auth-sandbox.ebury.io";
        public const string BasePayment = "sandbox.ebury.io";

        public const string MassPaymentCreateEndPoint = "https://{BasePayment}/mass-payments?client_id={ClientId}";
        public const string MPI_Errors = "https://{BasePayment}/mass-payments/{$mass_payment_id}/errors?client_id={ClientId}";
        public const string MPI_Details = "https://{BasePayment}/mass-payments?mass_payment_id={$mass_payment_id}&client_id={ClientId}";

        private IConfiguration configuration { get; set; }
        public string ErrorEndPoint = "";
        public const string AuthenticateURL = "https://{BaseAuth}";
        public const string redirect_uri = "https://www.alight.com/about";
        private string state { get; set; }

        public EburyConfig()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().AddJsonFile("ebury_config.json");
            configuration = configurationBuilder.Build();
        }

        public static string GetMassPaymentCreateEndPoint()
        {
            return MassPaymentCreateEndPoint;
        }

      
    }
}

