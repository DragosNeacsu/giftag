using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace FakeTicket.Controllers
{
    public class SkyScannerController : Controller
    {
        [HttpGet]
        public JsonResult GetDestination(string keyword)
        {

            var apiKey = "prtl6749387986743898559646983194";
            var market = "GB";
            var currency = "GBP";
            var locale = "en-GB";
            string url = $"http://partners.api.skyscanner.net/apiservices/autosuggest/v1.0/{market}/{currency}/{locale}/?query={keyword}&apiKey={apiKey}";
            WebRequest request = HttpWebRequest.Create(url);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string responseText = reader.ReadToEnd();

            return Json(responseText, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAirlines(string keyword)
        {
            var resultList = new List<dynamic>();

            var directoryPath = System.Web.HttpContext.Current.Server.MapPath($"/Content/Airlines/");
            var fileNames = Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories);
            foreach (var file in fileNames)
            {
                var fileName = file.Substring(file.LastIndexOf("\\") + 1);
                var airlineName = FormatFileName(fileName);
                if (airlineName.ToLower().Contains(keyword.ToLower()))
                {
                    resultList.Add(new
                    {
                        airlineName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(airlineName),
                        airlineLogo = file,
                        airlineCode = fileName
                    });
                }
            }

            return Json(resultList, JsonRequestBehavior.AllowGet);
        }

        private string FormatFileName(string airlineName)
        {
            airlineName = airlineName.Split('.')[0];
            airlineName = airlineName.Replace("-", " ");
            airlineName = airlineName.Replace("_", " ");

            return airlineName;
        }
    }
}