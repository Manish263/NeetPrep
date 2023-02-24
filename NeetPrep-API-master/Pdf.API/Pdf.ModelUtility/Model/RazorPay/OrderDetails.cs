using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdf.ModelUtility.Model.RazorPay
{
    public class OrderDetails
    {
        public string PdfId { get; set; }
        public string UserId { get; set; }
        public string ReceiptId { get; set; }
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string Signature { get; set; }
        public bool IsPaymentDone { get; set; }
    }
}
