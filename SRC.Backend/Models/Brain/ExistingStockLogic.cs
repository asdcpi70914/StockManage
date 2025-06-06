using SRC.Backend.Models.Pages.ExistingStock;
using SRC.DB.Interfaces.ExistingStock;

namespace SRC.Backend.Models.Brain
{
    public class ExistingStockLogic
    {
        private Serilog.ILogger Logger {  get; set; }
        private IDF_ExistingStock DF_ExistingStock {  get; set; }
        public string InnerMessage { get; set; }

        public ExistingStockLogic(Serilog.ILogger logger,IDF_ExistingStock dF_ExistingStock)
        {
            Logger = logger;
            DF_ExistingStock = dF_ExistingStock;
        }

        public ExistingStockSearch Search(ExistingStockIndex.SearchModel condition,int? page,int? take)
        {
            ExistingStockSearch Model = new ExistingStockSearch();

            try
            {
                Model.Data = DF_ExistingStock.ListEquipmentMaintain(condition.name, condition.start_time, condition.end_time, page, take, out int rowtotal);
                Model.Pagination = new System.SRCUIPagination(page, take, rowtotal);
            }
            catch(Exception ex)
            {
                Logger.Fatal(ex, $"查詢現有存量清冊資料發生異常，{ex.Message}");
            }

            return Model;
        }
    }
}
