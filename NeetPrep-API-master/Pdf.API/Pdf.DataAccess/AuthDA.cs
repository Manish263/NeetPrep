using Pdf.DataAccess.DB;
using Pdf.ModelUtility.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Pdf.DataAccess
{
    public class AuthDA : DBQueries
    {
        private readonly string _connectionString;
        public AuthDA(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable GetUserByEmail(string email)
        {
            using SqlConnection connection = new(_connectionString);
            string query = @"select * from tbl_Users where email = @email and Delete_Flag = 'N' ";
            Dictionary<string, object> param = new()
            {
                { "@email", email },
            };
            return ReadData(query, CommandType.Text, connection, param);
        }

        public DataTable GetStagingUser(string email)
        {
            using SqlConnection connection = new(_connectionString);
            string query = @"select top 1 * from tbl_Users_Staging where email = @email and Delete_Flag = 'N' order by id desc";
            Dictionary<string, object> param = new()
            {
                { "@email", email },
            };
            return ReadData(query, CommandType.Text, connection, param);
        }

        public void DeleteStagingUser(string email)
        {
            using SqlConnection connection = new(_connectionString);
            string query = @"update tbl_Users_Staging set Delete_Flag = 'Y' where email = @email and Delete_Flag = 'N' ";
            Dictionary<string, object> param = new()
            {
                { "@email", email },
            };
            ReadData(query, CommandType.Text, connection, param);
        }

        public bool CreateUserStaging(string email, string otp)
        {
            using SqlConnection connection = new(_connectionString);
            string query = @"insert into tbl_Users_Staging(Email, Varification_Code) values (@email, @otp)";
            Dictionary<string, object> param = new()
            {
                { "@email", email },
                { "@otp", otp}
            };
            DataTable dt = ReadData(query, CommandType.Text, connection, param);
            //if (dt != null && dt.Rows.Count > 0)
            //    return true;
            //return false;
            return true;
        }

        public bool CreateNewUser(User userDetails)
        {
            using SqlConnection connection = new(_connectionString);
            string query = @"insert into tbl_Users(Email, Password, User_Id) values (@email, @pass, @userId)";

            Dictionary<string, object> param = new()
            {
                { "@email", userDetails.Email },
                { "@pass", userDetails.PasswordHash },
                { "@userId", userDetails.UserId }
            };
            DataTable dt = ReadData(query, CommandType.Text, connection, param);
            //if (dt != null && dt.Rows.Count > 0)
            //    return true;
            //return false;
            return true;
        }

        public bool ChangePassword(User userDetails)
        {
            using SqlConnection connection = new(_connectionString);
            string query = @"update tbl_Users set Password = @pass where Email = @email";

            Dictionary<string, object> param = new()
            {
                { "@email", userDetails.Email },
                { "@pass", userDetails.PasswordHash }
            };
            DataTable dt = ReadData(query, CommandType.Text, connection, param);
            //if (dt != null && dt.Rows.Count > 0)
            //    return true;
            //return false;
            return true;
        }
    }
}
