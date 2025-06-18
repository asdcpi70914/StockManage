using Azure;
using Dapper;
using Microsoft.EntityFrameworkCore;
using SRC.DB.Abstract;
using SRC.DB.HardCodes;
using SRC.DB.Interfaces.Home;
using SRC.DB.Models.Complex;
using SRC.DB.Models.EFMSSQL;
using SRC.DB.Responsibility.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Responsibility.Home
{
    public class DF_Home:ADF, IDF_Home
    {
        public DF_Home(IDataBaseFactory dataBaseService, EFContext db) : base(dataBaseService, db)
        {
        }

        public List<HomeComplex.HomeUpData> ListHomeUpData(long? unit)
        {
            DynamicParameters prms = new DynamicParameters();
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("SELECT eq.name AS equipment_name,sm.name AS subscribepoint_name,main.min_base_stock,main.stock,eq.state ");
            sql.AppendLine("FROM min_base_stock_subscribe_setting main");
            sql.AppendLine("INNER JOIN equipment_maintain eq ON main.sub_pid = eq.pid");
            sql.AppendLine("LEFT JOIN subscribepoint_maintain sm on main.subscribepoint_pid = sm.pid");
            sql.AppendLine("WHERE eq.state = 'ENABLE'");


            if (unit.HasValue)
            {
                sql.AppendLine("AND main.subscribepoint_pid = @subscribepoint_pid");
                prms.Add("@subscribepoint_pid", unit.Value);
            }

            sql.AppendLine("Group BY eq.name,sm.name,main.min_base_stock,main.stock,eq.state");
            sql.AppendLine("Having main.stock < main.min_base_stock");

            return Pagination<HomeComplex.HomeUpData>(
                sql.ToString(), " order by create_time ", prms, null, null, out int rowsCount);

        }

        public List<HomeComplex.HomeMidData> ListHomeMidData(long? unit)
        {
            DynamicParameters prms = new DynamicParameters();
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("SELECT main.pid,eq.name AS equipment_name,sm.name AS subscribepoint_name,main.min_base_stock,main.stock,eq.state,eq.price,eq.type ");
            sql.AppendLine("FROM min_base_stock_subscribe_setting main");
            sql.AppendLine("INNER JOIN equipment_maintain eq ON main.sub_pid = eq.pid");
            sql.AppendLine("LEFT JOIN subscribepoint_maintain sm on main.subscribepoint_pid = sm.pid");
            sql.AppendLine("WHERE eq.state = 'ENABLE'");

            if (unit.HasValue)
            {
                sql.AppendLine("AND main.subscribepoint_pid = @subscribepoint_pid");
                prms.Add("@subscribepoint_pid", unit.Value);
            }

            sql.AppendLine("Group By main.pid,eq.name,sm.name,main.min_base_stock,main.stock,eq.state,eq.price,eq.type");


            return Pagination<HomeComplex.HomeMidData>(
                sql.ToString(), " order by create_time ", prms, null, null, out int rowsCount);
        }
        public List<HomeComplex.HomeDownData> ListHomeDownData(long? unit)
        {
            DynamicParameters prms = new DynamicParameters();
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("SELECT sm.name AS unit,SUM((main.apply_amount * eq.price)) AS amount ");
            sql.AppendLine("FROM unit_apply main");
            sql.AppendLine("LEFT JOIN min_base_stock_subscribe_setting setting ON main.setting_pid = setting.pid");
            sql.AppendLine("INNER JOIN equipment_maintain eq ON setting.sub_pid = eq.pid");
            sql.AppendLine("LEFT JOIN subscribepoint_maintain sm ON main.unit = sm.pid");

            if (unit.HasValue)
            {
                sql.AppendLine("Where setting.subscribepoint_pid = @subscribepoint_pid");
                prms.Add("@subscribepoint_pid", unit.Value);
            }

            sql.AppendLine("GROUP BY sm.name");

            var result = Pagination<HomeComplex.HomeDownData>(
                sql.ToString(), " order by create_time ", prms, null, null, out int rowsCount);

            return result.OrderByDescending(x => x.amount).Take(10).ToList(); ;
        }
    }
}
