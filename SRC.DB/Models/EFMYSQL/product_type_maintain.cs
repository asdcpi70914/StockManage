using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMYSQL;

public partial class product_type_maintain
{
    /// <summary>
    /// 流水號
    /// </summary>
    public long pid { get; set; }

    /// <summary>
    /// 產品類別名稱
    /// </summary>
    public string product_type_name { get; set; } = null!;

    /// <summary>
    /// 建立人員
    /// </summary>
    public Guid creator { get; set; }

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime create_time { get; set; }

    /// <summary>
    /// 修改人員
    /// </summary>
    public Guid? editor { get; set; }

    /// <summary>
    /// 修改時間
    /// </summary>
    public DateTime? edit_time { get; set; }
}
