

namespace SRC.Backend.Models.Pages.Equipment
{
    public class EquipmentIndex
    {
        public class SearchModel
        {
            public string name { get; set; }
            public int? price { get; set; }
            public DateTime? start_time { get; set; }
            public DateTime? end_time { get; set; }
            public string state { get; set; }
        }

        public EquipmentSearch SearchResultPage { get; set; }
    }
}
