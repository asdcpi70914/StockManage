using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMSSQL;

public partial class stockin_log
{
    public long pid { get; set; }

    public long equipment_pid { get; set; }

    public int old_stock { get; set; }

    public int new_stock { get; set; }

    public DateTime create_time { get; set; }

    public string creator { get; set; } = null!;
}
