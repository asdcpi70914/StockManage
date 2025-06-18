using SRC.Backend.Models.Pages.BackendUser;
using SRC.Backend.Models.System;

namespace SRC.Backend.Models.Brain
{
    public class BackendUserDataCheck
    {
        public string InnerMessage { get; protected set; }

        public bool CheckCreateBackendUser(BackendUserEdit model)
        {
            string chkMsg = string.Empty;
            if (string.IsNullOrWhiteSpace(model.Account))
            {
                chkMsg += $"{UIMessage.SYS.AccountEmpty},";
            }

            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                chkMsg += $"{UIMessage.SYS.AccountNameEmpty},";
            }

            if (string.IsNullOrWhiteSpace(model.Password) && model.Action == ActionMode.ADD)
            {
                chkMsg += $"{UIMessage.SYS.PasswordEmpty},";
            }

            if (model.ad_accountchk)
            {
                if (string.IsNullOrWhiteSpace(model.AD_Account))
                {
                    chkMsg += "請輸入AD連動帳號,";
                }
            }

            //if (string.IsNullOrWhiteSpace(model.Unit))
            //{
            //    chkMsg += $"{UIMessage.SYS.UnitEmpty},";
            //}

            InnerMessage = chkMsg;
            return chkMsg == string.Empty;

        }

        public bool CheckEditBackendUser(BackendUserEdit model)
        {
            string chkMsg = string.Empty;
            if (string.IsNullOrWhiteSpace(model.Account))
            {
                chkMsg += $"{UIMessage.SYS.AccountEmpty},";
            }

            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                chkMsg += $"{UIMessage.SYS.AccountNameEmpty},";
            }

            InnerMessage = chkMsg;
            return chkMsg == string.Empty;

        }


        public bool CheckResetPasswordData(ResetPassword data)
        {
            string chkMsg = string.Empty;

            if (string.IsNullOrWhiteSpace(data.Password))
            {
                chkMsg += $"{UIMessage.SYS.PasswordEmpty},";
            }

            if (string.IsNullOrWhiteSpace(data.ConfirmPassword))
            {
                chkMsg += $"{UIMessage.SYS.ConfirmPasswordEmpty},";
            }

            if (data.Password != data.ConfirmPassword)
            {
                chkMsg += $"{UIMessage.SYS.PasswordNoMatch},";
            }

            InnerMessage = chkMsg;
            return chkMsg == string.Empty;

        }
    }
}
