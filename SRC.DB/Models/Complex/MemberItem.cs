namespace SRC.DB.Models.Complex
{
    public class MemberItem
    {
        public long pid { get; set; }
        public Guid user_id { get; set; }
        public string hz_id { get; set; }
        public string account { get; set; }
        public string name_ch { get; set; }
        public bool IsSelected { get; set; }
    }
}
