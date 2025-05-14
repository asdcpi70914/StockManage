using SRC.DB.Responsibility;
using SRC.DB.Interfaces.Authority;
using SRC.DB.Interfaces.Settings;
using SRC.DB.Interfaces.Users;
using SRC.DB.Responsibility.Authority;
using SRC.DB.Responsibility.Settings;
using SRC.DB.Responsibility.Users;

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
                ;
            ;
        }
    }
}
