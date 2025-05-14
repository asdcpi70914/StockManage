using EProto.DB.Interfaces.Settings;

namespace EProto.Backend.Models.Brain
{
    public class TaiwanAddress
    {
        public string PostalCode { get; protected set; }
        public string CityCode { get; protected set; }
        public string TownCode { get; protected set; }
        public string Address { get; protected set; }

        public string CityDesc { get; protected set; }
        public string TownDesc { get; protected set; }
        public string AddressDesc { get; protected set; }

        private IDF_SystemCode CodeDB { get; set; }


        public TaiwanAddress(IDF_SystemCode db, string postalCode, string cityCode, string townCode, string address)
        {
            Init(db, postalCode, cityCode, townCode, address);
        }

        private void Init(IDF_SystemCode db, string postalCode, string cityCode, string townCode, string address)
        {
            CodeDB = db;
            PostalCode = postalCode;
            CityCode = cityCode;
            TownCode = townCode;
            Address = address;
            AddressDesc = $"{postalCode} {address}";

            system_city_code city = CodeDB.GetCity(cityCode);

            if (city == null) return;

            CityDesc = city.name;
            AddressDesc = $"{postalCode} {CityDesc}{address}";

            system_towncode town = CodeDB.GetTown(city.county_code, townCode);

            if (town == null) return;

            TownDesc = town.TownName;
            AddressDesc = $"{postalCode} {CityDesc}{TownDesc}{address}";
        }

    }
}
