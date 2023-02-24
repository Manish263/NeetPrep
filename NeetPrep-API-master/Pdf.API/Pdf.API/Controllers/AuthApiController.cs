using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Pdf.ModelUtility.Model;
using Pdf.Service.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pdf.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly IAuth _AuthService;
        private readonly IConfiguration _configuration;
        public AuthApiController(IAuth authService, IConfiguration configuration)
        {
            _AuthService = authService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] UserRegister userDetails)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                bool isRegistrationSuccess = _AuthService.Register(userDetails, ref message);

                if (isRegistrationSuccess)
                    return StatusCode(200, new { message });
            }
            else
                message = "Invalid details";

            return StatusCode(400, new { message });
        }

        [HttpPost("generateOTP")]
        public ActionResult GenerateOTP([FromBody] GeneralModal userDetails)
        {
            string email = userDetails.Email;
            string message = string.Empty;

            bool isOTPCreatedSuccess = _AuthService.GenerateOTP(email, ref message);

            if (isOTPCreatedSuccess)
                return StatusCode(200, new { message });

            return StatusCode(400, new { message });
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] UserRegister userDetails)
        {
            string message = string.Empty;
            UserRegister us = userDetails;
            if (ModelState.IsValid)
            {
                var (isLoginSuccess, userData, message1) = _AuthService.Login(userDetails, ref message);

                if (isLoginSuccess)
                {
                    string token = CreateToken(userData);
                    return StatusCode(200, new { message, token });
                }
                else
                    message = message1;
            }
            else
                message = "Invalid Details";
            return StatusCode(400, new { message });
        }

        [HttpPost("genForgotPassOTP")]
        public ActionResult GenForgotPassOTP([FromBody] GeneralModal userDetails)
        {
            string email = userDetails.Email;
            string message = string.Empty;

            bool isOTPCreatedSuccess = _AuthService.GenerateOTP(email, ref message, true);

            if (isOTPCreatedSuccess)
                return StatusCode(200, new { message });

            return StatusCode(400, new { message });
        }
        
        [HttpPost("changePassword")]
        public ActionResult changePassword([FromBody] GeneralModal userDetails)
        {
            string message = string.Empty;
            bool isChangePassSuccess = _AuthService.ChangePassword(userDetails, ref message);

            if (isChangePassSuccess)
                return StatusCode(200, new { message });
            
            else
                message = "Invalid details";

            return StatusCode(400, new { message });
        }

        private string CreateToken(User userData)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Role","REG"),
                    new Claim("Email",userData.Email.ToString()),
                    new Claim("UserId",userData.UserId.ToString())
                }),

                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration.GetSection("AuthDetails:ExpireTimeInHours").Value)),
                Issuer = _configuration.GetSection("AuthDetails:Issuer").Value,
                Audience = _configuration.GetSection("AuthDetails:Audience").Value,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            _configuration.GetSection("AuthDetails:SecurityKey").Value)),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return token;
        }
    }
}
