using Shared.PaymobDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbestraction
{
    public interface IPaymobService
    {
        Task<string> GetPaymentLinkAsync(int appointmentId);
        Task<bool> UpdatePaymentStatusAsync(string transactionId, string orderIdFromUrl, bool success);
    }
}
