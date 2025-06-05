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
    private readonly IConfiguration _configuration;

    public CheckoutApiController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost]
    public ActionResult Create([FromBody] CreateCheckoutRequest request)
    {
        var domain = _configuration["Frontend:Domain"];
        var stripeSecretKey = _configuration["Stripe:SecretKey"];
        
        var stripeClient = new StripeClient(stripeSecretKey);
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
                    Price = _configuration["Stripe:CompanyReportPriceId"],
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