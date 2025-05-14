using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.HardCodes
{
    public class ROLE_TYPE
    {
        public enum STATE
        {
            CUSTOMER_1 = 11,
            CUSTOMER_2 = 12,
            CUSTOMER_3 = 13,
            VSTAR = 2,
            ADMIN = 999
        }

        public const string DESC_CUSTOMER = "客服";
        public const string DESC_VSTAR = "前台使用者";
        public const string DESC_ADMIN = "後台使用者";

        public STATE Code { get; set; }
        public string Desc { get; set; }

        public ROLE_TYPE(STATE code)
        {
            Init(code);
        }

        public ROLE_TYPE(string state)
        {
            Init(state);
        }

        private void Init(STATE code)
        {
            Code = code;
            switch (Code)
            {
                case STATE.CUSTOMER_1:
                case STATE.CUSTOMER_2:
                case STATE.CUSTOMER_3:
                    Desc = DESC_CUSTOMER;
                    break;
                case STATE.VSTAR:
                    Desc = DESC_VSTAR;
                    break;
                case STATE.ADMIN:
                    Desc = DESC_ADMIN;
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
    }
}
