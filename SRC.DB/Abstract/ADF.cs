using Azure;
using Dapper;
using Microsoft.EntityFrameworkCore;
using SRC.DB.Models.EFMSSQL;
using SRC.DB.Responsibility.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Abstract
{
    public abstract class ADF
    {
        protected IDataBaseFactory DataBaseService { get; set; }
        public string InnerMessage { get; protected set; }
        protected EFContext DB { get; set; }

        public int PaginationDataCount { get; protected set; }

        protected ADF(IDataBaseFactory dataBaseService, EFContext db)
        {
            DataBaseService = dataBaseService;
            DB = db;
        }

        protected List<T> Pagination<T>(IQueryable<T> data, int? page, int? take, out int totalCount)
        {
            if (!page.HasValue || !take.HasValue)
            {
                List<T> temp = data.ToList();
                totalCount = temp.Count();
                return temp;
            }

            totalCount = data.Count();

            int calPage = page.Value - 1;
            calPage = calPage < 0 ? 0 : calPage;
            return data.Skip(calPage * take.Value).Take(take.Value).ToList();
        }

        //protected async Task<List<T>> PaginationAsync<T>(IQueryable<T> data, int? page, int? take)
        //{
        //    if (!page.HasValue || !take.HasValue)
        //    {
        //        List<T> temp = data.ToList();
        //        PaginationDataCount = temp.Count();
        //        return temp;
        //    }

        //    PaginationDataCount = data.Count();

        //    int calPage = page.Value - 1;
        //    calPage = calPage < 0 ? 0 : calPage;

        //    return await data.Skip(calPage * take.Value).Take(take.Value).ToListAsync();
        //}

        /// <summary>
        /// 查詢資料並分頁
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL查詢語法</param>
        /// <param name="orderStatement">排序 ex: order by create_time desc , 注意欄位可以有資料表別名 ex: member.create_time </param>
        /// <param name="prms">Dapper的Parameter</param>
        /// <param name="page">目前第幾頁</param>
        /// <param name="take">每頁幾筆</param>
        /// <param name="rowsCount">目前查詢結果的資料總筆數</param>
        /// <returns></returns>
        protected List<T> Pagination<T>(string sql, string orderStatement, DynamicParameters prms, int? page, int? take, out int rowsCount)
        {
            System.Data.Common.DbConnection conn = DB.Database.GetDbConnection();
            PaginationDataCount = rowsCount = 0;

            try
            {

                if (page.HasValue && take.HasValue)
                {
                    //rowsCount = conn.Query<int>($"select count(1) from ( {sql.Insert(sql.ToLower().IndexOf("select") + 6, " top 100 percent ")} ) as mrgautocct ", prms).First();

                    rowsCount = conn.Query<int>($"select count(1) from ( {sql} ) as mrgautocct ", prms).First();

                    //orderStatement = string.IsNullOrWhiteSpace(orderStatement) ? "" : $"{orderStatement} offset";

                    return conn.Query<T>($"{sql} {orderStatement} offset {(page.Value - 1) * take.Value} rows fetch next {take.Value} rows only", prms).ToList();

                    //return conn.Query<T>($"{sql} {orderStatement} limit {(page.Value - 1) * take.Value} , {take.Value} ", prms).ToList();
                }
                else
                {
                    List<T> result = conn.Query<T>(sql.ToString(), prms).ToList();
                    PaginationDataCount = rowsCount = result.Count;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }

        protected List<T> Query<T>(string sql, DynamicParameters prms)
        {
            return Pagination<T>(sql, string.Empty, prms, null, null, out int rowcount);
        }

        protected List<T> Query<T>(string sql, DynamicParameters prms, int? page, int? take, out int rowtotal)
        {
            return Pagination<T>(sql, string.Empty, prms, page, take, out rowtotal);
        }
    }
}
