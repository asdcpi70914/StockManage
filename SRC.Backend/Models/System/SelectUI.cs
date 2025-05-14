using SRC.DB.HardCodes;
using SRC.DB.Interfaces.Settings;

namespace SRC.Backend.Models.System
{
    public class SelectUI : ISelectUI
    {
        public Dictionary<string, string> Citys { get; set; }

        public Dictionary<string, string> Towns { get; set; }

        public Dictionary<Guid, string> MemberDropDown { get; set; }

        public Dictionary<string, string> OrderState { get; set; }

        public Dictionary<string, string> ActiveMode { get; set; }

        public Dictionary<long, string> RestaurantDropDown { get; set; }

        public Dictionary<string, string> BookState { get; set; }

        public Dictionary<string, string> AnalysisItem { get; set; }

        public Dictionary<long, string> ProductDropDown { get; set; }

        public Dictionary<long, string> GoodsMaintain { get; set; }

        public Dictionary<string, string> ProjectState { get; set; }

        private IDF_SystemCode SysCodeDF { get; set; }
        private Serilog.ILogger SLog { get; set; }

        public SelectUI(Serilog.ILogger logger, IDF_SystemCode sysCodeDF)
        {
            SysCodeDF = sysCodeDF;
            SLog = logger;
        }

    }
}
