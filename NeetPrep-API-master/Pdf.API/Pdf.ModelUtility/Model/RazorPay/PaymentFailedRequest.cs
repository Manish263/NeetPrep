using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdf.ModelUtility.Model.RazorPay
{
    public class PaymentFailedRequest
    {
        public string ErrorCode { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Step { get; set; }
        public string Reason { get; set; }
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
    }
}
