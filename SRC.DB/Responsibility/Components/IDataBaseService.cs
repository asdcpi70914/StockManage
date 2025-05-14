using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Responsibility.Components
{
    /// <summary>
    /// 資料庫底層服務(指令模式)
    /// </summary>
    public interface IDataBaseService
    {
        /// <summary>
        /// TSQL OutPut 字典
        /// </summary>
        Dictionary<string, Type> Dict { get; set; }

        /// <summary>
        /// 執行
        /// </summary>
        /// <param name="storedProcedure">SP 名稱</param>
        /// <param name="parmeters">SP 參數</param>
        /// <returns>結果</returns>
        Adapter Execute(string storedProcedure, DynamicParameters parmeters);

        /// <summary>
        /// 執行
        /// </summary>
        /// <typeparam name="VModel">泛型</typeparam>
        /// <param name="storedProcedure">SP名稱</param>
        /// <param name="parmeters">參數</param>
        /// <returns>結果</returns>
        Adapter<List<VModel>> Execute<VModel>(string storedProcedure, DynamicParameters parmeters);

        /// <summary>
        /// 執行
        /// </summary>
        /// <typeparam name="VModel">泛型(ViewModel的概念)</typeparam>
        /// <param name="storedProcedure">SP名稱</param>
        /// <param name="parmeters">參數</param>
        /// <param name="expressions">綁定物件</param>
        /// <returns>結果</returns>
        Adapter<VModel> Execute<VModel>(string storedProcedure, DynamicParameters parmeters, params Expression<Func<VModel, object>>[] expressions)
             where VModel : new();

        /// <summary>
        /// 取得回傳值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns>結果</returns>
        T Get<T>();

        /// <summary>
        /// 取得指定回傳值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="paramterName">SP 參數</param>
        /// <returns>結果</returns>
        T Get<T>(string paramterName);
    }
}
