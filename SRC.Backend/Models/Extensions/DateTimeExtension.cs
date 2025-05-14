namespace SRC.Backend.Models.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime Day1(this DateTime time)
        {
            return Convert.ToDateTime(time.ToString("yyyy/MM/01"));
        }

        public static DateTime NextMonthDay1(this DateTime time)
        {
            return time.Day1().AddMonths(1);
        }

        public static DateTime FirstDay(this DateTime time)
        {
            return Convert.ToDateTime(time.ToString("yyyy/MM/01"));
        }
    }
}
