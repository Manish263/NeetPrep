using Newtonsoft.Json;
using Pdf.CommonUtility;
using Pdf.CommonUtility.CommonFunctions;
using Pdf.DataAccess;
using Pdf.ModelUtility.Model;
using Pdf.ModelUtility.Model.RazorPay;
using Pdf.Service.Interface;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdf.Service.BusinessLogic
{
    public class DashboardService: IDashboard
    {
        private readonly DashboardDA _dashboardDA;
        public DashboardService(DashboardDA dashboardDA)
        {
            _dashboardDA = dashboardDA;
        }
        public List<PdfDetails> GetPdfDetails(string userId)
        {
            List<PdfDetails> pdfDetails = new();
            DataTable dt = _dashboardDA.GetPdfDetails(userId);
            pdfDetails = (from DataRow dr in dt.Rows
                          select new PdfDetails()
                          {
                              Id = CommonFunctions.FncCheckEmpty(dr["id"]),
                              Name = CommonFunctions.FncCheckEmpty(dr["name"]),
                              Description = CommonFunctions.FncCheckEmpty(dr["description"]),
                              Price = Convert.ToInt32(dr["price"]),
                              IsAvailable = Convert.ToInt16(dr["isAvail"]) == 1,
                          }).ToList();
            return pdfDetails;
        }
        public PdfDetails GetPdfDetailsById(string pdfId, string userId)
        {
            PdfDetails pdfDetails = new();
            DataTable dt = _dashboardDA.GetPdfDetailsById(pdfId, userId);
            pdfDetails = (from DataRow dr in dt.Rows
                          select new PdfDetails()
                          {
                              Id = CommonFunctions.FncCheckEmpty(dr["id"]),
                              Name = CommonFunctions.FncCheckEmpty(dr["name"]),
                              Description = CommonFunctions.FncCheckEmpty(dr["description"]),
                              Price = Convert.ToInt32(dr["price"]),
                              PdfLink = CommonFunctions.FncCheckEmpty(dr["pdfLink"]),
                              IsAvailable = Convert.ToInt16(dr["isAvail"]) == 1,
                          }).FirstOrDefault();
            return pdfDetails;
        }
        public OrderResponse CreateOrder(string email, string userId, string pdfId)
        {
            OrderResponse orderResponse = null;
            //--Getting pdf details
            PdfDetails existingPdfDetails = GetPdfDetailsById(pdfId, userId);
            if (existingPdfDetails.IsAvailable)
            {
                //--Download the pdf
                orderResponse = new()
                {
                    Price = existingPdfDetails.Price,
                    OrderName = existingPdfDetails.Name,
                    OrderDescription = existingPdfDetails.Description,
                    Email = email,
                    PdfLink = existingPdfDetails.PdfLink,
                    IsAvailable = existingPdfDetails.IsAvailable,
                };
            }
            else
            {
                //--Getting order details
                OrderDetails orderDetails = GetOrderDetailsById(pdfId, userId);
                if (orderDetails != null && !orderDetails.IsPaymentDone)
                {
                    //--Order already exist but payment not done
                    orderResponse = new()
                    {
                        SecretKey = RazorpayKeys.Key,
                        OrderId = orderDetails.OrderId,
                        Price = existingPdfDetails.Price,
                        Currency = Currency.INR,
                        OrderName = existingPdfDetails.Name,
                        OrderDescription = existingPdfDetails.Description,
                        Email = email,
                        IsAvailable = orderDetails.IsPaymentDone,
                    };
                }
                else
                {
                    //--Create new order
                    Guid? receiptId = Guid.NewGuid();
                    //--Razorpay
                    Dictionary<string, object> orderReqDetails = new()
                    {
                        { "amount", existingPdfDetails.Price },
                        { "currency", Currency.INR },
                        { "receipt",  receiptId.Value.ToString()},
                        { "notes", new Dictionary<string, string>() { { "productName", existingPdfDetails.Name }, { "productId", existingPdfDetails.Id } } }
                    };

                    RazorpayClient client = new(RazorpayKeys.Key, RazorpayKeys.SecretKey);

                    Order order = client.Order.Create(orderReqDetails);
                    string orderId = order["id"].ToString();

                    //--Storing order details in our DB
                    OrderDetails orderdetails = new()
                    {
                        PdfId = existingPdfDetails.Id,
                        UserId = userId,
                        ReceiptId = receiptId.Value.ToString(),
                        OrderId = orderId
                    };
                    bool isOrderInsertSuccess = _dashboardDA.InsertOrderDetails(orderdetails);
                    //--End

                    orderResponse = new()
                    {
                        SecretKey = RazorpayKeys.Key,
                        OrderId = orderId,
                        Price = existingPdfDetails.Price,
                        Currency = Currency.INR,
                        OrderName = existingPdfDetails.Name,
                        OrderDescription = existingPdfDetails.Description,
                        Email = email,
                        IsAvailable = false,
                    };
                }
            }
            return orderResponse;
        }
        public (bool isSuccess, string message, PaymentResponse paymentResponse) UpdateOrder(string userId, PaymentRequest paymentRequest)
        {
            try
            {
                Dictionary<string, string> attributes = new(); 
                attributes.Add("razorpay_payment_id", paymentRequest.PaymentId); 
                attributes.Add("razorpay_order_id", paymentRequest.OrderId); 
                attributes.Add("razorpay_signature", paymentRequest.Signature); 
                Utils.verifyPaymentSignature(attributes);

                OrderDetails orderDetails = new()
                {
                    OrderId = paymentRequest.OrderId,
                    PaymentId = paymentRequest.PaymentId,
                    Signature = paymentRequest.Signature,
                    IsPaymentDone = true
                };
                _dashboardDA.UpdateOrderDetails(orderDetails);
                string pdfId = GetPdfIdFromOrderDetails(paymentRequest.OrderId);
                _dashboardDA.InsertUsersPdf(userId, pdfId);
                PaymentResponse paymentResponse = new()
                {
                    PaymentId = paymentRequest.PaymentId
                };
                return (true, "Success", paymentResponse);
            }
            catch
            {
                return (false, "Invalid Payment", null);
            }
        }
        public OrderDetails GetOrderDetailsById(string pdfId, string userId)
        {
            OrderDetails orderDetails = new();
            DataTable dt = _dashboardDA.GetOrderDetailsById(pdfId, userId);
            orderDetails = (from DataRow dr in dt.Rows
                          select new OrderDetails()
                          {
                              PdfId = CommonFunctions.FncCheckEmpty(dr["Pdf_Id"]),
                              UserId = CommonFunctions.FncCheckEmpty(dr["User_Id"]),
                              ReceiptId = CommonFunctions.FncCheckEmpty(dr["Receipt_Id"]),
                              OrderId = CommonFunctions.FncCheckEmpty(dr["Order_Id"]),
                              PaymentId = CommonFunctions.FncCheckEmpty(dr["Payment_Id"]),
                              Signature = CommonFunctions.FncCheckEmpty(dr["Signature"]),
                              IsPaymentDone = Convert.ToInt16(dr["IsPaymentDone"]) == 1,
                          }).FirstOrDefault();
            return orderDetails;
        }
        public string GetPdfIdFromOrderDetails(string orderId)
        {
            DataTable dt = _dashboardDA.GetPdfIdFromOrderDetails(orderId);
            string pdfId = dt.Rows[0]["Pdf_Id"].ToString();
            return pdfId;
        }
        public void PaymentFailed(string userId, PaymentFailedRequest paymentFailedRequest) => _dashboardDA.InsertPaymentFailed(userId, paymentFailedRequest);
        public void InsertContact(UserContact userContact) => _dashboardDA.InsertContact(userContact);
        public void InsertPDFDetails(PdfDetails pdfDetails)
        {
            Guid? pdfId = Guid.NewGuid();
            pdfDetails.Id = pdfId.ToString();
            pdfDetails.Price *= 100;
            _dashboardDA.InsertPDFDetails(pdfDetails);
        }
        public void UpdatePDFDetails(PdfDetails pdfDetails)
        {
            pdfDetails.Price *= 100;
            _dashboardDA.UpdatePDFDetails(pdfDetails);
        }
        public void DeletePdf(string Id)
        {
            _dashboardDA.DeletePdf(Id);
        }
        public bool GetEditPermission(string userId)
        {
            DataTable dt = _dashboardDA.GetUserType(userId);
            if (dt != null && dt.Rows.Count > 0)
            {
                return CommonFunctions.FncCheckEmpty(dt.Rows[0]["User_Type"]) == "SUPERUSER";
            }
            return false;
        }
    }
}
