using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMYSQL;

/// <summary>
/// 白身組裝完成時間與委外加工單關聯表
/// </summary>
public partial class projects_outsource_relation
{
    /// <summary>
    /// 流水號
    /// </summary>
    public long pid { get; set; }

    /// <summary>
    /// 專案追蹤表pid
    /// </summary>
    public long projects_pid { get; set; }

    /// <summary>
    /// 委外加工單pid
    /// </summary>
    public long? projects_outsource_pid { get; set; }

    /// <summary>
    /// 白身組裝完成時間
    /// </summary>
    public DateTime? build_time { get; set; }

    /// <summary>
    /// 委外加工單完成時間
    /// </summary>
    public DateTime? outsource_back_time { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string remark { get; set; } = null!;

    public virtual projects_outsource? projects_outsource_p { get; set; }
}
