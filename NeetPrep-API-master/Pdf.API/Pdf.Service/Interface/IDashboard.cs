using Pdf.ModelUtility.Model;
using Pdf.ModelUtility.Model.RazorPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdf.Service.Interface
{
    public interface IDashboard
    {
        List<PdfDetails> GetPdfDetails(string userId);
        PdfDetails GetPdfDetailsById(string pdfId, string userId);
        OrderResponse CreateOrder(string email, string userId, string pdfId);
        (bool isSuccess, string message, PaymentResponse paymentResponse) UpdateOrder(string userId, PaymentRequest paymentRequest);
        OrderDetails GetOrderDetailsById(string pdfId, string userId);
        void PaymentFailed(string userId, PaymentFailedRequest paymentFailedRequest);
        void InsertContact(UserContact userContact);
        void InsertPDFDetails(PdfDetails pdfDetails);
        void UpdatePDFDetails(PdfDetails pdfDetails);
        void DeletePdf(string Id);
        bool GetEditPermission(string userId);
    }
}
