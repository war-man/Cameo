using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cameo.Common
{
    public static class AppData
    {
        public static AppConfiguration Configuration;
    }

    public class AppConfiguration
    {
        public string UploadsPath { get; set; }
        public string NoPhotoUrl { get; set; }
        public string SpinnerUrl { get; set; }
        public int WebsiteCommission { get; set; }
        public int PaymentSystemCommission { get; set; }
        public string NumberViewStringFormat { get; set; }
        public string DateViewStringFormat { get; set; }
        public string DateTextViewStringFormat { get; set; }
        public string DateDbViewStringFormat { get; set; }
        public string TimeViewStringFormat { get; set; }

        public string TokenValidAudience { get; set; }
        public string TokenValidIssuer { get; set; }
        public string TokenSecurityKey { get; set; }
        public int TokenExpirationPeriodInDays { get; set; }
    }
}
