using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cameo.Common
{
    public static class AppConfiguration
    {
        public static string AmazonESDomenEndpoint
        {
            get
            {
                return ConfigurationHelper.GetSetting("AmazonESDomenEndpoint");
            }
        }
        
    }
}
