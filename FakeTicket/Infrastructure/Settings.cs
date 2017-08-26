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
        public static string PaypalUrl => ConfigurationManager.AppSettings["PayPal.Url"];
        public static string PaypalEmail => ConfigurationManager.AppSettings["PayPal.Email"];
        public static string PaypalAuthToken => ConfigurationManager.AppSettings["PayPal.AuthToken"];
        public static string GoogleAccount => ConfigurationManager.AppSettings["Google.Account"];
        public static string GooglePassword => ConfigurationManager.AppSettings["Google.Password"];
        public static string SmtpHost => ConfigurationManager.AppSettings["Smtp.Host"];
    }
}