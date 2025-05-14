using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMYSQL;

public partial class system_code
{
    public long pid { get; set; }

    public string code { get; set; } = null!;

    public string data { get; set; } = null!;

    public string description { get; set; } = null!;

    public string sub_description { get; set; } = null!;

    public int? weight { get; set; }
}
