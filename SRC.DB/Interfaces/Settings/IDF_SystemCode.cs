using SRC.DB.Models.Complex;
using SRC.DB.Models.EFMSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Interfaces.Settings
{
    public interface IDF_SystemCode
    {
        List<system_code> SearchSystemCode();
        List<system_code> List_SystemCode(string Code);
        system_code GetSystemCode(long pid);
        system_code GetSystem_Code(string code, string data);
        string GetBackendUserSalt();

        SmtpConfig GetSmtpConfig();

        string GetProjectFileUploadPath();
        string GetDesignFileUploadPath();
        string GetBomFileUploadPath();
        string GetQualityAssuranceFileUploadPath();

        Dictionary<string, string> GetProductTypeDropDown();
        string GetProductTypeDesc(string data);

        List<system_towncode> ListTownCode(string Code);

        List<system_city_code> ListCityCode();

        system_towncode GetSystemTownCode(string TownCode);
        system_city_code GetSystemCityCode(string CityCode);

        List<system_city_code> ListCityCode(List<string> Codes);
        List<system_towncode> ListTownCode(List<string> Codes);
    }
}
