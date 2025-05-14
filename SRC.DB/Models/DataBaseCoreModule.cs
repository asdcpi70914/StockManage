using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using SRC.DB.Responsibility.Components;

namespace SRC.DB.Models
{
    /// <summary>
    /// 資料庫核心模組
    /// </summary>
    public static class DataBaseCoreModule
    {
        public enum DatabaseEngine
        {
            /// <summary>
            /// 未定義預設值
            /// </summary>
            UNKNOW = 0,

            /// <summary>
            /// Microsoft SQL Database
            /// </summary>
            MSSQL = 1,

            /// <summary>
            /// MySQL Database
            /// </summary>
            MYSQL = 2
        }

        /// <summary>
        /// 注入資料庫相關執行底層
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataBaseCoreFactory(this IServiceCollection services, IConfiguration configuration, DatabaseEngine Engine)
        {
            //return services.AddScoped<Func<string, IDataBaseService>>(
            //                    provider => 
            //                        key => new DataBaseService(new SqlConnection(configuration.GetConnectionString(key.Split('.').FirstOrDefault()))))
            //               .AddTransient<IDataBaseFactory, DataBaseFactory>();




            switch (Engine)
            {
                case DatabaseEngine.MSSQL:
                    return services.AddScoped<Func<string, IDataBaseService>>(
                                        provider =>
                                            key => new DataBaseService(new SqlConnection(configuration.GetConnectionString(key.Split('.').FirstOrDefault()))))
                                   .AddTransient<IDataBaseFactory, DataBaseFactory>();
                case DatabaseEngine.MYSQL:
                    return services.AddScoped<Func<string, IDataBaseService>>(
                                        provider =>
                                            key => new DataBaseService(new MySqlConnection(configuration.GetConnectionString("DefaultConnection"))))
                                   .AddTransient<IDataBaseFactory, MySqlDataBaseFactory>();
                default:
                    throw new ArgumentException("Database Engine Invalid");
            }
        }
    }
}
