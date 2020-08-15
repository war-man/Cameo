using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cameo.Models;
using Cameo.Data;
using Microsoft.AspNetCore.Diagnostics;
using Hangfire;
using Microsoft.Extensions.Logging;

using RestSharp;
using RestSharp.Authenticators;
using System.Net.Http;
using System.Text;
using System.Net;
using System.IO;

namespace Cameo.Controllers
{
    public class HomeController : BaseController
    {
        readonly private IBackgroundJobClient _backgroundJobs;

        public HomeController(IBackgroundJobClient backgroundJobs, ILogger<HomeController> logger)
        {
            _backgroundJobs = backgroundJobs;
            _logger = logger;
        }

        public IActionResult Index(int? id)
        {
            ////_backgroundJobs.Enqueue(() => Console.WriteLine("AAAAAA!"));

            ////_logger.LogInformation("Home Index page opened");
            //int k = 6;
            //int l = k / 0;
            throw new Exception("Талант не найден");
            try
            {
                throw new Exception("Талант не найден");
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
            

            //return NotFound();

            ////var client = new RestClient("http://ac1cb2.wiut.uz/BaseWIUT/hs/epaymentswiut/findeinfo/t1/00007045");
            //////client.Authenticator = new HttpBasicAuthenticator("webuser", "1c$web$Rv9");
            ////client.AddDefaultHeader("Authorization", "Basic d2VidXNlcjoxYyR3ZWIkUnY5");

            ////var request = new RestRequest("BaseWIUT/hs/epaymentswiut/findeinfo/t1/00007045", DataFormat.Json);
            //////request.AddHeader("Authorization", "Basic d2VidXNlcjoxYyR3ZWIkUnY5");

            ////var response = client.Get(request);


            //var clientt = new HttpClient();
            //clientt.BaseAddress = new Uri("http://localhost:60464/api/");

            //var authenticationString = $"webuser:1c$web$Rv9";
            //var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(authenticationString));
            //var requestMessage = new HttpRequestMessage(HttpMethod.Get, "BaseWIUT/hs/epaymentswiut/findeinfo/t1/00007045");
            ////requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

            //var values = new List<KeyValuePair<string, string>>();
            //values.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            //var content = new FormUrlEncodedContent(values);

            //content.Headers.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
            //requestMessage.Content = content;
            ////HTTP GET
            //var task = clientt.SendAsync(requestMessage);
            //var response = task.Result;
            //response.EnsureSuccessStatusCode();
            //string responseBody = response.Content.ReadAsStringAsync().Result;

            ////var responseTask = clientt.GetAsync("BaseWIUT/hs/epaymentswiut/findeinfo/t1/00007045");
            ////responseTask.Wait();
            ////var result = responseTask.Result;
            ///

            //HttpClient client = new HttpClient();
            //var byteArray = Encoding.ASCII.GetBytes("webuser:1c$web$Rv9");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            //HttpResponseMessage response = await client.GetAsync("http://ac1cb2.wiut.uz/BaseWIUT/hs/epaymentswiut/findeinfo/t1/00007045");
            //HttpContent content = response.Content;

            //// ... Check Status Code                                
            //Console.WriteLine("Response StatusCode: " + (int)response.StatusCode);

            //// ... Read the string.
            //string result = await content.ReadAsStringAsync();

            string host = "http://ac1cb2.wiut.uz";
            string port = "";
            string relativeUrl = "/BaseWIUT/hs/epaymentswiut/findeinfo/t1/00007045";

            HttpWebResponse response = MakeBasicAuthorizedApiRequest(host, port, relativeUrl, "get", "webuser", "1c$web$Rv9", null, 600);
            var reader = new StreamReader(response.GetResponseStream());
            var objText = reader.ReadToEnd();




            var curUser = accountUtil.GetCurrentUser(User);

            return View();
        }

        public HttpWebResponse MakeBasicAuthorizedApiRequest(string host, string port, string relativeUrl, string method, string username, string password, string postData = null, int timeoutSeconds = 0)
        {
            string endpoint = "";
            if (!string.IsNullOrWhiteSpace(host))
                endpoint += host;
            if (!string.IsNullOrWhiteSpace(port))
                endpoint += ":" + port;

            string url = endpoint + relativeUrl;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;


            string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
            request.Headers.Add("Authorization", "Basic " + encoded);

            if (timeoutSeconds > 0)
                request.Timeout = timeoutSeconds * 1000;
            request.Accept = "application/json";
            //request.ContentType = "application/json";
            request.ContentType = "application/x-www-form-urlencoded";

            if (!string.IsNullOrWhiteSpace(postData))
            {
                var data = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            else
                request.ContentLength = 0;

            return (HttpWebResponse)request.GetResponse();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }


        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            //throw new Exception("This is some thrown exception");
            return View();
        }

        public IActionResult SecurityAndPrivacy()
        {
            return View();
        }

        public IActionResult TermsOfService()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    var pathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        //    Exception exception = pathFeature?.Error; // Here will be the exception details

        //    var curUser = accountUtil.GetCurrentUser(User);
        //    _logger.LogError(exception, "UserID = " + curUser?.ID ?? "unauthorized");

        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
