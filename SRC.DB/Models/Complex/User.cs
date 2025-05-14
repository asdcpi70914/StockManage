using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Models.Complex
{
    public class User
    {
        public Guid? UserId { get; set; }
        public string Account { get; set; }
        public string PasswordHash { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        //public bool EmailConfirmed { get; set; }
        //public DateTime? LockoutEnd { get; set; }
        public bool? Enabled { get; set; }
        public int AccessFailedCount { get; set; }
        public Guid? Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public Guid? Editor { get; set; }
        public DateTime? EditTime { get; set; }
    }
}
