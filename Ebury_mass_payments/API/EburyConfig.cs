using System;
namespace Ebury_mass_payments
{
    public class EburyAccounts
    {
        public string email { get; set; }
        public string password { get; set; }

        public EburyAccounts(string email,string password)
        {
            email = email;
            password = password;
        }
    }

    public class EburyConfig
    {
        public const string client_id = "wEN7xPcgBUnRHW7jrYJfKAGjbHL340ox";
        public const string client_secret = "pHPxWW5AKzArtXtuOLblK1eU9I8H051D";
        public const string account_id = "EBPCLI285600";

        public const int timeout = 10;
        public const string baseAuth = "auth-sandbox.ebury.io";
        public const string basePayment = "sandbox.ebury.io";

        public const string MassPaymentCreateEndPoint = "https://{basePayment}/mass-payments?client_id={client_id}";
        public const string MPI_Errors = "https://{basePayment}/mass-payments/{$mass_payment_id}/errors?client_id={client_id}";


        public string ErrorEndPoint = "";
        public const string AuthenticateURL = "https://{baseAuth}";
        public const string redirect_uri = "https://www.alight.com/about";
        private string state { get; set; }

        public EburyConfig()
        {
            
        }

        public static EburyAccounts getAccounts(string name = "")
        {
            var accounts = new List<EburyAccounts>();
            var a = new EburyAccounts("ebury.api@alight.com", "Alight2022");
            accounts.Add(a);
            return a;
        }

      
    }
}

