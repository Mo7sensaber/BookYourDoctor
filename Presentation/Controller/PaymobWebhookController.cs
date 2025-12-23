using Microsoft.AspNetCore.Mvc;
using ServiceAbestraction;
using Shared.PaymobDto;

[ApiController]
[Route("api/[controller]")]
public class PaymobWebhookController : ControllerBase
{
    private readonly IPaymobService _paymentService;

    public PaymobWebhookController(IPaymobService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet("callback")]
    public async Task<IActionResult> GetCallback([FromQuery] string id, [FromQuery] string order, [FromQuery] bool success)
    {
        // سنرسل رقم الـ order (الذي يمثل الجزء الرقمي من الـ Intention ID)
        await _paymentService.UpdatePaymentStatusAsync(id, order, success);

        return Content(@"
        <div style='text-align:center; margin-top:50px; font-family:Arial;'>
            <h1 style='color:green;'>Payment Processed Successfully!</h1>
            <p>Your appointment has been confirmed.</p>
        </div>", "text/html");
    }
}