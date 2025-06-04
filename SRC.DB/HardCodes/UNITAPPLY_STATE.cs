using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.HardCodes
{
    public class UNITAPPLY_STATE : ISRCState<UNITAPPLY_STATE.STATE>
    {
        public enum STATE
        {
            INIT = 0,
            REVIEW_OK = 1,
            REVIEW_FAIL = 2,
            DISTRIBUTE = 4,
            DISTRIBUTE_OK = 5,
            DELIVERY = 6,
            CANCEL = 99,
            SUPPLYROOM_CANCEL = 999
        }

        public const string DESC_INIT = "申請中";
        public const string DESC_REVIEW_OK = "審核通過";
        public const string DESC_REVIEW_FAIL = "審核退回";
        public const string DESC_DISTRIBUTE = "撥發中";
        public const string DESC_DISTRIBUTE_OK = "已撥發";
        public const string DESC_DELIVERY = "出貨";
        public const string DESC_FINIFSH = "撥發中";
        public const string DESC_CANCEL = "撤銷";
        public const string DESC_SUPPLYROOM_CANCEL = "供應室撤銷";

        public STATE Code { get; set; }
        public string Desc { get; set; }

        public UNITAPPLY_STATE(STATE code)
        {
            Init(code);
            InitDesc();
        }

        public UNITAPPLY_STATE(string state)
        {
            Init(state);
            InitDesc();
        }

        public UNITAPPLY_STATE()
        {

        }

        private void InitDesc()
        {
            switch (Code)
            {
                case STATE.INIT:
                    Desc = DESC_INIT;
                    break;
                case STATE.REVIEW_OK:
                    Desc = DESC_REVIEW_OK;
                    break;
                case STATE.REVIEW_FAIL:
                    Desc = DESC_REVIEW_FAIL;
                    break;
                case STATE.DISTRIBUTE:
                    Desc = DESC_DISTRIBUTE;
                    break;
                case STATE.DISTRIBUTE_OK:
                    Desc = DESC_DISTRIBUTE_OK;
                    break;
                case STATE.DELIVERY:
                    Desc = DESC_DELIVERY;
                    break;
                case STATE.SUPPLYROOM_CANCEL:
                    Desc = DESC_SUPPLYROOM_CANCEL;
                    break;
                case STATE.CANCEL:
                    Desc = DESC_CANCEL;
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void Init(STATE state)
        {
            Code = state;
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
            return new UNITAPPLY_STATE(state).Desc;
        }
    }

}
