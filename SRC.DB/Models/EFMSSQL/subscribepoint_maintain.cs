using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMSSQL;

public partial class subscribepoint_maintain
{
    public long pid { get; set; }

    public string name { get; set; } = null!;

    public DateTime create_time { get; set; }

    public string creator { get; set; } = null!;

    public string? editor { get; set; }

    public DateTime? edit_time { get; set; }
}
