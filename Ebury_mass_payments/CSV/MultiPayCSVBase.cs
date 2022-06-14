using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Ebury_mass_payments.CSV
{
    public class MultiPayCSVBase
    {
        public static CsvConfiguration config => new(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            DetectDelimiter = true,
            MissingFieldFound = null,
            HasHeaderRecord = true,
        };

      

        public sealed class PaymentMap : ClassMap<Structures.EburyPayments>
        {
            // 0000000003,D,81733004001,A,12010,2,B,C,37856.43,0,,20211231,,2-Sal�rio,
            public PaymentMap()
            {
                Map(m => m.direction).Index(0).Default("buy");
                Map(m => m.trade_type).Index(1).Default("spot");
                Map(m => m.beneficiary_name).Index(2);
                Map(m => m.beneficiary_address).Index(3);
                Map(m => m.beneficiary_country).Index(5);
                Map(m => m.payment_currency).Index(6).Default("GBP");
                Map(m => m.payment_amount).Index(7);
                Map(m => m.bank_name).Index(9);
                
                Map(m => m.bank_country).Index(12);

                Map(m => m.account_number).Index(13);
                Map(m => m.iban).Index(14);
                Map(m => m.swift_code).Index(15);
                Map(m => m.payment_reference).Index(16);
                Map(m => m.bank_code).Index(17);
                Map(m => m.value_date).Index(18).Default("2022-06-14");

            }
        }

        
    }
}

