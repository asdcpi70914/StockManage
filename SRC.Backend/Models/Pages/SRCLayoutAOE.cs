using SRC.Backend.Models.System;

namespace SRC.Backend.Models.Pages
{
    public class SRCLayoutAOE
    {
        //public ActionMode Mode { get; set; }
        public bool IsDivPage { get; set; }

        public bool NoSaveButton { get; set; }
        public bool StartButton { get; set; }
        public bool NoCancelButton { get; set; }
        public bool HasCloseButton { get; set; }

        public string formId { get; set; }
        public string View { get; set; }
        public string LeftBtnView { get; set; } = "~/Views/Shared/Project/custom_button_left_view.cshtml";
        public string RightBtnView { get; set; } = "~/Views/Shared/Project/custom_button_right_view.cshtml";
        public string Action { get; set; }
        public string Controller { get; set; }

        public string Title { get; set; }

        public string Enctype { get; set; }

        public object AOEObject { get; set; }

        public ModalSize ViewSize { get; set; }

        public string Height { get; set; }

        public string SaveButtonDesc { get; set; } = "儲存";

        public string ResultTargetDivID { get; set; }

        public bool NeedOverflow { get; set; } = false;
        public bool NeedFinishButton { get; set; } = false;
        public bool NeedCancelButton { get; set; } = false;
        public bool NeedStopButton { get; set; } = false;
        public bool NeedModifyButton { get; set; } = false;
        public string StopBtnDesc { get; set; } = "專案暫停";
        public string SaveBtnColor { get; set; } = "btn-success";
    }

}
