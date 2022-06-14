using System;
namespace Ebury_mass_payments.Structures;

public class EburyPayments
{
    public string account_number { get; set; } = "";
    public string bank_address { get; set; } = "";
    public string bank_code { get; set; } = "";
    public string bank_country { get;set; }
    public string bank_name { get; set; } = "";
    public string beneficiary_address { get;set; }
    public string beneficiary_name { get;set; }
    public string beneficiary_country { get;set; }
    public string beneficiary_reference { get;set; }
    public string direction { get;set; }
    public string iban { get;set; }
    public string inn { get;set; }
    public string kio { get;set; }
    public string payment_currency { get;set; }
    public double payment_amount { get;set; }
    public string payment_reference { get;set; }
    public string purpose_of_payment { get; set; }
    public string reason_for_trade { get; set; } = "salary_payroll";
    public string russian_central_bank_account { get;set; }
    public string swift_code { get;set; }
    public string trade_type { get; set; }
    public string value_date { get;set; }
    public string vo { get; set; }


    public EburyPayments()
    {
        value_date = "2022-06-12";
        payment_reference = "Some payment reference";
    
    }
}

