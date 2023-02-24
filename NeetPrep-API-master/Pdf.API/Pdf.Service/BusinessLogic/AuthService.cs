using Pdf.DataAccess;
using Pdf.ModelUtility.Model;
using Pdf.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Pdf.CommonUtility.AuthUtility;
using Pdf.CommonUtility.EmailUtility;
using Pdf.CommonUtility.CommonFunctions;

namespace Pdf.Service.BusinessLogic
{
    public class AuthService: IAuth
    {
        private readonly AuthDA _authDA;
        public AuthService(AuthDA authDA)
        {
            _authDA = authDA;
        }
        /// <summary>
        /// THIS METHOD IS USED TO REGISTER NEW USER
        /// </summary>
        /// <param name="userDetails"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Register(UserRegister userDetails, ref string message)
        {
            bool isUserCreated = false;

            // GETTING USER BY EMAIL IF ALREADY EXIST 
            DataTable user = _authDA.GetUserByEmail(userDetails.Email);
            if (user.Rows.Count > 0)
                message = "User Already Exist";
            else
            {
                // GETTING USER FROM STAGING TO VERIFY OTP
                user = _authDA.GetStagingUser(userDetails.Email);

                if (user.Rows.Count > 0 && CommonFunctions.FncCheckEmpty(user.Rows[0]["Varification_Code"]) != string.Empty && CommonFunctions.FncCheckEmpty(user.Rows[0]["Varification_Code"]) == userDetails.OTP)
                {
                    Guid? UserId = Guid.NewGuid(); 
                    HashingUtility.GetNewPasswordHash(userDetails.Password, out string passwordHash);
                    User newUser = new()
                    {
                        Email = userDetails.Email,
                        PasswordHash = passwordHash,
                        UserId = UserId,
                    };
                    isUserCreated = _authDA.CreateNewUser(newUser);

                    // DELETING FROM STAGING IF USER SUCCESSFULLY CREATED
                    if (isUserCreated)
                    {
                        _authDA.DeleteStagingUser(userDetails.Email);
                        message = "User created successfully";
                    }

                }
                else
                    message = "Something went wrong, Please try again";
            }
            return isUserCreated;
        }

        public bool GenerateOTP(string email, ref string message, bool isforgotPassSec = false)
        {
            bool isOTPCreated = false;
            // GETTING USER BY EMAIL IF ALREADY EXIST 
            DataTable user = _authDA.GetUserByEmail(email);
            // IN CASE OF SIGN-UP EMAIL SHOULD NOT BE ALREADY PRESENT
            if (isforgotPassSec == false && user.Rows.Count > 0)
                message = "User Already Exist";
            // IN CASE OF FORGOT-PASSWORD EMAIL SHOULD BE ALREADY PRESENT
            else if (isforgotPassSec && user.Rows.Count == 0)
                message = "Email does not exist";
            else
            {
                string OTP = HashingUtility.GenerateRendomNumber();

                isOTPCreated = _authDA.CreateUserStaging(email, OTP);

                if (isOTPCreated)
                {
                    message = "Please verify your email by entering verification code which is sent to your email";
                    Email emailDetails = new()
                    {
                        Subject = "OTP - your one time password",
                        Message = "Your one time password is " + OTP,
                        ReceiverAddress = email,
                        ReceiverName = email
                    };
                    bool isEmailSent = EmailUtility.SendMail(emailDetails);
                    if (isEmailSent == false)
                    {
                        message = "Please verify your email";
                        isOTPCreated = false;
                    }
                }
                else
                    message = "Something went wrong please try again";
            }
            return isOTPCreated;
        }

        /// <summary>
        /// THIS METHOD IS USED TO LOGIN A USER
        /// </summary>
        /// <param name="userDetails"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public (bool isUserExist, User userData, string message) Login(UserRegister userDetails, ref string message)
        {
            bool isUserExist = false;
            User userData = null;
            // GETTING USER BY EMAIL IF EXIST 
            DataTable userDT = _authDA.GetUserByEmail(userDetails.Email);
            if (userDT.Rows.Count == 0)
                message = "Email or Password are incorrect";
            //else if (Convert.ToString(userDT.Rows[0]["Verified"]) == "N")
            //    message = "Email is not varified";
            else
            {
                userData = MapUserData(userDT.Rows[0]);
                bool isPassCorrect = HashingUtility.VerifyPassword(userDetails.Password, userData.PasswordHash);

                if (isPassCorrect)
                {
                    message = "Login Success";
                    isUserExist = true;
                }
                else
                    message = "Email or Password are incorrect";
            }
            return (isUserExist, userData, message);
        }

        public bool ChangePassword(GeneralModal userDetails, ref string message)
        {
            bool isPasswordChanged = false;

            // GETTING USER BY EMAIL IF EXIST 
            DataTable user = _authDA.GetUserByEmail(userDetails.Email);
            if (user.Rows.Count == 0)
                message = "User does not Exist";
            else
            {
                // GETTING USER FROM STAGING TO VERIFY OTP
                user = _authDA.GetStagingUser(userDetails.Email);

                if (user.Rows.Count > 0 && CommonFunctions.FncCheckEmpty(user.Rows[0]["Varification_Code"]) != string.Empty && CommonFunctions.FncCheckEmpty(user.Rows[0]["Varification_Code"]) == userDetails.OTP)
                {
                    HashingUtility.GetNewPasswordHash(userDetails.Password, out string passwordHash);
                    User newUserDetails = new()
                    {
                        Email = userDetails.Email,
                        PasswordHash = passwordHash
                    };
                    isPasswordChanged = _authDA.ChangePassword(newUserDetails);

                    // DELETING FROM STAGING IF PASSWORD SUCCESSFULLY CHANGED
                    if (isPasswordChanged)
                    {
                        _authDA.DeleteStagingUser(userDetails.Email);
                        message = "Password changed successfully";
                    }
                    
                }
                else
                    message = "Something went wrong, Please try again";
            }
            return isPasswordChanged;
        }

        private static User MapUserData(DataRow userDT)
        {
            User user = new()
            {
                Email = CommonFunctions.FncCheckEmpty(userDT["email"]),
                PasswordHash = CommonFunctions.FncCheckEmpty(userDT["Password"]),
                UserId = new Guid(CommonFunctions.FncCheckEmpty(userDT["User_Id"]))
            };
            return user;
        }
    }
}
