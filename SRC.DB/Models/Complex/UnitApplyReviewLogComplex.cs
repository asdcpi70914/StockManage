using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Models.Complex
{
    public class UnitApplyReviewLogComplex
    {
        public string sub_name { get; set; }
        public string review_name { get; set; }
        public DateTime review_time { get; set; }
        public string ori_state { get; set; }
        public string new_state {  get; set; }
        public string memo { get; set; }
    }
}
