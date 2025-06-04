using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Models.Complex
{
    public class UnitApplyComplex
    {
        public long pid { get; set; }
        public string subscribepoint { get; set; }

        public string type { get; set; }

        public string sub_name { get; set; }

        public int min_base_stock { get; set; }

        public string state { get; set; } = null!;

        public string unit { get; set; } = null!;

        public int apply_amount { get; set; }

        public DateTime create_time { get; set; }

        public int RemainingStock { get; set; }
    }
}
