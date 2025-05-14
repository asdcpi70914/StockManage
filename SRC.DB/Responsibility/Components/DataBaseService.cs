using Castle.Components.DictionaryAdapter;
using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Responsibility.Components
{
    /// <summary>
    /// 資料庫底層服務(指令模式)
    /// </summary>
    public class DataBaseService : IDataBaseService
    {
        /// <summary>
        /// 資料庫連線
        /// </summary>
        private IDbConnection Db { get; set; }

        /// <summary>
        /// TSQL OutPut 字典
        /// </summary>
        public Dictionary<string, Type> Dict { get; set; }

        /// <summary>
        /// TSQL 執行後取得 OutPut 結果
        /// </summary>
        private Hashtable OutPut { get; set; }

        /// <summary>
        /// Castle 動態將 Hashtable 轉成 Interface 之工廠
        /// </summary>
        private DictionaryAdapterFactory DictAdapterFactory { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="db">資料庫連線</param>
        public DataBaseService(IDbConnection db) : base()
        {
            Db = db;
            Dict = new Dictionary<string, Type>();
            OutPut = new Hashtable();
            DictAdapterFactory = new DictionaryAdapterFactory();
        }

        /// <summary>
        /// 執行
        /// </summary>
        /// <param name="storedProcedure">SP 名稱</param>
        /// <param name="parmeters">SP 參數</param>
        /// <returns>結果</returns>
        public Adapter Execute(string storedProcedure, DynamicParameters parmeters)
        {
            IAdapter adapter = null;
            using (var conn = Db)
            {
                conn.Execute(storedProcedure, parmeters, commandType: CommandType.StoredProcedure);

                var method = typeof(DynamicParameters).GetMethod("Get");

                foreach (var item in Dict)
                {
                    OutPut[item.Key.TrimStart('@')] = Convert.ChangeType(method.MakeGenericMethod(item.Value).Invoke(parmeters, new object[] { item.Key }), item.Value);
                }

                adapter = DictAdapterFactory.GetAdapter<IAdapter>(OutPut);
            }

            return new Adapter()
            {
                ResCode = adapter.ResCode,
                ResMsg = adapter.ResMsg,
            };
        }

        /// <summary>
        /// 執行
        /// </summary>
        /// <typeparam name="VModel">泛型</typeparam>
        /// <param name="storedProcedure">SP名稱</param>
        /// <param name="parmeters">參數</param>
        /// <returns>結果</returns>
        public Adapter<List<VModel>> Execute<VModel>(string storedProcedure, DynamicParameters parmeters)
        {

            IEnumerable<VModel> result = null;
            IAdapter adapter = null;
            using (var conn = Db)
            {
                result = conn.Query<VModel>(storedProcedure, parmeters, commandType: CommandType.StoredProcedure);

                var method = typeof(DynamicParameters).GetMethod("Get");

                foreach (var item in Dict)
                {
                    OutPut[item.Key.TrimStart('@')] = Convert.ChangeType(method.MakeGenericMethod(item.Value).Invoke(parmeters, new object[] { item.Key }), item.Value);
                }

                adapter = DictAdapterFactory.GetAdapter<IAdapter>(OutPut);

            }

            return new Adapter<List<VModel>>()
            {
                Data = result == null ? new List<VModel>() : result.ToList(),
                ResCode = adapter.ResCode,
                ResMsg = adapter.ResMsg,
            };
        }

        /// <summary>
        /// 指定Dapper底層-避免要用反射還要找多行方法
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="gridReader">目前Db出來之結果</param>
        /// <param name="buffered">Dapper Get Default Buffered</param>
        /// <returns>多筆結果</returns>
        private IEnumerable<T> Read<T>(SqlMapper.GridReader gridReader, bool buffered = true) => gridReader.Read<T>(buffered);

        /// <summary>
        /// 執行
        /// </summary>
        /// <typeparam name="VModel">泛型(ViewModel的概念)</typeparam>
        /// <param name="storedProcedure">SP名稱</param>
        /// <param name="parmeters">參數</param>
        /// <param name="expressions">綁定物件</param>
        /// <returns>結果</returns>
        public Adapter<VModel> Execute<VModel>(string storedProcedure, DynamicParameters parmeters, params Expression<Func<VModel, object>>[] expressions)
          where VModel : new()
        {
            var method = GetType().GetMethod("Read", BindingFlags.NonPublic | BindingFlags.Instance);
            var resultModel = new VModel();
            var resultModelType = typeof(VModel);

            var properties = resultModelType.GetProperties();

            IAdapter adapter = null;

            using (var conn = Db)
            {
                using (var result = conn.QueryMultiple(storedProcedure, parmeters, commandType: CommandType.StoredProcedure))
                {
                    foreach (var expressionItem in expressions)
                    {
                        if (!result.IsConsumed)
                        {
                            var propertyItem = properties.FirstOrDefault(i => i.Name == (expressionItem.Body as MemberExpression).Member.Name);

                            if (propertyItem != null)
                            {
                                var propVal = method.MakeGenericMethod(propertyItem.PropertyType.GetGenericArguments().FirstOrDefault()).Invoke(this, new object[] { result, true });
                                propertyItem.SetValue(resultModel, propVal);
                            }
                        }
                    }

                    method = typeof(DynamicParameters).GetMethod("Get");

                    foreach (var item in Dict)
                    {
                        OutPut[item.Key.TrimStart('@')] = Convert.ChangeType(method.MakeGenericMethod(item.Value).Invoke(parmeters, new object[] { item.Key }), item.Value);
                    }

                    adapter = DictAdapterFactory.GetAdapter<IAdapter>(OutPut);
                }
            }

            return new Adapter<VModel>()
            {
                Data = resultModel,
                ResCode = adapter.ResCode,
                ResMsg = adapter.ResMsg
            };
        }

        /// <summary>
        /// 取得回傳值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns>結果</returns>
        public T Get<T>() => (T)DictAdapterFactory.GetAdapter(typeof(T), OutPut);

        /// <summary>
        /// 取得指定回傳值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="paramterName">SP 參數</param>
        /// <returns>結果</returns>
        public T Get<T>(string paramterName) => (T)Convert.ChangeType(OutPut[paramterName.TrimStart('@')], typeof(T));
    }
}
