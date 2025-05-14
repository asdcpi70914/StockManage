using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMYSQL;

/// <summary>
/// 紀錄委外加工單修改的歷程
/// </summary>
public partial class projects_outsource_log
{
    public long pid { get; set; }

    /// <summary>
    /// 委外加工單PID
    /// </summary>
    public long projects_outsource_pid { get; set; }

    /// <summary>
    /// 異動人員
    /// </summary>
    public Guid editor { get; set; }

    /// <summary>
    /// 異動時間
    /// </summary>
    public DateTime edit_time { get; set; }

    /// <summary>
    /// 異動前的發包時間
    /// </summary>
    public DateTime ori_build_time { get; set; }

    /// <summary>
    /// 異動前的回件時間
    /// </summary>
    public DateTime? ori_back_time { get; set; }
}
