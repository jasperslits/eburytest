using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ebury_mass_payments.API;
using Ebury_mass_payments.API.Responses;

namespace Ebury_mass_payments.Pages.Ebury
{
    public class DetailsModel : PageModel
    {
        private EburyApi eapi { get; set; }
        public List<String> messages { get; set; } = new();
        public List<EburyPaymentsError> epe { get; set; }


        private async Task GetPaymentDetails(string mpi_id)
        {

            epe = await eapi.GetPayments(mpi_id);
        }

        public async Task OnGetAsync(string id)
        {
            var a = new Authenticator();
            var r = await a.Authenticate();
            if (r == false)
            {
                return;
            }

            eapi = new EburyApi(a.GetAccessToken());
            await GetPaymentDetails(id);
            messages = eapi.getMessages();

        }
    }
}
