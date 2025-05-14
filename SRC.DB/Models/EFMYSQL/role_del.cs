using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMYSQL;

public partial class role_del
{
    public int pid { get; set; }

    public int role_pid { get; set; }

    public string? name { get; set; }

    public string? editor { get; set; }

    public DateTime? edit_time { get; set; }

    public DateTime create_time { get; set; }

    public string creator { get; set; } = null!;

    public int state { get; set; }

    public string? programe_code { get; set; }
}
