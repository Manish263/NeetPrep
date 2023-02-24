using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdf.ModelUtility.Model.RazorPay
{
    public class PaymentRequest
    {
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string Signature { get; set; }
    }
}
