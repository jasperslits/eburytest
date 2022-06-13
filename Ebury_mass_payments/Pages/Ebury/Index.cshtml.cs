using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ebury_mass_payments.CSV;

namespace Ebury_mass_payments.Pages.Ebury
{
    public class IndexModel : PageModel
    {
        public List<String> messages { get; set; } = new();

        private EburyApi eapi { get; set; }

        public async Task OnGetAsyncback()
        {
            List<EburyPayments> ep = new();
           
            ep = await EburyPaymentLoader.LoadPayments("wes2.csv");

        }

       

        public async Task OnGetAsync()
        {
          var a = new Authenticator();
          var r = await a.Authenticate();
          if (r == false)
            {
                return;
            }

           MassPaymentInstruction mpi = new MassPaymentInstruction();
      
           mpi.payment_instructions = await EburyPaymentLoader.LoadPayments("wes2.csv");
            mpi.sell_currency = "EUR";
            eapi = new EburyApi(a.GetAccessToken());
            await eapi.SendPayments(mpi);
    
            messages = eapi.getMessages();
            
        }
    }
}

