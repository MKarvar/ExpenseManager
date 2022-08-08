using System;
using System.Globalization;

namespace ExpenseManager.ApplicationService.Utilities
{
    public static class DatetimeExtensions
    {
        public static string ConvertMiladiToShamsi(this DateTime? date)
        {
            if (!date.HasValue)
                return string.Empty;
            return ConvertMiladiToShamsi(date.Value);
        }

        public static string ConvertMiladiToShamsi(this DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            return  $"{pc.GetYear(date)}/{pc.GetMonth(date)}/{pc.GetDayOfMonth(date)} {pc.GetHour(date)}:{pc.GetMinute(date)}:{pc.GetSecond(date)}";
        }
    }
}
