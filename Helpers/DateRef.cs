using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeCard.Helpers
{
    public static class DateRef
    {
        const string BaselineDate = "12/22/2018";
        public static decimal GetWorkDay(DateTime date)
        {
            DateTime refDate = DateTime.Parse(BaselineDate);
            decimal days = (decimal)(date - refDate).TotalDays;

            decimal workDay = decimal.Floor(days / 14) + decimal.Remainder(days,14)/100;
            return workDay;
        }

        public static string GetWorkDate(decimal workDay, bool year=true)
        {
            int cycle = (int)Decimal.Floor(workDay);
            DateTime refDate = DateTime.Parse(BaselineDate);
            if (year)
            {
                return $"{ refDate.AddDays((double)(cycle * 14 + (workDay - cycle) * 100)): MM/dd/yyyy}";
            }
            else
            {
                return $"{ refDate.AddDays((double)(cycle * 14 + (workDay - cycle) * 100)): MM/dd}";
            }
            
        }
    }
}
