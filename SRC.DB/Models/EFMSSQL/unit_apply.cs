using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMSSQL;

public partial class unit_apply
{
    public long pid { get; set; }

    public long setting_pid { get; set; }

    public int apply_amount { get; set; }

    public string unit { get; set; } = null!;

    public DateTime create_time { get; set; }

    public string creator { get; set; } = null!;

    public DateTime? edit_time { get; set; }

    public string? editor { get; set; }

    public string state { get; set; } = null!;

    /// <summary>
    /// 繳庫數量
    /// </summary>
    public int? pay_treasury { get; set; }

    public virtual ICollection<unit_apply_review_log> unit_apply_review_logs { get; } = new List<unit_apply_review_log>();
}
