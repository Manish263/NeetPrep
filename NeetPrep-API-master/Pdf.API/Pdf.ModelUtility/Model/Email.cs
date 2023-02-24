using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdf.ModelUtility.Model
{
    public class Email
    {
        public string ReceiverAddress { get; set; }
        public string ReceiverName { get; set; } = "";
        public string Subject { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
