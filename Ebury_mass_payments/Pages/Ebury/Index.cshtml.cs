using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ebury_mass_payments.CSV;
using Ebury_mass_payments.Structures;
using System.IO;

namespace Ebury_mass_payments.Pages.Ebury
{
    public class IndexModel : PageModel
    {
        public List<String> AuthMessages { get; set; } = new();
        public List<String> messages { get; set; } = new();
        public string MpiId { get; set; }

        private EburyApi Eapi { get; set; }

        public async Task OnGetAsync(string id)
        {
            var f = $"Uploads/{id}";
            if (!System.IO.File.Exists(f))
            {
                messages.Add($"File {f} does not exist");
                return;
            }

            var a = new Authenticator();
            var r = await a.Authenticate();
            if (r == false)
            {
                AuthMessages = a.GetMessages();
                return;
            }

            MassPaymentInstruction mpi = new MassPaymentInstruction();

            mpi.payment_instructions = await EburyPaymentLoader.LoadPayments(id);
            mpi.sell_currency = "EUR";
            Eapi = new EburyApi(a.GetAccessToken());
            MpiId = await Eapi.SendPayments(mpi,false);
            AuthMessages = a.GetMessages();
            messages = Eapi.getMessages();
    

        }
    }
}

