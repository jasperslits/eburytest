using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ebury_mass_payments.API;
using Ebury_mass_payments.API.Responses;

namespace Ebury_mass_payments
{
    public class DetailsModel : PageModel
   
        {
            private EburyApi eapi { get; set; }
            public List<String> messages { get; set; } = new();
            public EburyPaymentsSummary epe { get; set; }
            public bool hasErrors = false;
            public string mpi_id { get; set; }

            public async Task OnGetAsync(string id)
            {
                mpi_id = id;
                var a = new Authenticator();
                await a.Authenticate();

                messages = a.GetMessages();
                eapi = new EburyApi(a.GetAccessToken());
                epe = await eapi.GetDetails(id);
                messages.Add("Processed " + epe.payments_summary.payments_processed.ToString());
                messages.Add("Errored " + epe.payments_summary.payments_errored.ToString());
                messages.Add("Created " + epe.payments_summary.payments_created.ToString());
                messages.Add("Received " + epe.payments_summary.payments_received.ToString());
                if (epe.payments_summary.payments_errored != 0)
                {
                    hasErrors = true;
                }
            //messages = eapi.getMessages();

        }
        }
    }

