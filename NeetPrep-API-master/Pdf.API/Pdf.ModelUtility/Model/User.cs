using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Pdf.ModelUtility.Model
{
    public class User
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int VarificationCode { get; set; }
        public Guid? UserId { get; set; }
    }
}
