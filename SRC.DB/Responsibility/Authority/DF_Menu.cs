using Dapper;
using Microsoft.EntityFrameworkCore;
using SRC.DB.Abstract;
using SRC.DB.Interfaces.Authority;
using SRC.DB.Models.EFMSSQL;
using SRC.DB.Models.Funcs;
using SRC.DB.Responsibility.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Responsibility.Authority
{
    public class DF_Menu : ADF, IDF_Menu
    {
        //private IDataBaseFactory DataBaseService { get; set; }
        //public string RMessage { get; protected set; }
        //private InfinityContext DB { get; set; }
        public DF_Menu(IDataBaseFactory dataBaseService, EFContext db) : base(dataBaseService, db)
        {
            //this.DataBaseService = dataBaseService;
            //DB = db;
        }

        public List<SRCMenu> GetMenus(string account)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(
                "  select " +
                "      @account, mball.func_id, " +
                "      f.pid, f.name, f.url, f.parentid, f.type, f.icon, f.weight " +
                "      from " +
                "      ( " +
                "          select rf.func_id " +
                "          from backend_users m " +
                "          join backend_users_role mr on m.user_id = mr.user_id " +
                "          join role_func rf on rf.role_id = mr.role_id " +
                "          where m.account = @account " +
                "          group by rf.func_id " +
                "      ) " +
                "      mball join Func f on f.pid = mball.func_id " +
                "      order by f.weight");


            DynamicParameters prms = new DynamicParameters();
            prms.Add("@account", account);


            System.Data.Common.DbConnection conn = DB.Database.GetDbConnection();

            List<SRCMenu> menus = conn.Query<SRCMenu>(sql.ToString(), prms).ToList();

            return menus;
        }

    }
}
