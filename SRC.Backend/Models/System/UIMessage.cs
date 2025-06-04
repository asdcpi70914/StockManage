namespace SRC.Backend.Models.System
{
    public class UIMessage
    {
        public class TITLE
        {
            public const string DisplayAlter = "系統訊息";
        }

        public class BUTTON
        {
            public const string Confirm = "確定";
        }

        public class SYS
        {
            public const string EditArgumentLess = "編輯回傳參數異常,暫時無法處理此筆資料";
            public const string ServerBusy = "系統忙碌，請稍後再試！";
            public const string ServerError = "系統異常！";
            public const string ServerNoR = "無法取得API回應資料";
            public const string LoginAgain = "請重新登入";
            public const string UpdateOK = "更新成功";
            public const string UpdateFail = "更新失敗";
            public const string LogingAgain = "請重新登入";
            public const string AccountExist = "帳號已存在";
            public const string AccountEmpty = "帳號不可空白";
            public const string AccountNameEmpty = "帳號名稱不可空白";
            public const string MemberNameEmpty = "會員名稱不可空白";

            public const string PasswordEmpty = "密碼欄位不可空白";
            public const string WordLengthOver = "字數超過限制";
            public const string ConfirmPasswordEmpty = "確認密碼欄位不可空白";
            public const string PasswordNoMatch = "密碼不相符";
            public const string UnitEmpty = "請選擇單位";
            public const string NoDataToProcess = "查無資料無法處理";
            public const string ArgumentEmpty = "參數錯誤";

            public const string SmallerOne = "數量不可小於1";

            public const string TitleEmpty = "標題欄位不可空白";
            public const string DescriptionEmpty = "描述欄位不可空白";


            public const string Fail_Edit = "修改失敗";
            public const string Fail_Add = "新增失敗";
            public const string Fail_Delete = "刪除失敗";
            public const string Fail_Cancel = "撤銷失敗";
            public const string Fail_Assign = "指派失敗";
            public const string Fail_Review = "審核失敗";
            public const string Fail_Upload = "上傳失敗";
            public const string Fail_Stop = "暫停失敗";
            public const string Fail_Start = "啟動失敗";
            public const string Fail_Abandon = "作廢失敗";
            public const string Fail_Distribute = "撥發失敗";

            public const string Success_Add = "新增成功";
            public const string Success_Edit = "修改成功";
            public const string Success_Delete = "刪除成功";
            public const string Success_Abandon = "作廢成功";
            public const string Success_Finish = "結案成功";
            public const string Success_Delivery = "出貨成功";
            public const string Success_Cancel = "撤銷成功";
            public const string Success_Stop = "暫停成功";
            public const string Success_Start = "啟動成功";
            public const string Success_Generate = "建立成功";
            public const string Success_Review = "審核成功";
            public const string Success_Assign = "指派成功";
            public const string Success_Upload = "上傳成功";
            public const string Success_Complete = "專案已完工";
            public const string Success_Distribute = "撥發成功";
            public static readonly string Fail_Search = "查詢資料出現異常";

            public const string AccountNotExist = "帳號不存在";
            public const string ProjectOutsource_Review_Message = "此委外單將進入專案工程課審核流程";

            public static readonly string SystemFail = "系統錯誤，請聯絡系統管理員";
            public static readonly string Fail_ResponseQuestion = "回覆失敗";
            public static readonly string Fail_ChangeState = "狀態變更失敗";
            public static readonly string Fail_NeedParameters = "必要參數不足";


            public static readonly string Msg_NoReply = "尚未回覆";
            public static readonly string Msg_NoBackQuestion = "目前無後送案件可領取";

            public static readonly string OK_Edit = "修改完成";
        }
    }
}
