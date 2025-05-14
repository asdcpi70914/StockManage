using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMYSQL;

/// <summary>
/// 專案追蹤表相關文件
/// </summary>
public partial class projects_file
{
    /// <summary>
    /// 流水號
    /// </summary>
    public long pid { get; set; }

    /// <summary>
    /// Projects的pid
    /// </summary>
    public long projects_pid { get; set; }

    /// <summary>
    /// 檔案名稱
    /// </summary>
    public string filename { get; set; } = null!;

    /// <summary>
    /// 可用項目包含projects的state欄位(就是專案狀態)
    /// </summary>
    public string a_type { get; set; } = null!;

    /// <summary>
    /// 從哪個功能上傳的
    /// </summary>
    public string upload_action { get; set; } = null!;

    /// <summary>
    /// 儲存位置
    /// </summary>
    public string a_path { get; set; } = null!;

    /// <summary>
    /// 上傳人員pid
    /// </summary>
    public long? upload_user { get; set; }

    /// <summary>
    /// 上傳時間
    /// </summary>
    public DateTime? create_time { get; set; }

    public virtual project projects_p { get; set; } = null!;
}
