using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbestraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // لازم يكون مسجل دخول عشان يدفع
    public class PaymentController(IManagerService manager) : ControllerBase
    {
        [HttpPost("GetPaymentLink/{appointmentId}")]
        public async Task<IActionResult> GetPaymentLink(int appointmentId)
        {
            try
            {
                // بننادي على الخدمة من خلال المانجر
                var paymentLink = await manager.payment.GetPaymentLinkAsync(appointmentId);

                return Ok(new { url = paymentLink });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
