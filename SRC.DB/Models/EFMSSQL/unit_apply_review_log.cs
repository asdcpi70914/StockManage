using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMSSQL;

public partial class unit_apply_review_log
{
    public long pid { get; set; }

    public long unit_apply_pid { get; set; }

    public string ori_state { get; set; } = null!;

    public string new_state { get; set; } = null!;

    public string memo { get; set; } = null!;

    public DateTime create_time { get; set; }

    public string creator { get; set; } = null!;

    public virtual unit_apply unit_apply_p { get; set; } = null!;
}
