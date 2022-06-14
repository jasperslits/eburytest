using System;
using Ebury_mass_payments.CSV;
using Ebury_mass_payments.Structures;

namespace Ebury_mass_payments
{
    public class EburyPaymentLoader
    {
        public EburyPaymentLoader()
        {
        }


        public static async Task<List<EburyPayments>> LoadPayments(string filename = "")
        {
            var payments = new List<EburyPayments>();
            if (! string.IsNullOrEmpty(filename))
            {
                payments = await MultiPayCSVReader.LoadData(filename);
                return payments;
            }

            var p = new EburyPayments()
            {
                bank_country = "NL",
                payment_amount = 1000.50,
                iban = "NL59RABO0171524578",
                beneficiary_address = "De Huysacker 61",
                beneficiary_country = "NL",
                beneficiary_name = "Slits",
                beneficiary_reference = "35355352",
                direction = "buy",
                payment_currency = "EUR",
               
                swift_code = "RABONL2U",
                reason_for_trade = "salary_payroll",
                purpose_of_payment = "Salary",
                trade_type = "spot",
                bank_name = "Rabobank",
                bank_address = "Some address",
                account_number = "171524578",
                bank_code = "12334"

            };
            var q = new EburyPayments()
            {
                bank_country = "NL",
                payment_amount = 1000.50,
                iban = "NL59RABO0171524578",
                beneficiary_address = "De Huysacker 61",
                beneficiary_country = "NL",
                beneficiary_name = "Slits",
                beneficiary_reference = "35355352",
                direction = "buy",
                payment_currency = "EUR",
                purpose_of_payment = "Salary",
                swift_code = "RABONL2U",
                reason_for_trade = "salary_payroll",
                trade_type = "spot"

            };

            payments.Add(p);
            return payments;
        }
    }
}

