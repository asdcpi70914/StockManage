using SRC.Backend.Models.Pages.UnitUseReport;
using SRC.DB.Interfaces.UnitApply;

namespace SRC.Backend.Models.Brain
{
    public class UnitUseReportLogic
    {
        private Serilog.ILogger Logger { get; set; }
        private IDF_UnitApply DF_UnitApply { get; set; }
        public string InnerMessage { get; set; }

        public UnitUseReportLogic(Serilog.ILogger logger,IDF_UnitApply dF_UnitApply)
        {
            Logger = logger;
            DF_UnitApply = dF_UnitApply;
        }

        public UnitUseReportSearch Search(UnitUseReportIndex.SearchModel condition,int? page,int? take)
        {
            UnitUseReportSearch Model = new UnitUseReportSearch();
            try
            {
                Model.data = DF_UnitApply.ListUnitApply(condition.type, condition.sub_pid, null, condition.start_time, condition.end_time, "", page, take, out int rowtotal);

                Model.Pagination = new System.SRCUIPagination(page,take,rowtotal);
            }
            catch(Exception ex)
            {
                Logger.Fatal(ex, $"取得使用單位年報表發生異常，{ex.Message}");
            }

            return Model;
        }
    }
}
