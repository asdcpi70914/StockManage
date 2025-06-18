using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMSSQL;

public partial class equipment_maintain
{
    public long pid { get; set; }

    public string type { get; set; } = null!;

    public string name { get; set; } = null!;

    public decimal price { get; set; }

    public string creator { get; set; } = null!;

    public DateTime create_time { get; set; }

    public string? editor { get; set; }

    public DateTime? edit_time { get; set; }

    public string state { get; set; } = null!;
}
