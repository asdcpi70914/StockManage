using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMYSQL;

/// <summary>
/// 委外加工單
/// </summary>
public partial class projects_outsource
{
    public long pid { get; set; }

    public long projects_pid { get; set; }

    /// <summary>
    /// 委外加工單編號
    /// </summary>
    public string outsource_no { get; set; } = null!;

    /// <summary>
    /// 類別
    /// </summary>
    public string type_name { get; set; } = null!;

    /// <summary>
    /// 發包時間
    /// </summary>
    public DateTime outsource_build_time { get; set; }

    /// <summary>
    /// 委外回件時間
    /// </summary>
    public DateTime? outsource_back_time { get; set; }

    /// <summary>
    /// 建立人員
    /// </summary>
    public Guid creator { get; set; }

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime create_time { get; set; }

    /// <summary>
    /// 發包備註
    /// </summary>
    public string bulid_time_remark { get; set; } = null!;

    /// <summary>
    /// 回件備註
    /// </summary>
    public string back_time_remark { get; set; } = null!;

    /// <summary>
    /// 完成時間
    /// </summary>
    public DateTime? finish_time { get; set; }

    /// <summary>
    /// 審核狀態
    /// </summary>
    public string? state { get; set; }

    /// <summary>
    /// 製程pid(來源：製程維護功能)
    /// </summary>
    public long? process { get; set; }

    /// <summary>
    /// 廠商pid(來源：廠商維護功能)
    /// </summary>
    public long? firm { get; set; }

    /// <summary>
    /// 預計送件時間
    /// </summary>
    public DateTime? send_time { get; set; }

    /// <summary>
    /// 圖號
    /// </summary>
    public string? picture_number { get; set; }

    /// <summary>
    /// 零件總數
    /// </summary>
    public int? total_component { get; set; }

    /// <summary>
    /// 表單備註
    /// </summary>
    public string? form_remark { get; set; }

    public virtual ICollection<projects_outsource_relation> projects_outsource_relations { get; } = new List<projects_outsource_relation>();
}
