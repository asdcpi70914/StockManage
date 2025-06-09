using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMSSQL;

public partial class factoryinfo_maintain
{
    public long pid { get; set; }

    public string name { get; set; } = null!;

    public string contact_phone { get; set; } = null!;

    public string company_number { get; set; } = null!;

    public string city { get; set; } = null!;

    public string town { get; set; } = null!;

    public string address { get; set; } = null!;

    public DateTime create_time { get; set; }

    public string creator { get; set; } = null!;

    public DateTime? edit_time { get; set; }

    public string? editor { get; set; }
}
