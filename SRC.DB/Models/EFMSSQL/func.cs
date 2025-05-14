using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMSSQL;

/// <summary>
/// 功能清單
/// </summary>
public partial class func
{
    public int pid { get; set; }

    public string? name { get; set; }

    public string? url { get; set; }

    public int? parentid { get; set; }

    public string? editor { get; set; }

    public DateTime? edit_time { get; set; }

    public string? type { get; set; }

    public DateTime create_time { get; set; }

    public string? creator { get; set; }

    public string? icon { get; set; }

    public short weight { get; set; }

    public short state { get; set; }

    public string? memo { get; set; }
}
