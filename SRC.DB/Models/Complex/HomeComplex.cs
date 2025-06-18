using SRC.DB.Models.EFMSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Models.Complex
{
    public class HomeComplex
    {
        public class HomeUpData
        {
            public string equipment_name { get; set; }
            public string subscribepoint_name { get; set; }
            public int stock { get; set; }
            public int min_base_stock { get; set; }
            public string type { get; set; }
        }


        public class HomeMidData 
        {
            public string equipment_name { get; set; }
            public string subscribepoint_name { get; set; }
            public int stock { get; set; }
            public decimal price { get; set; }
            public string type { get; set; }
        }

        public class HomeDownData
        {
            public string unit { get; set; }
            public decimal amount { get; set; }
        }
    }
}
