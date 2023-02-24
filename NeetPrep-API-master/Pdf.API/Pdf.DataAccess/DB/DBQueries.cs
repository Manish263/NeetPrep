using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdf.DataAccess.DB
{
    public class DBQueries
    {
        public static DataTable ReadData(string query, CommandType type, SqlConnection connection, Dictionary<string, object>? paramDict = null)
        {
            try
            {
                using SqlDataAdapter sqlDataAdapter = new(query, connection);
                sqlDataAdapter.SelectCommand.CommandType = type;
                sqlDataAdapter.SelectCommand.CommandTimeout = 120;
                if (paramDict != null)
                    foreach (KeyValuePair<string, object> param in paramDict)
                        sqlDataAdapter.SelectCommand.Parameters.AddWithValue(param.Key, param.Value);
                DataTable dt = new();
                sqlDataAdapter.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
