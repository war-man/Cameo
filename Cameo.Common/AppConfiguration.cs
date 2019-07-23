using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cameo.Common
{
    /*public static class AppConfiguration
    {
        public static string UploadsPath
        {
            get
            {
                return ConfigurationHelper.GetSetting("UploadsPath");
            }
        }
        
    }*/

    public class AppConfiguration
    {
        public string UploadsPath { get; set; }
        public string ApplicationRootPath { get; set; }
        public string NophotoUrl { get; set; }
        public string SpinnerUrl { get; set; }
    }
}
