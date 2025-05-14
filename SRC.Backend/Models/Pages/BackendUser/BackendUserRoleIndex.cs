using SRC.Backend.Models.System;
using SRC.Backend.Models.Brain;
using SRC.DB.Interfaces.Users;
using SRC.DB.Models.EFMSSQL;

namespace SRC.Backend.Models.Pages.BackendUser
{
    public class BackendUserRoleIndex
    {
        public class SearchModel
        {
            public string Account { get; set; }
            public string Email { get; set; }
            public string UserName { get; set; }

            public BackendUserRoleSearch GetSearchViewModel(Serilog.ILogger sLog, IDF_BackendUser backendUserDF, UserInfoSecurity security, int? page, int? take, out string innerMessage)
            {
                innerMessage = string.Empty;

                BackendUserRoleSearch result = new BackendUserRoleSearch()
                {
                    Users = new List<BackendUserRoleSearch.SearchView>()
                };

                try
                {
                    List<backend_user> users = backendUserDF.SearchRoleInUser(Account, UserName, Email, page, take, out int rowCount);

                    foreach (var each in users)
                    {
                        result.Users.Add(new BackendUserRoleSearch.SearchView()
                        {
                            UserID = each.user_id,
                            Account = each.account,
                            Email = each.email,
                            UserName = each.name_ch,
                            RoleNameList = each.backend_users_roles.GroupBy(m => m.role_id).Select(m => m.First().role.name).ToList()
                        });
                    }

                    result.Pagination = new SRCUIPagination(page, take, rowCount);

                    return result;
                }
                catch (Exception ex)
                {
                    sLog.Fatal(ex, $"查詢帳號所屬角色發生異常,{ex.Message}");
                    innerMessage = ex.Message;
                }

                return null;
            }
        }


        public BackendUserRoleSearch SearchResultPage { get; set; }
    }
}
