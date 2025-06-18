using SRC.DB.Models.Complex;
using SRC.DB.Models.EFMSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Interfaces.UnitApply
{
    public interface IDF_UnitApply
    {
        List<UnitApplyComplex> ListUnitApply(string type, long? setting_pid, long? subscribepoint_pid, DateTime? start_time, DateTime? end_time, List<string> state, long? unit, int? page, int? take, out int rowtotal);

        UnitApplyEditComplex GetUnitApply(long pid);

        UnitApplyComplex GetReviewUnitApply(long pid);

        Task Create(unit_apply data);

        Task Edit(unit_apply data,unit_apply_review_log log);

        Task Delete(List<long> pids, string account);

        Task Review(long pid, string state, string memo,string account);

        Dictionary<long, string> DicMinBaseStoc(string type,long? unit = null);

        min_base_stock_subscribe_setting GetRemainingStock(string type, long pid);

        List<UnitApplyReviewLogComplex> ListReviewLogs(long pid);
    }
}
