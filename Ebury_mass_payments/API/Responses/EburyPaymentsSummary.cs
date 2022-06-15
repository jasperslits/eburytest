using System;

// [{"error":null,"external_reference_id":"aMtjhLql",
// "links":{"funding_account_href":"/mass-payments/5f3c9522-779a-4741-9771-70a4f9520940/funding-account",
// "payments_created_href":"/payments?mass_payment_id=5f3c9522-779a-4741-9771-70a4f9520940","
// payments_errored_href":"/mass-payments/5f3c9522-779a-4741-9771-70a4f9520940/errors",
// "trades_href":"/trades?client_id=EBPCLI285600&mass_payment_id=5f3c9522-779a-4741-9771-70a4f9520940"},
// "mass_payment_id":"5f3c9522-779a-4741-9771-70a4f9520940",
// "mass_payment_status":"Creating",
// "payments_summary":{"payments_created":0,"payments_errored":0,"payments_processed":1,"payments_received":1},
// "sell_currency":"EUR","trades_created":0}]


namespace Ebury_mass_payments.API.Responses
{
    public class EburyPaymentSum
    {
        public int payments_created { set; get; } = 0;
        public int payments_errored { set; get; } = 0;
        public int payments_processed { set; get; } = 0;
        public int payments_received { set; get; } = 0;
    }

    public class EburyPaymentsSummary
    {
        public EburyPaymentSum payments_summary { get; set; }
   
        public string mass_payment_status { get; set; }

        public EburyPaymentsSummary()
        {
        }
    }
}

