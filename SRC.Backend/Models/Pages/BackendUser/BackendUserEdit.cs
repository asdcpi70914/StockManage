using SRC.Backend.Models.System;

namespace SRC.Backend.Models.Pages.BackendUser
{
    public class BackendUserEdit
    {
        public ActionMode Action { get; set; }
        public long? UserId { get; set; }
        public string Account { get; set; }
        public bool ad_accountchk { get; set; }
        public string AD_Account { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public string Unit { get; set; }
        public bool Enabled { get; set; }
        public int AccessFailedCount { get; set; }
        public Guid? Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public Guid? Editor { get; set; }
        public DateTime? EditTime { get; set; }
        public long? superior { get; set; }
        public bool person_in_charge { get; set; }
    }
}
