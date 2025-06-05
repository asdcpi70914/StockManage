using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Models.Complex
{
    public class PayTreasuryComplex
    {
        public long pid { get; set; }
        public string name { get; set; }
        public int apply_amount { get; set; }
        public int already_pay_amount { get; set; }
        public string apply_name { get; set; }
        public int stock { get; set; }
        public DateTime apply_time { get; set; }
        public string unit { get; set; }
    }
}
