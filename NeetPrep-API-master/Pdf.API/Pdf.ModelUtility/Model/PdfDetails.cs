using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdf.ModelUtility.Model
{
    public class PdfDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Price { get; set; }
        public string PdfLink { get; set; }
        public bool IsAvailable { get; set; }
    }
}
