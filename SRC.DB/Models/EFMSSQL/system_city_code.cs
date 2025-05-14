using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMSSQL;

/// <summary>
/// 城市代碼
/// </summary>
public partial class system_city_code
{
    public string county_code { get; set; } = null!;

    public string county_code01 { get; set; } = null!;

    public string name { get; set; } = null!;
}
