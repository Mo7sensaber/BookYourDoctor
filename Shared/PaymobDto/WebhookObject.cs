using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.PaymobDto
{
    public class WebhookObject
    {
        public long id { get; set; } // Transaction ID
        public bool success { get; set; }
        public int amount_cents { get; set; }
        public string currency { get; set; } = default!;
        public WebhookOrder order { get; set; } = default!;
        public string hmac { get; set; } = default!;
    }
}
