using System.Collections.Generic;
using System;

namespace SRC.Backend.Models.System
{
    public class SRCLoginMeta
    {
        public Guid? UserId { get; set; }
        public string Account { get; set; }
        public string UserName { get; set; }
        public string Unit { get; set; }
        public List<string> RoleCode { get; set; }
    }
}
