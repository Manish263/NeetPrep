using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdf.CommonUtility.CommonFunctions
{
    public class CommonFunctions
    {
        public static string FncCheckEmpty(dynamic value)
        {
            return (object)value == DBNull.Value ? string.Empty : value.ToString();
        }
        public static dynamic FncCheckForeignString(dynamic data)
        {
            if (data == null) return DBNull.Value;
            object obj = (object)data;
            if (obj == null || string.IsNullOrWhiteSpace(Convert.ToString(obj))) return DBNull.Value;
            return obj.ToString();
        }
    }
}
