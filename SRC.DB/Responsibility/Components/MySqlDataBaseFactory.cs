using AutoMapper;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Responsibility.Components
{
    /// <summary>
    /// 資料庫工廠
    /// </summary>
    public class MySqlDataBaseFactory : IDataBaseFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataBaseFactory"/> class.
        /// 資料庫工廠
        /// </summary>
        /// <param name="funcDbService">資料庫實作</param>
        public MySqlDataBaseFactory(Func<string, IDataBaseService> funcDbService)
            : base()
        {
            FuncDbService = funcDbService;
        }

        /// <summary>
        /// Gets or sets 取得尚未確認生命週期之服務
        /// </summary>
        private Func<string, IDataBaseService> FuncDbService { get; }

        /// <summary>
        /// Gets or sets 取得封裝服務
        /// </summary>
        protected internal IDataBaseService DbService { get; private set; }

        /// <summary>
        /// Gets or sets SP 名稱
        /// </summary>
        public string StoredProcedure { get; private set; }

        /// <summary>
        /// Gets or sets 參數
        /// </summary>
        private DynamicParameters Parmeters { get; set; }

        /// <summary>
        /// 開始使用 SP
        /// </summary>
        /// <param name="storedProcedure"> SP 名稱 </param>
        /// <returns> 介面 - 使用 Linq </returns>
        public IDataBaseFactory Use(string storedProcedure)
        {
            DbService = FuncDbService(storedProcedure);
            StoredProcedure = storedProcedure; //string.Join(".", storedProcedure.Split('.').Skip(2)).Replace("]", "").Replace("[", "");
            Parmeters = new DynamicParameters();
            return this;
        }

        /// <summary>
        /// 加入參數
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="funcParam">參數</param>
        /// <returns>回傳</returns>
        public IDataBaseFactory Add<T>(Func<ISqlParamter, SqlParamter<T>> funcParam)
        {
            // 故意塞 null 值, 待擴充再產生新物件
            var param = funcParam(null);

            if (param.Direction == DirectionType.InputOutput || param.Direction == DirectionType.Output)
            {
                Parmeters.Add(param.Name, param.Value, direction: (ParameterDirection)param.Direction, size: int.MaxValue);

                DbService.Dict.Add(param.Name, typeof(T));
            }
            else
            {
                if (param.Value != null && param.Value.GetType() == typeof(DataTable))
                    Parmeters.Add(param.Name, param.Value, DbType.Object);
                else
                    Parmeters.Add(param.Name, param.Value);
            }

            return this;
        }

        /// <summary>
        /// 執行
        /// </summary>
        /// <returns>結果</returns>
        public Adapter Execute()
        {
            Add(i => i.Model("ResCode"));
            Add(i => i.Model("ResMsg"));
            //this.Add(i => i.Model("456", new Adapter()));

            return DbService.Execute(StoredProcedure, Parmeters);
        }


        /// <summary>
        /// 執行
        /// </summary>
        /// <typeparam name="VModel">泛型</typeparam>
        /// <returns>結果</returns>
        public Adapter<List<VModel>> Execute<VModel>()
        {
            Add(i => i.Model("ResCode"));
            Add(i => i.Model("ResMsg"));

            return DbService.Execute<VModel>(StoredProcedure, Parmeters);
        }

        /// <summary>
        /// 執行
        /// </summary>
        /// <param name="action">取得 TSQL Output 值</param>
        /// <returns>結果</returns>
        public Adapter Execute(Action<IDataBaseFactoryRet> action)
        {
            Add(i => i.Model("ResCode"));
            Add(i => i.Model("ResMsg"));
            //this.Add(i => i.Model("456", new Adapter()));

            var result = DbService.Execute(StoredProcedure, Parmeters);

            action(this);

            return result;
        }

        /// <summary>
        /// 執行
        /// </summary>
        /// <typeparam name="VModel">泛型</typeparam>
        /// <param name="action">取得 TSQL Output 值</param>
        /// <returns>結果</returns>
        public Adapter<List<VModel>> Execute<VModel>(Action<IDataBaseFactoryRet> action)
        {
            Add(i => i.Model("ResCode"));
            Add(i => i.Model("ResMsg"));
            var result = DbService.Execute<VModel>(StoredProcedure, Parmeters);

            action(this);

            return result;
        }

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
        public Adapter<VModel> Execute<VModel>(params Expression<Func<VModel, object>>[] expressions)
            where VModel : new()
        {
            Add(i => i.Model("ResCode"));
            Add(i => i.Model("ResMsg"));

            return DbService.Execute(StoredProcedure, Parmeters, expressions);
        }

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
        public Adapter<VModel> Execute<VModel>(Action<IDataBaseFactoryRet> action, params Expression<Func<VModel, object>>[] expressions)
            where VModel : new()
        {
            Add(i => i.Model("ResCode"));
            Add(i => i.Model("ResMsg"));

            var result = DbService.Execute(StoredProcedure, Parmeters, expressions);

            action(this);

            return result;
        }

        /// <summary>
        /// 取得回傳值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="paramterName">參數名稱</param>
        /// <returns>物件</returns>
        public T Get<T>(string paramterName) => DbService.Get<T>(paramterName);

        /// <summary>
        /// 取得回傳值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns>物件</returns>
        public T Get<T>() => DbService.Get<T>();

        /// <summary>
        /// 取得回傳值
        /// </summary>
        /// <typeparam name="T">Interface source</typeparam>
        /// <typeparam name="V">concrete target</typeparam>
        /// <returns>物件</returns>
        public V Get<T, V>()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, V>()).CreateMapper();
            var source = Get<T>();
            return mapper.Map<V>(source);
        }
    }
}
