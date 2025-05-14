namespace SRC.Backend.Models.Pages.BackendUser
{
    public class ResetPassword
    {
        public long Pid { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
