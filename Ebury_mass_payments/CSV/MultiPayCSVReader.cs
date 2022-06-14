using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using Ebury_mass_payments.CSV;
using Ebury_mass_payments.Structures;

namespace Ebury_mass_payments.CSV;
public class MultiPayCSVReader : MultiPayCSVBase
{

    public static async Task<List<EburyPayments>> LoadData(string generated)
    {
        List<EburyPayments> dRecords = new();
     
        using StreamReader reader = new($"Uploads/{generated}");
        using CsvReader csv = new(reader, config);
        {
            csv.Context.TypeConverterOptionsCache.GetOptions<string>().NullValues.Add(string.Empty);
            csv.Read();
            csv.Read();
            csv.Read();


            csv.Context.RegisterClassMap<PaymentMap>();

            while (csv.Read())
            {
                        dRecords.Add(csv.GetRecord<EburyPayments>());
            }
            return dRecords;

        
    }
}
}