using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cameo.Common
{
    public static class ConfigurationHelper
    {
        public static string GetSetting(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (name.Length == 0)
                throw new ArgumentException($"Error argument \"{nameof(name)}\" is an empty string");

            string result = null;

            // Chech in configuration file
            result = ConfigurationManager.AppSettings[name];
            if (result != null)
                return result;

            return result;
        }
    }
}
