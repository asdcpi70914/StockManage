using System.ComponentModel.DataAnnotations;
using System;
using SRC.Backend.Models.System;

namespace SRC.Backend.Models.Pages.BackendUser
{
    public class ChangePasswordModel
    {
        public Guid UserId { get; set; }
        public string Account { get; set; }

        [DataType(DataType.Password)]
        [StringLength(40)]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [StringLength(40)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [StringLength(40)]
        public string ConfirmPassword { get; set; }
        public Guid? Editor { get; set; }
        public DateTime? EditTime { get; set; }

        public string ResultMessage { get; set; }

        public string ErrorMessage { get; set; }

        public SRCPageMessage ResultView { get; set; }
    }
}
