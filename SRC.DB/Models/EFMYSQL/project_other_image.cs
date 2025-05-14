using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMYSQL;

public partial class project_other_image
{
    public long pid { get; set; }

    /// <summary>
    /// 專案pid
    /// </summary>
    public long project_pid { get; set; }

    /// <summary>
    /// 原始照片名稱
    /// </summary>
    public string ori_name { get; set; } = null!;

    /// <summary>
    /// 照片儲存路徑
    /// </summary>
    public string file_path { get; set; } = null!;

    /// <summary>
    /// 取代文字
    /// </summary>
    public string replace_string { get; set; } = null!;

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime create_time { get; set; }

    public virtual project project_p { get; set; } = null!;
}
