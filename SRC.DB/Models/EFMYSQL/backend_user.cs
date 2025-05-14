using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMYSQL;

public partial class backend_user
{
    public long pid { get; set; }

    public Guid user_id { get; set; }

    public string account { get; set; } = null!;

    public string? ad_account { get; set; }

    public string name_ch { get; set; } = null!;

    public string? name_en { get; set; }

    public string? email { get; set; }

    public string password_hash { get; set; } = null!;

    public string? phone_number { get; set; }

    public DateTime? lockout_end { get; set; }

    public sbyte enabled { get; set; }

    public int? access_failed_count { get; set; }

    public DateTime? changed_password_time { get; set; }

    public DateTime create_time { get; set; }

    public string creator { get; set; } = null!;

    public DateTime? edit_time { get; set; }

    public string? editor { get; set; }

    public string? unit { get; set; }

    public sbyte first_login { get; set; }

    public DateTime? apply_date { get; set; }

    public string? verification_code { get; set; }

    public DateTime? first_login_time { get; set; }

    public string? jwt_code { get; set; }

    public string? device_code { get; set; }

    public sbyte? email_confirmed { get; set; }

    public DateTime? email_confirmed_time { get; set; }

    public DateTime? limit_time { get; set; }

    public sbyte? state { get; set; }

    /// <summary>
    /// 負責人
    /// </summary>
    public bool person_in_charge { get; set; }

    public virtual ICollection<backend_dept> backend_depts { get; } = new List<backend_dept>();

    public virtual ICollection<backend_users_role> backend_users_roles { get; } = new List<backend_users_role>();
}
