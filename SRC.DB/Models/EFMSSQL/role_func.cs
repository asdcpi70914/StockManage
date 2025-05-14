using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMSSQL;

/// <summary>
/// 功能權限
/// </summary>
public partial class role_func
{
    public int pid { get; set; }

    public int func_id { get; set; }

    public int role_id { get; set; }

    public short weight { get; set; }
}
