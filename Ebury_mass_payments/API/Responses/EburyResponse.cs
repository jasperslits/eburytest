using System;
namespace Ebury_mass_payments.API.Responses
{
    public class EburyErrorDetail
    {
        public string code { get; set; }
        public string message { get; set; }
    }

    public class EburyPaymentsError : EburyPayments
    {
        public List<EburyErrorDetail> errors { get; set; }

        public EburyPaymentsError()
        {
        }
    }
}

