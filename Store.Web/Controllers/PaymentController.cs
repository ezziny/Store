using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.BasketService.Dtos;
using Store.Services.Services.PaymentService;
using Stripe;

namespace Store.Web.Controllers;

public class PaymentController: BaseController
{
    private readonly IPaymentService _paymentService;
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    public PaymentController(IPaymentService paymentService, IConfiguration configuration, ILogger logger)
    {
        _paymentService = paymentService;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(CustomerBasketDto basket)
        => Ok(await _paymentService.CreateOrUpdatePaymentIntent(basket)); //truth be said i don't think we'll use this like.. EVER

    [HttpPost]
    public async Task<IActionResult> Webhook()
    {
        {
            string endpointSecret = _configuration["Stripe:endpointSecret"];
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                var signatureHeader = Request.Headers["Stripe-Signature"];

                stripeEvent = EventUtility.ConstructEvent(json,
                    signatureHeader, endpointSecret);
                PaymentIntent paymentIntent;

                if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("payment succeeded:", paymentIntent.Id);
                    await _paymentService.UpdateOrderPaymentSucceeded(paymentIntent.Id);
                    _logger.LogInformation("order status updated to payment succeeded:", paymentIntent.Id);

                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("payment failed:", paymentIntent.Id);
                    await _paymentService.UpdateOrderPaymentFailed(paymentIntent.Id);
                    _logger.LogInformation("order status updated to payment failed:", paymentIntent.Id);

                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentCreated)
                {
                    _logger.LogInformation("payment created");
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}