using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMYSQL;

public partial class project_state_log
{
    /// <summary>
    /// 流水號
    /// </summary>
    public long pid { get; set; }

    /// <summary>
    /// 專案pid
    /// </summary>
    public long project_pid { get; set; }

    /// <summary>
    /// 變更前狀態
    /// </summary>
    public string ori_state { get; set; } = null!;

    /// <summary>
    /// 變更後狀態
    /// </summary>
    public string new_state { get; set; } = null!;

    /// <summary>
    /// 建立人員
    /// </summary>
    public Guid creator { get; set; }

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime create_time { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string? memo { get; set; }
}
