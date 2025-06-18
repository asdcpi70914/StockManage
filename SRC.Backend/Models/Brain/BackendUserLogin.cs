using SRC.DB.Interfaces.Settings;
using SRC.DB.Interfaces.Users;
using SRC.DB.Models.EFMSSQL;
using SRC.ST.Security;
using System.Drawing;
using System.Text;

namespace SRC.Backend.Models.Brain
{
    public class BackendUserLogin
    {
        private IDF_BackendUser backendUserDF { get; set; }

        public string InnerMessage { get; set; }

        public Serilog.ILogger _Logger { get; set; }

        public BackendUserLogin(IDF_BackendUser stvmuserDB, Serilog.ILogger logger)
        {
            backendUserDF = stvmuserDB;
            _Logger = logger;
        }

        public BackendUserLogin(Serilog.ILogger logger)
        {
            _Logger = logger;
        }


        public bool LoginFunc(string account, string password,string code,string vaildCode, IDF_SystemCode sysCodeDF)
        {

            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(password))
            {
                InnerMessage = "帳號密碼不可空白";
                return false;
            }

            if(code != vaildCode)
            {
                InnerMessage = "驗證碼錯誤，請重新確認!";
                return false;
            }

            try
            {
                string salt = sysCodeDF.GetBackendUserSalt();
                //string passwordHash = new Security().Encrypt(password);
                string passwordHash = new SecuritySalt(salt).Encrypt(password);

                if (!backendUserDF.GetUserByLoginPwd(account, passwordHash, out backend_user user))
                {
                    if (user != null)
                    {
                        int maxFail = 5;

                        //更新密碼錯誤次數

                        backendUserDF.IncrementAccountFailCount(account);

                        if (user.access_failed_count + 1 > maxFail)
                        {
                            //密碼錯誤次數達上限，鎖定帳號
                            backendUserDF.LockAccount(account);
                            InnerMessage = $"錯誤次數已達{maxFail}次，帳號已被鎖定！";
                            return false;
                        }
                    }

                    InnerMessage = "帳號錯誤或密碼錯誤";
                    return false;
                }

                if (!user.enabled)
                {
                    InnerMessage = "帳號停用中";
                    return false;
                }

                if (user.lockout_end.HasValue && user.lockout_end.Value > DateTime.Now)
                {
                    InnerMessage = "帳號已被鎖定";
                    return false;
                }

                InnerMessage = "登入成功";
                return true;

                //string resCode = user.ResCode;
                //if (resCode != ReMsg.Success)
                //{
                //    return (false, ReMsg.AccountFailMsg(user.Data == null ? 0 : user.Data.AccessFailedCount)[resCode], user.Data);
                //}
                //return (true, "登入成功", user.Data);
            }
            catch (Exception ex)
            {
                InnerMessage = "帳號登入發生異常";
                _Logger.Fatal(ex, $"帳號登入發生異常,{ex.Message}");

                return false;
            }
        }

        public string RandomCode(int length)
        {
            string s = "0123456789zxcvbnmasdfghjklqwertyuiop";
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();
            int index;
            for (int i = 0; i < length; i++)
            {
                index = rand.Next(0, s.Length);
                sb.Append(s[index]);
            }
            return sb.ToString();
        }

        public void PaintInterLine(Graphics g, int num, int width, int height)
        {
            Random r = new Random();
            int startX, startY, endX, endY;
            for (int i = 0; i < num; i++)
            {
                startX = r.Next(0, width);
                startY = r.Next(0, height);
                endX = r.Next(0, width);
                endY = r.Next(0, height);
                g.DrawLine(new Pen(Brushes.Red), startX, startY, endX, endY);
            }
        }

    }
}
