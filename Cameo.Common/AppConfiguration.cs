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
        public string ApplicationRootPath { get; set; }
        public string NoPhotoUrl { get; set; }
        public string SpinnerUrl { get; set; }
        public int PaymentSystemCommission { get; set; }
        public string NumberStringFormat { get; set; }
    }
}
