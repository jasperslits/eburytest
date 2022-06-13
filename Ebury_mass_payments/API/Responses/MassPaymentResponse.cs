using System;
namespace Ebury_mass_payments.API.Responses
{
    public class MassPaymentResponse
    {
        public string mass_payment_id { get; set; }
        public string external_reference_id { get; set; }
        public string mass_payment_status { get; set; }

        public MassPaymentResponse()
        {
        }
    }
}

