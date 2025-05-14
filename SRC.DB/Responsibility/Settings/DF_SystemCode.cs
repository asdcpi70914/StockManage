using SRC.DB.HardCodes;
using Microsoft.EntityFrameworkCore;
using SRC.DB.Abstract;
using SRC.DB.Interfaces.Settings;
using SRC.DB.Models.Complex;
using SRC.DB.Models.EFMSSQL;
using SRC.DB.Responsibility.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Responsibility.Settings
{
    public class DF_SystemCode : ADF, IDF_SystemCode
    {
        public DF_SystemCode(IDataBaseFactory dataBaseService, EFContext db) : base(dataBaseService, db)
        {
        }


        public List<system_code> SearchSystemCode()
        {
            return DB.system_codes.ToList();
        }

        public List<system_code> List_SystemCode(string Code)
        {
            return DB.system_codes.Where(x => x.code == Code).ToList();
        }

        public string GetBackendUserSalt()
        {
            return DB.system_codes.AsNoTracking().Where(m => m.code == "BACKEND_USER_SALT").FirstOrDefault().data;
        }

        public system_code GetSystemCode(long pid)
        {
            return DB.system_codes.Where(x => x.pid == pid).AsNoTracking().FirstOrDefault();
        }

        public system_code GetSystem_Code(string code, string data)
        {
            IQueryable<system_code> Data = DB.system_codes;

            if (!string.IsNullOrWhiteSpace(code))
            {
                Data = Data.Where(x => x.code == code);
            }

            if (!string.IsNullOrEmpty(data))
            {
                Data = Data.Where(x => x.data == data);
            }

            return Data.FirstOrDefault();
        }

        public SmtpConfig GetSmtpConfig()
        {
            List<system_code> smtpSetting = DB.system_codes.AsNoTracking().Where(m => m.code == "SMTPCONFIG").ToList();
            SmtpConfig config = new SmtpConfig();
            config.From = smtpSetting.Where(m => m.data == "From").FirstOrDefault().description;
            config.MailServerAccount = smtpSetting.Where(m => m.data == "MailServerAccount").FirstOrDefault().description;
            config.MailServerPassword = smtpSetting.Where(m => m.data == "MailServerPassword").FirstOrDefault().description;
            config.MailServer = smtpSetting.Where(m => m.data == "MailServer").FirstOrDefault().description;
            string temp = smtpSetting.Where(m => m.data == "Port").FirstOrDefault().description;
            int port = 0;
            if (int.TryParse(temp, out port))
            {
                config.Port = port;
            }

            return config;

        }

        public string GetProjectFileUploadPath()
        {
            return DB.system_codes.AsNoTracking().Where(x => x.code == "FOLDER" && x.data == "PJ_TRACE").FirstOrDefault()?.description;
        }
        public string GetDesignFileUploadPath()
        {
            return DB.system_codes.AsNoTracking().Where(x => x.code == "FOLDER" && x.data == "PJ_DESIGN").FirstOrDefault()?.description;
        }
        public string GetBomFileUploadPath()
        {
            return DB.system_codes.AsNoTracking().Where(x => x.code == "FOLDER" && x.data == "PJ_BOM").FirstOrDefault()?.description;
        }

        public string GetQualityAssuranceFileUploadPath()
        {
            return DB.system_codes.AsNoTracking().Where(x => x.code == "FOLDER" && x.data == "PJ_QA").FirstOrDefault()?.description;
        }

        public Dictionary<string, string> GetProductTypeDropDown()
        {
            return DB.system_codes.AsNoTracking().Where(x => x.code == "PRODUCT_TYPE").ToDictionary(x => x.data, x => x.description);
        }

        public string GetProductTypeDesc(string data)
        {
            return DB.system_codes.AsNoTracking().Where(x => x.code == "PRODUCT_TYPE" && x.data == data).FirstOrDefault()?.description;
        }
    }
}
