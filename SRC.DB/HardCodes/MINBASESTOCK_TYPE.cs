using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.HardCodes
{
    public class MINBASESTOCK_TYPE: ISRCState<MINBASESTOCK_TYPE.STATE>
    {
        public enum STATE
        {
            EQUIPMENT= 0,
            MATERIAL = 1,
        }

        public const string DESC_EQUIPMENT = "裝備";
        public const string DESC_MATERIAL = "器材";

        public STATE Code { get; set; }
        public string Desc { get; set; }

        public MINBASESTOCK_TYPE(STATE code)
        {
            Init(code);
        }

        public MINBASESTOCK_TYPE(string state)
        {
            Init(state);
        }

        public MINBASESTOCK_TYPE()
        {

        }

        private void Init(STATE code)
        {
            Code = code;
            switch (Code)
            {
                case STATE.EQUIPMENT:
                    Desc = DESC_EQUIPMENT;
                    break;
                case STATE.MATERIAL:
                    Desc = DESC_MATERIAL;
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
            return new MINBASESTOCK_TYPE(state).Desc;
        }
    }
}
