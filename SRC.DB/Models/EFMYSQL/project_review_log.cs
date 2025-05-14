using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMYSQL;

/// <summary>
/// 專案以及委外單審核歷程
/// </summary>
public partial class project_review_log
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
    /// 委外單pid(如果是白身組裝檢查中以及組裝品檢檢查中，則此欄位為NULL)
    /// </summary>
    public long? project_outsource_pid { get; set; }

    public string type { get; set; } = null!;

    /// <summary>
    /// 審核狀態(通過或退回)
    /// </summary>
    public string review_state { get; set; } = null!;

    /// <summary>
    /// 退回原因
    /// </summary>
    public string? reason { get; set; }

    /// <summary>
    /// 建立時間(審核時間)
    /// </summary>
    public DateTime create_time { get; set; }

    /// <summary>
    /// 建立人員(審核人員)
    /// </summary>
    public Guid creator { get; set; }
}
