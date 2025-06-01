namespace CompanySearchBackend.Models;

public class StripeModels
{
    public class PaymentRequest
    {
        public long Amount { get; set; }
        public string Currency { get; set; } = "eur";
    }

    public class PaymentResponse
    {
        public string ClientSecret { get; set; }
    }
    
    public class StripeOptions
    {
        public string option { get; set; }
    }
}