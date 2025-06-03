using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.HardCodes
{
    public class REVIEW_STATE : ISRCState<REVIEW_STATE.STATE>
    {
        public enum STATE
        {
            INIT = 0,
            REVIEW_OK = 1,
            CANCEL = 3
        }

        public const string DESC_INIT = "申請中";
        public const string DESC_REVIEW_OK = "審核通過";
        public const string DESC_CANCEL = "撤銷";

        public STATE Code { get; set; }
        public string Desc { get; set; }

        public REVIEW_STATE(STATE code)
        {
            Init(code);
        }

        public REVIEW_STATE(string state)
        {
            Init(state);
        }

        public REVIEW_STATE()
        {

        }

        private void Init(STATE code)
        {
            Code = code;
            switch (Code)
            {
                case STATE.INIT:
                    Desc = DESC_INIT;
                    break;
                case STATE.REVIEW_OK:
                    Desc = DESC_REVIEW_OK;
                    break;
                case STATE.CANCEL:
                    Desc = DESC_CANCEL;
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void Init(string state)
        {
            STATE tempState;
            if (!Enum.TryParse(state, out tempState))
            {
                throw new ArgumentException();
            }

            Code = tempState;

        }

        public override string GetDesc(STATE state)
        {
            return new REVIEW_STATE(state).Desc;
        }
    }

}
