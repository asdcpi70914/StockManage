﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.HardCodes
{
    public class EQUIPMENT_STATE: ISRCState<EQUIPMENT_STATE.STATE>
    {
        public enum STATE
        {
            ENABLE = 0,
            INVALID = 99,
        }

        public const string DESC_ENABLE = "啟用";
        public const string DESC_INVALID = "作廢";

        public STATE Code { get; set; }
        public string Desc { get; set; }

        public EQUIPMENT_STATE(STATE code)
        {
            Init(code);
        }

        public EQUIPMENT_STATE(string state)
        {
            Init(state);
        }

        private void Init(STATE code)
        {
            Code = code;
            switch (Code)
            {
                case STATE.ENABLE:
                    Desc = DESC_ENABLE;
                    break;
                case STATE.INVALID:
                    Desc = DESC_INVALID;
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
            return new EQUIPMENT_STATE(state).Desc;
        }
    }
}
