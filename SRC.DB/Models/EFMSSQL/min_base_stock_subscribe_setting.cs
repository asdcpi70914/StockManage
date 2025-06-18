using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMSSQL;

public partial class min_base_stock_subscribe_setting
{
    public long pid { get; set; }

    public long subscribepoint_pid { get; set; }

    public string type { get; set; } = null!;

    /// <summary>
    /// 器材或是裝備pid
    /// </summary>
    public long sub_pid { get; set; }

    public int min_base_stock { get; set; }

    public int stock { get; set; }

    public DateTime create_time { get; set; }

    public string creator { get; set; } = null!;

    public DateTime? edit_time { get; set; }

    public string? editor { get; set; }
}
