using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FakeTicket.Infrastructure
{
    public class Settings
    {
        public static string SkyScannerUrl => ConfigurationManager.AppSettings["SkyScanner.Url"];
        public static string SkyScannerApiKey => ConfigurationManager.AppSettings["SkyScanner.ApiKey"];
    }
}