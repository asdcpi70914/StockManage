using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMYSQL;

public partial class projects_assign_log
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
    /// 指派人員pid
    /// </summary>
    public long? user_pid { get; set; }

    /// <summary>
    /// 專案負責人pid
    /// </summary>
    public long? pm_pid { get; set; }

    /// <summary>
    /// 原型加工部負責人pid
    /// </summary>
    public long? prototypework_pid { get; set; }

    /// <summary>
    /// 工藝研發部負責人pid
    /// </summary>
    public long? craftrd_pid { get; set; }

    /// <summary>
    /// 專案狀態
    /// </summary>
    public string project_type { get; set; } = null!;

    /// <summary>
    /// 指派時間
    /// </summary>
    public DateTime create_time { get; set; }
}
