using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdf.ModelUtility.Model
{
    public class UserRegister
    {
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The Password is required")]
        [StringLength(50, ErrorMessage = "Must be between 5 and 20 characters", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string OTP { get; set; }
    }
}
