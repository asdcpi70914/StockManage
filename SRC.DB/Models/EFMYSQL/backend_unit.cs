using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMYSQL;

/// <summary>
/// 單位資料
/// </summary>
public partial class backend_unit
{
    /// <summary>
    /// 單位流水號
    /// </summary>
    public long pid { get; set; }

    /// <summary>
    /// 單位名稱
    /// </summary>
    public string name { get; set; } = null!;

    /// <summary>
    /// 單位編碼
    /// </summary>
    public string code { get; set; } = null!;
}
