using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Responsibility.Components
{
    /// <summary>
    /// 資料庫工廠 - 介面
    /// </summary>
    public interface IDataBaseFactory : IDataBaseFactoryRet
    {
        /// <summary>
        /// 開始使用 SP
        /// </summary>
        /// <param name="storedProcedure"> SP 名稱 </param>
        /// <returns> 介面 - 使用 Linq </returns>
        IDataBaseFactory Use(string storedProcedure);

        /// <summary>
        /// 加入參數
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="funcParam">參數</param>
        /// <returns>回傳</returns>
        IDataBaseFactory Add<T>(Func<ISqlParamter, SqlParamter<T>> funcParam);

        /// <summary>
        /// 執行
        /// </summary>
        /// <returns>結果</returns>
        Adapter Execute();

        /// <summary>
        /// 執行
        /// </summary>
        /// <param name="action">取得 TSQL Output 值</param>
        /// <returns>結果</returns>
        Adapter Execute(Action<IDataBaseFactoryRet> action);

        /// <summary>
        /// 執行
        /// </summary>
        /// <typeparam name="VModel">泛型</typeparam>
        /// <returns>結果</returns>
        Adapter<List<VModel>> Execute<VModel>();

        /// <summary>
        /// 執行
        /// </summary>
        /// <typeparam name="VModel">泛型</typeparam>
        /// <param name="action">取得 TSQL Output 值</param>
        /// <returns>結果</returns>
        Adapter<List<VModel>> Execute<VModel>(Action<IDataBaseFactoryRet> action);

        /// <summary>
        /// 執行
        /// </summary>
        /// <typeparam name="VModel">泛型(ViewModel的概念)</typeparam>
        /// <param name="expressions">綁定物件
        /// <para>
        /// 範例 :
        /// Model - Master Include IEnumerable&lt;S1&gt; , IEnumerable&lt;S2&gt;
        /// 對應 - (x => x.S1, x => x.S2)
        /// </para>
        /// </param>
        /// <returns>結果</returns>
        Adapter<VModel> Execute<VModel>(params Expression<Func<VModel, object>>[] expressions) where VModel : new();

        /// <summary>
        /// 執行
        /// </summary>
        /// <typeparam name="VModel">泛型(ViewModel的概念)</typeparam>
        /// <param name="action">取得 TSQL Output 值</param>
        /// <param name="expressions">綁定物件
        /// <para>
        /// 範例 :
        /// Model - Master Include IEnumerable&lt;S1&gt; , IEnumerable&lt;S2&gt;
        /// 對應 - (x => x.S1, x => x.S2)
        /// </para>
        /// </param>
        /// <returns>結果</returns>
        Adapter<VModel> Execute<VModel>(Action<IDataBaseFactoryRet> action, params Expression<Func<VModel, object>>[] expressions) where VModel : new();
    }

    /// <summary>
    /// 資料庫工廠取得回傳值 - 介面
    /// </summary>
    public interface IDataBaseFactoryRet
    {
        /// <summary>
        /// 取得回傳值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="paramterName">參數名稱</param>
        /// <returns>物件</returns>
        T Get<T>(string paramterName);

        /// <summary>
        /// 取得回傳值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns>物件</returns>
        T Get<T>();

        /// <summary>
        /// 取得回傳值
        /// </summary>
        /// <typeparam name="T">Interface source</typeparam>
        /// <typeparam name="V">concrete target</typeparam>
        /// <returns>物件</returns>
        V Get<T, V>();
    }
}
