namespace SRC.Backend.Models.Pages.BackendUser
{
    public class BackendUserIndex
    {

        public class SearchModel
        {
            public string Account { get; set; }
            public string Email { get; set; }
            public string UserName { get; set; }
            public string PhoneNumber { get; set; }
            public bool? Enabled { get; set; }
        }

        public BackendUserSearch SearchResultPage { get; set; }
    }
}
