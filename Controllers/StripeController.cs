using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace CompanySearchBackend.Controllers;

public class CreateCheckoutRequest
{ 
    public decimal TotalAmount { get; set; }
    public int Quantity { get; set; }
}

[ApiController]
[Route("/api/create-checkout-session")]
public class CheckoutApiController : Controller
{
    [HttpPost]
    public ActionResult Create([FromBody] CreateCheckoutRequest request)
    {
        var domain = "http://localhost:5173";
        var stripeClient = new StripeClient(Environment.GetEnvironmentVariable("StripeSecretKey"));
        var options = new SessionCreateOptions
        {
            InvoiceCreation = new SessionInvoiceCreationOptions
            {
                Enabled = true,
            },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Quantity = request.Quantity,
                    Price = "price_1RQulOH7PDOZqMI1cfZElxXz",
                },
            },
            Mode = "payment",
            SuccessUrl = domain + "/payment-result?success=true&session_id={CHECKOUT_SESSION_ID}",
            CancelUrl = domain + "/payment-result?canceled=true",
        };
        var service = new SessionService(stripeClient);
        Session session = service.Create(options);

        return Ok(new { url = session.Url });
    }
}

