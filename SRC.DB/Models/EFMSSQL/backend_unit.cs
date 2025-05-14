using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMSSQL;

public partial class backend_unit
{
    public long pid { get; set; }

    public string name { get; set; } = null!;

    public string code { get; set; } = null!;
}
