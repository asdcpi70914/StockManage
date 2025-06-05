using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Models.Complex
{
    public class PurchaseStockInComplex
    {
        public string type { get; set; }
        public long pid { get; set; }
        public int stock { get; set; }
        public DateTime create_time { get; set; }
    }
}
