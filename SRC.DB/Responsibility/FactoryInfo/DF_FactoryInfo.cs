using Microsoft.EntityFrameworkCore;
using SRC.DB.Abstract;
using SRC.DB.Interfaces.FactoryInfo;
using SRC.DB.Models.EFMSSQL;
using SRC.DB.Responsibility.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Responsibility.FactoryInfo
{
    public class DF_FactoryInfo:ADF, IDF_FactoryInfo
    {
        public DF_FactoryInfo(IDataBaseFactory dataBaseService, EFContext db) : base(dataBaseService, db)
        {
        }

        public List<factoryinfo_maintain> ListFactoryInfo(string name, string city, string town, string address, DateTime? start_time, DateTime? end_time, int? page, int? take, out int rowtotal)
        {
            IQueryable<factoryinfo_maintain> data = DB.factoryinfo_maintains.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
            {
                data = data.Where(x => x.name.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                data = data.Where(x => x.city == city);
            }

            if (!string.IsNullOrWhiteSpace(town))
            {
                data = data.Where(x => x.town == town);
            }

            if (!string.IsNullOrWhiteSpace(address))
            {
                data = data.Where(x => x.address.Contains(address));
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

        public factoryinfo_maintain GetFactoryInfo(long pid)
        {
            return DB.factoryinfo_maintains.AsNoTracking().Where(x => x.pid == pid).FirstOrDefault();
        }

        public async Task Create(factoryinfo_maintain data)
        {
            await DB.factoryinfo_maintains.AddAsync(data);

            await DB.SaveChangesAsync();
        }

        public async Task Edit(factoryinfo_maintain data)
        {
            var editData = await DB.factoryinfo_maintains.Where(x => x.pid == data.pid).FirstOrDefaultAsync();

            if (editData == null)
            {
                throw new Exception($"查無廠商編輯資料，pid：{data.pid}");
            }

            editData.name = data.name;
            editData.contact_phone = data.contact_phone;
            editData.company_number = data.company_number;
            editData.city = data.city;
            editData.town = data.town;
            editData.address = data.address;
            editData.editor = data.editor;
            editData.edit_time = data.edit_time;

            await DB.SaveChangesAsync();
        }

        public async Task Delete(List<long> pids)
        {
            var deleteData = await DB.factoryinfo_maintains.Where(x => pids.Contains(x.pid)).ToListAsync();

            DB.factoryinfo_maintains.RemoveRange(deleteData);

            await DB.SaveChangesAsync();
        }
    }
}
