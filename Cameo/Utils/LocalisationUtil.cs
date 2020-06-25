using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cameo.Utils
{
    public class LocalisationUtil
    {
        public static List<Languages> AvailableLanguages = new List<Languages>
        {
            new Languages{ LangFullName = "Русский", LangCultureName = "ru"},
            new Languages{ LangFullName = "O'zbek", LangCultureName = "uz"},
            new Languages{ LangFullName = "Ўзбек", LangCultureName = "kk"} //"kk" (казахский) будем использовать для узбекского языка на кириллице
        };

        public static bool IsLanguageAvailable(string lang)
        {
            return AvailableLanguages.Where(a => a.LangCultureName.Equals(lang)).FirstOrDefault() != null ? true : false;
        }

        public static string GetDefaultLanguage()
        {
            return AvailableLanguages[0].LangCultureName;
        }

        public void SetLanguage(string lang, HttpResponse response)
        {
            try
            {
                if (!IsLanguageAvailable(lang))
                    lang = GetDefaultLanguage();
                var cultureInfo = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);

                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddYears(1);
                response.Cookies.Append("culture", lang, option);

                //HttpCookie langCookie = new HttpCookie("culture", lang);
                //langCookie.Expires = DateTime.Now.AddYears(1);
                //HttpContext.Current.Response.Cookies.Add(langCookie);

            }
            catch (Exception ex)
            {

            }
        }
    }

    public class Languages
    {
        public string LangFullName { get; set; }
        public string LangCultureName { get; set; }
    }
}
