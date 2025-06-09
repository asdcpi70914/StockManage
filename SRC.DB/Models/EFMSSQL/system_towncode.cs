using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMSSQL;

public partial class system_towncode
{
    public string CountryCode { get; set; } = null!;

    public string TownCode { get; set; } = null!;

    public string TownName { get; set; } = null!;

    public string postalcode { get; set; } = null!;
}
