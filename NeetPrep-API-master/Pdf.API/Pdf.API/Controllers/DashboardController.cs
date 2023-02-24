using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pdf.ModelUtility.Model;
using Pdf.ModelUtility.Model.RazorPay;
using Pdf.Service.Interface;
using System.Security.Claims;

namespace Pdf.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboard _dashboardService;
        private readonly IConfiguration _configuration;
        public DashboardController(IDashboard dashboard, IConfiguration configuration)
        {
            _dashboardService = dashboard;
            _configuration = configuration;
        }
        [HttpGet("getPdfDetails")]
        public ActionResult GetPdfDetails()
        {
            string userId = Convert.ToString(User.Claims.First(c => c.Type == "UserId").Value);
            List<PdfDetails> pdfDetails = _dashboardService.GetPdfDetails(userId);
            return StatusCode(200, new { pdfDetails });
        }
        [HttpPost("createOrder")]
        public ActionResult CreateOrder(string Id)
        {
            string email = Convert.ToString(User.Claims.First(c => c.Type == "Email").Value);
            string userId = Convert.ToString(User.Claims.First(c => c.Type == "UserId").Value);
            OrderResponse orderResponse = _dashboardService.CreateOrder(email, userId, Id);
            return StatusCode(200, new { orderResponse });
        }
        [HttpPost("updateOrder")]
        public ActionResult UpdateOrder([FromBody] PaymentRequest paymentRequest)
        {
            string userId = Convert.ToString(User.Claims.First(c => c.Type == "UserId").Value);
            var (isSuccess, message, paymentResponse) = _dashboardService.UpdateOrder(userId, paymentRequest);
            return StatusCode(200, new { isSuccess, paymentResponse, message });
        }
        [HttpPost("paymentFailed")]
        public ActionResult PaymentFailed([FromBody] PaymentFailedRequest paymentFailedRequest)
        {
            string userId = Convert.ToString(User.Claims.First(c => c.Type == "UserId").Value);
            _dashboardService.PaymentFailed(userId, paymentFailedRequest);
            string message = "success";
            return StatusCode(200,new { message });
        }
        [HttpPost("userContact")]
        public ActionResult UserContact([FromBody] UserContact userContact)
        {
            _dashboardService.InsertContact(userContact);
            string message = "success";
            return StatusCode(200,new { message });
        } 
        [HttpPost("addNewPdf")]
        public ActionResult AddNewPdf([FromBody] PdfDetails pdfDetails)
        {
            _dashboardService.InsertPDFDetails(pdfDetails);
            string message = "success";
            return StatusCode(200,new { message });
        }
        [HttpPost("updatePdf")]
        public ActionResult UpdatePdf([FromBody] PdfDetails pdfDetails)
        {
            _dashboardService.UpdatePDFDetails(pdfDetails);
            string message = "success";
            return StatusCode(200,new { message });
        }
        [HttpPost("deletePdf")]
        public ActionResult DeletePdf(string Id)
        {
            _dashboardService.DeletePdf(Id);
            string message = "success";
            return StatusCode(200,new { message });
        }
        [HttpGet("getEditPermission")]
        public ActionResult GetEditPermission()
        {
            string userId = Convert.ToString(User.Claims.First(c => c.Type == "UserId").Value);
            bool permission = _dashboardService.GetEditPermission(userId);
            return StatusCode(200,new { hasEditPermission = permission });
        }
    }
}