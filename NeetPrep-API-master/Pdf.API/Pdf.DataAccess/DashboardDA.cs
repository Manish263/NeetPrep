using Pdf.CommonUtility.CommonFunctions;
using Pdf.DataAccess.DB;
using Pdf.ModelUtility.Model;
using Pdf.ModelUtility.Model.RazorPay;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdf.DataAccess
{
    public class DashboardDA : DBQueries
    {
        private readonly string _connectionString;
        public DashboardDA(string connectionString)
        {
            _connectionString = connectionString;
        }
        public DataTable GetPdfDetails(string userId)
        {
            using SqlConnection connection = new(_connectionString);
            string query = "select TPD.Id, TPD.Name, TPD.Description, TPD.Price, IIF(TUP.IsAvail is NULL, 0, TUP.IsAvail) [IsAvail] " +
                           "from tbl_PdfDetails AS TPD FULL JOIN " +
                           "(select * from tbl_UsersPdf where User_Id = @userId) AS TUP " +
                           "ON TPD.Id = TUP.Pdf_Id";
            Dictionary<string, object> param = new()
            {
                { "@userId", userId },
            };
            return ReadData(query, CommandType.Text, connection, param);
        }
        public DataTable GetPdfDetailsById(string pdfId, string userId)
        {
            using SqlConnection connection = new(_connectionString);
            string query = "select TPD.Id, TPD.Name, TPD.Description, TPD.Price, TPD.PdfLink, IIF(TUP.IsAvail is NULL, 0, TUP.IsAvail) [IsAvail] " +
                           "from tbl_PdfDetails AS TPD FULL JOIN " +
                           "(select * from tbl_UsersPdf where User_Id = @userId) AS TUP " +
                           "ON TPD.Id = TUP.Pdf_Id where TPD.Id = @pdfId";
            Dictionary<string, object> param = new()
            {
                { "@pdfId", pdfId },
                { "@userId", userId }
            };
            return ReadData(query, CommandType.Text, connection, param);
        }
        public DataTable GetOrderDetailsById(string pdfId, string userId)
        {
            using SqlConnection connection = new(_connectionString);
            string query = "select * from tbl_OrderDetails where Pdf_Id = @pdfId AND User_Id = @userId";
            Dictionary<string, object> param = new()
            {
                { "@pdfId", pdfId },
                { "@userId", userId }
            };
            return ReadData(query, CommandType.Text, connection, param);
        }

        public bool InsertOrderDetails(OrderDetails orderDetails)
        {
            using SqlConnection connection = new(_connectionString);
            string query = "insert into tbl_OrderDetails (Pdf_Id, User_Id, Receipt_Id, Order_Id) " +
                           "values(@pdfId, @userId, @receiptId, @orderId)";
            Dictionary<string, object> param = new()
            {
                { "@pdfId", orderDetails.PdfId },
                { "@userId", orderDetails.UserId },
                { "@receiptId", orderDetails.ReceiptId },
                { "@orderId", orderDetails.OrderId }
            };
            DataTable dt = ReadData(query, CommandType.Text, connection, param);
            return true;
        }
        public bool UpdateOrderDetails(OrderDetails orderDetails)
        {
            using SqlConnection connection = new(_connectionString);
            string query = "update tbl_OrderDetails set Payment_Id = @paymentId, Signature = @signature, IsPaymentDone = 1 where Order_Id = @orderId";
            Dictionary<string, object> param = new()
            {
                { "@paymentId", orderDetails.PaymentId },
                { "@signature", orderDetails.Signature },
                { "@orderId", orderDetails.OrderId }
            };
            DataTable dt = ReadData(query, CommandType.Text, connection, param);
            return true;
        }
        public bool InsertUsersPdf(string userId, string pdfId)
        {
            using SqlConnection connection = new(_connectionString);
            string query = "insert into tbl_UsersPdf (User_Id, Pdf_Id, IsAvail) " +
                           "values(@userId, @pdfId, @isAvail)";
            Dictionary<string, object> param = new()
            {
                { "@userId", userId },
                { "@pdfId", pdfId },
                { "@isAvail", 1 }
            };
            DataTable dt = ReadData(query, CommandType.Text, connection, param);
            return true;
        }
        public DataTable GetPdfIdFromOrderDetails(string orderId)
        {
            using SqlConnection connection = new(_connectionString);
            string query = "select Pdf_Id from tbl_OrderDetails where Order_Id = @orderId";
            Dictionary<string, object> param = new()
            {
                { "@orderId", orderId }
            };
            return ReadData(query, CommandType.Text, connection, param);
        }
        public void InsertPaymentFailed(string userId, PaymentFailedRequest paymentFailedRequest)
        {
            using SqlConnection connection = new(_connectionString);
            string query = "insert into tbl_PaymentFailed(ErrorCode, Description, Source, Step, Reason, OrderId, PaymentId) " +
                           "values(@errorCode, @description, @source, @step, @reason, @orderId, @paymentId)";
            Dictionary<string, object> param = new()
            {
                { "@errorCode", CommonFunctions.FncCheckForeignString(paymentFailedRequest.ErrorCode) },
                { "@description", CommonFunctions.FncCheckForeignString(paymentFailedRequest.Description) },
                { "@source", CommonFunctions.FncCheckForeignString(paymentFailedRequest.Source) },
                { "@step", CommonFunctions.FncCheckForeignString(paymentFailedRequest.Step) },
                { "@reason", CommonFunctions.FncCheckForeignString(paymentFailedRequest.Reason) },
                { "@orderId", CommonFunctions.FncCheckForeignString(paymentFailedRequest.OrderId) },
                { "@paymentId", CommonFunctions.FncCheckForeignString(paymentFailedRequest.PaymentId) }
            };
            ReadData(query, CommandType.Text, connection, param);
        }
        public void InsertContact(UserContact userContact)
        {
            using SqlConnection connection = new(_connectionString);
            string query = "insert into tbl_Contact(Email, Message) " +
                           "values(@email, @message)";
            Dictionary<string, object> param = new()
            {
                { "@email", CommonFunctions.FncCheckForeignString(userContact.Email) },
                { "@message", CommonFunctions.FncCheckForeignString(userContact.Message) }
            };
            ReadData(query, CommandType.Text, connection, param);
        }
        public void InsertPDFDetails(PdfDetails pdfDetails)
        {
            using SqlConnection connection = new(_connectionString);
            string query = "insert into tbl_PdfDetails(Id, Name, Description, Price, PdfLink) " +
                           "values(@id, @name, @desc, @price, @link)";
            Dictionary<string, object> param = new()
            {
                { "@id", CommonFunctions.FncCheckForeignString(pdfDetails.Id) },
                { "@name", CommonFunctions.FncCheckForeignString(pdfDetails.Name) },
                { "@desc", CommonFunctions.FncCheckForeignString(pdfDetails.Description) },
                { "@price", CommonFunctions.FncCheckForeignString(pdfDetails.Price) },
                { "@link", CommonFunctions.FncCheckForeignString(pdfDetails.PdfLink) },
            };
            ReadData(query, CommandType.Text, connection, param);
        }
        public void UpdatePDFDetails(PdfDetails pdfDetails)
        {
            using SqlConnection connection = new(_connectionString);
            string query = "update tbl_PdfDetails set Name = @name, Description = @desc, Price = @price, PdfLink = @link " +
                           "where Id = @id";
            Dictionary<string, object> param = new()
            {
                { "@id", CommonFunctions.FncCheckForeignString(pdfDetails.Id) },
                { "@name", CommonFunctions.FncCheckForeignString(pdfDetails.Name) },
                { "@desc", CommonFunctions.FncCheckForeignString(pdfDetails.Description) },
                { "@price", CommonFunctions.FncCheckForeignString(pdfDetails.Price) },
                { "@link", CommonFunctions.FncCheckForeignString(pdfDetails.PdfLink) },
            };
            ReadData(query, CommandType.Text, connection, param);
        }
        public void DeletePdf(string Id)
        {
            using SqlConnection connection = new(_connectionString);
            string query = "update tbl_PdfDetails set Delete_Flag = 'Y' where Id = @id";
            Dictionary<string, object> param = new()
            {
                { "@id", CommonFunctions.FncCheckForeignString(Id) }
            };
            ReadData(query, CommandType.Text, connection, param);
        }
        public DataTable GetUserType(string Id)
        {
            using SqlConnection connection = new(_connectionString);
            string query = "select User_Type from tbl_Users where User_Id = @id";
            Dictionary<string, object> param = new()
            {
                { "@id", CommonFunctions.FncCheckForeignString(Id) }
            };
            return ReadData(query, CommandType.Text, connection, param);
        }
    }
}
