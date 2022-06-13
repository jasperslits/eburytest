using System;
namespace Ebury_mass_payments
{
    public class MassPaymentInstruction
    {
        public string external_reference_id { get; set; }
        public bool auto_commit { get; set; } = true;
        public string sell_currency { get; set; }
        public List<EburyPayments> payment_instructions { get; set; }

        public MassPaymentInstruction()
        {
            external_reference_id = Authenticator.generateState();

        }
    }
}

