using System;
using System.Collections.Generic;
using System.Text;

namespace Cameo.Services
{
    public static class CommonService
    {
        public static string GenerateRandomNumerics(int length)
        {
            StringBuilder result = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int randomNumeric = random.Next(0, 10);
                result.Append(randomNumeric);
            }

            return result.ToString();
        }
    }
}
