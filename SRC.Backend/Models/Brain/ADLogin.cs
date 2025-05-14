using Novell.Directory.Ldap;
using SRC.Backend.Models.Config;
using System.DirectoryServices;

namespace SRC.Backend.Models.Brain
{
    public class ADLogin
    {
        public Serilog.ILogger Logger { get; set; }

        public SysAppsetting sysAppsetting { get; set; }

        public ADLogin(Serilog.ILogger logger, SysAppsetting Setting)
        {
            Logger = logger;
            sysAppsetting = Setting;
        }


        public bool GetADLoginAuth(string Account, string Password)
        {
            try
            {
                using (var connection = new LdapConnection { SecureSocketLayer = sysAppsetting.IsSSL })
                {
                    connection.Connect(sysAppsetting.ADServer, sysAppsetting.ADPort);
                    connection.Bind(Account, Password);
                    if (connection.Bound)
                    {
                        connection.Disconnect();
                        return true;
                    }
                    else
                    {
                        connection.Disconnect();
                        return false;
                    }
                }

                //DirectoryEntry entry = new DirectoryEntry($"LDAP://{sysAppsetting.ADServer}", Account, Password, AuthenticationTypes.Secure);
                //return entry;
            }
            catch (Exception ex)
            {

                Logger.Fatal(ex, $"取得AD登入授權發生異常：{ex.Message}");

                return false;
            }
        }

        //public bool GetDirectoryEntryByAccount(string sAMAccountName)
        //{
        //    DirectoryEntry de = GetDirectoryEntryObject();
        //    DirectorySearcher deSearch = new DirectorySearcher(de);
        //    deSearch.Filter = $"(&(objectCategory=user)(sAMAccountName={sAMAccountName}))";
        //    deSearch.SearchScope = SearchScope.Subtree;
        //    try
        //    {
        //        SearchResult? result = deSearch.FindOne();
        //        if (result != null)
        //        {
        //            var accountExpires = result.Properties["accountExpires"][0].ToString();

        //            //如果是0或者是最大時間，表示該帳號沒有到期時間
        //            if(accountExpires == "0" || accountExpires == DateTime.MaxValue.ToString())
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                if(DateTime.TryParse(accountExpires,out DateTime Expires))
        //                {

        //                    if(Expires > DateTime.Now)
        //                    {
        //                        return true;
        //                    }
        //                    else
        //                    {
        //                        return false;
        //                    }
        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            }                   
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.Fatal(ex, $"驗證AD登入資料發生異常，{ex.Message}");
        //        return false;
        //    }
        //}
    }
}
