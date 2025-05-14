using System.Collections.Generic;

namespace SRC.Backend.Models.System
{
    public class MenusAuth
    {
        public List<string> Urls { get; set; }

        public bool FirstLogin { get; set; }
    }
}
