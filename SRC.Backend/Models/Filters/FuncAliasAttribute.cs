using System;

namespace SRC.Backend.Models.Filters
{
    public class FuncAliasAttribute : Attribute
    {
        public string AliasName { get; set; }

        public string ControllerName { get; set; }
    }
}
