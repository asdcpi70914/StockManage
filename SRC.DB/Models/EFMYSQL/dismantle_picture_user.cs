using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMYSQL;

public partial class dismantle_picture_user
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
    /// 拆圖人員
    /// </summary>
    public long user_pid { get; set; }

    public DateTime create_time { get; set; }

    public string creator { get; set; } = null!;
}
