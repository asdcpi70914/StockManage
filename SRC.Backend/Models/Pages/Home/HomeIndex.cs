using SRC.DB.Models.Complex;

namespace SRC.Backend.Models.Pages.Home
{
    public class HomeIndex
    {
        public List<IGrouping<string,HomeComplex.HomeUpData>> UpData { get; set; } = new List<IGrouping<string,HomeComplex.HomeUpData>>();
        public List<HomeComplex.HomeMidData> MidData { get; set; } = new List<HomeComplex.HomeMidData>();
        public List<HomeComplex.HomeDownData> DownData { get; set; } = new List<HomeComplex.HomeDownData>();
    }
}
