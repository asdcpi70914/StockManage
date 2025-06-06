using SRC.DB.Responsibility;
using SRC.DB.Interfaces.Authority;
using SRC.DB.Interfaces.Settings;
using SRC.DB.Interfaces.Users;
using SRC.DB.Responsibility.Authority;
using SRC.DB.Responsibility.Settings;
using SRC.DB.Responsibility.Users;
using SRC.DB.Interfaces.Equipment;
using SRC.DB.Responsibility.Equipment;
using Microsoft.Extensions.DependencyInjection;
using SRC.DB.Interfaces.SubscribePoint;
using SRC.DB.Responsibility.SubscribePoint;
using SRC.DB.Responsibility.MinBaseStock;
using SRC.DB.Interfaces.MinBaseStock;
using SRC.DB.Interfaces.UnitApply;
using SRC.DB.Responsibility.UnitApply;
using SRC.DB.Interfaces.Distribute;
using SRC.DB.Responsibility.Distribute;
using SRC.DB.Interfaces.SupplyRoomApply;
using SRC.DB.Responsibility.SupplyRoomApply;
using SRC.DB.Interfaces.Delivery;
using SRC.DB.Responsibility.Delivery;
using SRC.DB.Interfaces.PurchaseStockIn;
using SRC.DB.Responsibility.PurchaseStockIn;
using SRC.DB.Interfaces.PayTreasury;
using SRC.DB.Responsibility.PayTreasury;
using SRC.DB.Interfaces.ExistingStock;
using SRC.DB.Responsibility.ExistingStock;

namespace SRC.Backend.Models.System
{
    public static class DFServerCollectionModule
    {
        public static IServiceCollection AddDFModule(this IServiceCollection services)
        {
            return
                services
                .AddTransient<IDF_BackendUser, DF_BackendUser>()
                .AddTransient<IDF_Role, DF_Role>()
                .AddTransient<IDF_SystemCode, DF_SystemCode>()
                .AddTransient<IDF_Menu, DF_Menu>()
                .AddTransient<IDF_Equipment, DF_Equipment>()
                .AddTransient<IDF_Material, DF_Material>()
                .AddTransient<IDF_SubscribePoint, DF_SubscribePoint>()
                .AddTransient<IDF_MinBaseStock, DF_MinBaseStock>()
                .AddTransient<IDF_UnitApply, DF_UnitApply>()
                .AddTransient<IDF_Distribute, DF_Distribute>()
                .AddTransient<IDF_SupplyRoomApply, DF_SupplyRoomApply>()
                .AddTransient<IDF_Delivery, DF_Delivery>()
                .AddTransient<IDF_PurchaseStockIn, DF_PurchaseStockIn>()
                .AddTransient<IDF_PayTreasury, DF_PayTreasury>()
                .AddTransient<IDF_ExistingStock, DF_ExistingStock>()
                ;
            ;
        }
    }
}
