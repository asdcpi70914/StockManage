using Microsoft.EntityFrameworkCore;
using SRC.DB.Abstract;
using SRC.DB.Interfaces.SubscribePoint;
using SRC.DB.Models.EFMSSQL;
using SRC.DB.Responsibility.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Responsibility.SubscribePoint
{
    public class DF_SubscribePoint: ADF, IDF_SubscribePoint
    {
        public DF_SubscribePoint(IDataBaseFactory dataBaseService, EFContext db) : base(dataBaseService, db)
        {
        }

        public List<subscribepoint_maintain> ListSubscribePoint(string name, DateTime? start_time, DateTime? end_time, int? page, int? take, out int rowtotal)
        {
            IQueryable<subscribepoint_maintain> data = DB.subscribepoint_maintains.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
            {
                data = data.Where(x => x.name.Contains(name));
            }

            if (start_time.HasValue)
            {
                data = data.Where(x => x.create_time >= start_time.Value);
            }

            if (end_time.HasValue)
            {
                data = data.Where(x => x.create_time <= end_time.Value.AddDays(1).AddSeconds(-1));
            }

            return Pagination(data, page, take, out rowtotal);
        }

        public subscribepoint_maintain GetSubscribePoint(long pid)
        {
            return DB.subscribepoint_maintains.AsNoTracking().Where(x => x.pid == pid).FirstOrDefault();
        }


        public bool CheckSameName(long? pid, string name)
        {
            if (pid.HasValue)
            {
                return DB.subscribepoint_maintains.AsNoTracking().Where(x => x.pid != pid.Value && x.name == name).Any();
            }
            else
            {
                return DB.subscribepoint_maintains.AsNoTracking().Where(x => x.name == name).Any();
            }
        }

        public async Task Create(subscribepoint_maintain data)
        {
            await DB.subscribepoint_maintains.AddAsync(data);
            await DB.SaveChangesAsync();
        }

        public async Task Edit(subscribepoint_maintain data)
        {
            var editData = await DB.subscribepoint_maintains.Where(x => x.pid == data.pid).FirstOrDefaultAsync();

            if(editData == null)
            {
                throw new Exception($"查無申購點編輯資料，pid：{data.pid}");
            }

            editData.name = data.name;
            editData.editor = data.editor;
            editData.edit_time = data.edit_time;

            await DB.SaveChangesAsync();
        }

        public async Task Delete(List<long> pids)
        {
            var deleteData = await DB.subscribepoint_maintains.Where(x => pids.Contains(x.pid)).ToListAsync();

            DB.subscribepoint_maintains.RemoveRange(deleteData);

            await DB.SaveChangesAsync();
        }

    }
}
