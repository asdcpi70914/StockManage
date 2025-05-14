using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMYSQL;

public partial class role_func
{
    public int pid { get; set; }

    public int func_id { get; set; }

    public int role_id { get; set; }

    public short weight { get; set; }
}
