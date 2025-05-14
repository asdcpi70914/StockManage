using System;

namespace SRC.Backend.Models.System
{
    public class SRCUIPagination
    {
        public string Id { get; set; }

        public int Take { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }

        public SRCUIPagination()
        {

        }
        public SRCUIPagination(int? page, int? take, int rowsCount)
        {
            CurrentPage = page.HasValue ? page.Value : 1;
            Take = take.HasValue ? take.Value : rowsCount;
            if (Take > 0)
            {
                TotalPage = (int)Math.Ceiling((double)rowsCount / Take);
            }
        }
    }
}
