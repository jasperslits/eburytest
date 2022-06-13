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


        public async Task OnGetAsync(string id)
        {
            var a = new Authenticator();
            if (! await a.Authenticate()) {
                messages = a.GetMessages();
            }
            
            eapi = new EburyApi(a.GetAccessToken());
            epe = await eapi.GetPayments(id);
            messages = eapi.getMessages();

        }
    }
}
