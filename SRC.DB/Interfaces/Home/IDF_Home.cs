using SRC.DB.Models.Complex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Interfaces.Home
{
    public interface IDF_Home
    {
        List<HomeComplex.HomeUpData> ListHomeUpData(long? unit);
        List<HomeComplex.HomeMidData> ListHomeMidData(long? unit);
        List<HomeComplex.HomeDownData> ListHomeDownData(long? unit);
    }
}
