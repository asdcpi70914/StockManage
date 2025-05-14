using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.HardCodes
{
    public abstract class ISRCState<T> where T : Enum
    {
        public abstract string GetDesc(T state);


        public Dictionary<string, string> All()
        {
            Dictionary<string, string> all = new Dictionary<string, string>();
            T state;

            foreach (var each in Enum.GetNames(typeof(T)))
            {
                state = (T)Enum.Parse(typeof(T), each);
                all.Add(each, GetDesc(state));
            }

            return all;
        }
    }
}
