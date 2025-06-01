using Stripe;

namespace CompanySearchBackend.Interfaces;

public interface IStripeInterface
{
    Task<PaymentIntent> CreatePaymentAsync (long amount, string currency = "eur");
    Task<bool> HandleWebHookAsync(string json, string signature);
}