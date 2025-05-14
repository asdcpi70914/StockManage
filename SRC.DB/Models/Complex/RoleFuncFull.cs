using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SRC.DB.Models.Complex
{
    [NotMapped]
    public class RoleFuncFull
    {
        public int? func_id { get; set; }
        public int? role_id { get; set; }
        public string url { get; set; }
        public short? weight { get; set; }

        public string name { get; set; }
        public string type { get; set; }
        public int pid { get; set; }
        public string icon { get; set; }

        public int? parentId { get; set; }

        [JsonPropertyName("checked")]
        public bool isChecked { get; set; }


        public bool hasChild { get; set; }
        public bool hasParent { get; set; }

        public IList<RoleFuncFull> child { get; set; }

    }
}
