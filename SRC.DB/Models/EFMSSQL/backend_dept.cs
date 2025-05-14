using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMSSQL;

public partial class backend_dept
{
    public long pid { get; set; }

    public long backend_user_pid { get; set; }

    public long? parent_pid { get; set; }

    public string code { get; set; } = null!;

    public DateTime create_time { get; set; }

    public virtual backend_user backend_user_p { get; set; } = null!;
}
