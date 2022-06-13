using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ebury_mass_payments.Pages.Ebury
{
    public class IndexModel : PageModel
    {
        public List<String> messages { get; set; } = new();

        public async Task OnGetAsync()
        {
          var a = new Authenticator();
          var r = await a.Authenticate();
          if (r == false)
            {
                return;
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
                purpose_of_payment = "Salary",
                swift_code = "RABONL2U",
                reason_for_trade = "salary_payroll",
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

            var payments = new List<EburyPayments>();
            payments.Add(p);
           // payments.Add(q);
            MassPaymentInstruction mpi = new MassPaymentInstruction();
            mpi.auto_commit = true;
      
            mpi.payment_instructions = payments;
            mpi.sell_currency = "EUR";
            var m = new EburyApi(a.GetAccessToken());
          //  await m.SendPayments(mpi);
            await m.GetPayments();
            messages = m.getMessages();
            
        }
    }
}

