using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdf.ModelUtility.Model.RazorPay
{
    public class OrderResponse
    {
        public string SecretKey { get; set; }
        public string OrderId { get; set; }
        public int? Price { get; set; }
        public string Currency { get; set; }
        public string OrderName { get; set; }
        public string OrderDescription { get; set; }
        public string Email { get; set; }
        public string PdfLink { get; set; }
        public bool IsAvailable { get; set; }
    }
}
