using Pdf.ModelUtility.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdf.Service.Interface
{
    public interface IAuth
    {
        bool Register(UserRegister userRegister, ref string message);
        (bool isUserExist, User userData, string message) Login(UserRegister userDetails, ref string message);
        bool GenerateOTP(string email, ref string message, bool isforgotPassSec = false);
        bool ChangePassword(GeneralModal userDetails, ref string message);
    }
}
