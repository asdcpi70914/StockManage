using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMYSQL;

/// <summary>
/// 製程維護表
/// </summary>
public partial class process_maintain
{
    /// <summary>
    /// 流水號
    /// </summary>
    public long pid { get; set; }

    /// <summary>
    /// 製程名稱
    /// </summary>
    public string process_name { get; set; } = null!;

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime create_time { get; set; }

    /// <summary>
    /// 建立人員
    /// </summary>
    public string creator { get; set; } = null!;

    /// <summary>
    /// 修改時間
    /// </summary>
    public DateTime? edit_time { get; set; }

    /// <summary>
    /// 修改人員
    /// </summary>
    public string? editor { get; set; }
}
