using CompanySearchBackend.Interfaces;
using CompanySearchBackend.Models;
using Stripe;
using Stripe.Checkout;

namespace CompanySearchBackend.Services;

public class StripeService(string webhookSecret) : IStripeInterface
{
    private readonly string _webhookSecret = webhookSecret;
    
    public async Task<PaymentIntent> CreatePaymentAsync(long amount, string currency = "eur")
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = amount,
            Currency = currency,
            PaymentMethodTypes = new List<string> { "card" },
            Metadata = new Dictionary<string, string> { { "orderId", "12345" } }
        };
            
        var service = new PaymentIntentService();
        return await service.CreateAsync(options);
    }

    public async Task<bool> HandleWebHookAsync(string json, string signature)
    {
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                signature,
                _webhookSecret
            );
                
            // Handle the event based on its type
            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    await HandleSuccessfulPaymentAsync(paymentIntent);
                    break;
                        
                case "payment_intent.payment_failed":
                    var failedPaymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    await HandleFailedPaymentAsync(failedPaymentIntent);
                    break;
            }
                
            return true;
        }
        catch (StripeException ex)
        {
            Console.WriteLine($"Webhook error: {ex.Message}");
            return false;
        }
    }
    
    private Task HandleSuccessfulPaymentAsync(PaymentIntent paymentIntent)
    {
        Console.WriteLine($"Payment succeeded: {paymentIntent.Id}");
            
        // Example: Update order status in database
        // await _orderRepository.UpdateStatusAsync(paymentIntent.Metadata["orderId"], "Paid");
            
        return Task.CompletedTask;
    }
        
    private Task HandleFailedPaymentAsync(PaymentIntent paymentIntent)
    {
        Console.WriteLine($"Payment failed: {paymentIntent.Id}");
            
        // Example: Update order status in database
        // await _orderRepository.UpdateStatusAsync(paymentIntent.Metadata["orderId"], "Failed");
            
        return Task.CompletedTask;
    }
}