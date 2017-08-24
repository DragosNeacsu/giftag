using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace FakeTicket.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}