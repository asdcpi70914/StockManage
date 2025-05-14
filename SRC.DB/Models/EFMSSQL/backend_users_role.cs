using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMSSQL;

/// <summary>
/// 角色權限
/// </summary>
public partial class backend_users_role
{
    public int pid { get; set; }

    public Guid user_id { get; set; }

    public int role_id { get; set; }

    public virtual role role { get; set; } = null!;

    public virtual backend_user user { get; set; } = null!;
}
