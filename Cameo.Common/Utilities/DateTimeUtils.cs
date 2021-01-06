using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cameo.Common.Utilities
{
    public static class DateTimeUtils
    {
        /// <summary>
        /// if unable to convert then return DateTime.MinValue
        /// </summary>
        /// <param name="inputDatetimeString"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(string inputDatetimeString, string format = null)
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-GB");

            if (string.IsNullOrWhiteSpace(format))
                format = AppData.Configuration.DateViewStringFormat; //"dd.MM.yyyy"

            DateTime result = !string.IsNullOrWhiteSpace(inputDatetimeString) 
                ? DateTime.ParseExact(inputDatetimeString, format, cultureInfo) : DateTime.MinValue;

            return result;
        }

        public static string ConvertToString(DateTime? inputDateTime, string format = null)
        {
            if (!inputDateTime.HasValue)
                return null;

            if (string.IsNullOrWhiteSpace(format))
                format = AppData.Configuration.DateViewStringFormat; //"dd.MM.yyyy"

            string result = inputDateTime.Value.ToString(format, CultureInfo.InvariantCulture);

            return result;
        }
    }
}
