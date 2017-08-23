using FakeTicket.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Mvc;

namespace FakeTicket.Controllers
{
    public class TicketController : BaseController
    {
        [HttpPost]
        public ActionResult Generate(Ticket ticket)
        {
            var templatePath = System.Web.HttpContext.Current.Server.MapPath(@"/Content/TicketTemplate/BoardingPass.PNG");

            Bitmap bitmap = (Bitmap)Image.FromFile(templatePath);//load the image file

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                using (Font arialFont = new Font("Arial", 10))
                {
                    graphics.DrawString(ticket.FirstName, arialFont, Brushes.Black, new PointF(500f, 10f));
                    graphics.DrawString(ticket.LastName, arialFont, Brushes.Black, new PointF(500f, 50f));
                    graphics.DrawString(ticket.From, arialFont, Brushes.Black, new PointF(500f, 100f));
                    graphics.DrawString(ticket.To, arialFont, Brushes.Black, new PointF(500f, 150f));
                }
            }

            bitmap = AddAirlineLogo(bitmap, ticket.AirlineLogo);

            var savePath = System.Web.HttpContext.Current.Server.MapPath(@"/GeneratedTickets/" + ticket.FirstName + ticket.LastName + "_" + GetTimestamp(DateTime.Now) + ".png");
            bitmap.Save(savePath, ImageFormat.Png);

            var generatedTicket = new TicketViewModel
            {
                FirstName = ticket.FirstName,
                LastName = ticket.LastName,
                Airline = ticket.Airline,
                AirlineLogo = ticket.AirlineLogo,
                BoardingTime = ticket.BoardingTime,
                Class = ticket.Class,
                FlightDate = ticket.FlightDate,
                FlightNumber = ticket.FlightNumber,
                From = ticket.From,
                Gate = ticket.Gate,
                Language = ticket.Language,
                To = ticket.To,
                Seat = ticket.Seat,
                GeneratedTicketPath = savePath
            };

            return View("Index", generatedTicket);
        }

        private Bitmap AddAirlineLogo(Bitmap bitmap, string airlineLogo)
        {
            var airlineLogoImage = System.Web.HttpContext.Current.Server.MapPath($"/Content/Airlines/{airlineLogo}");
            Bitmap airlineBitmap = (Bitmap)Image.FromFile(airlineLogoImage);//load the image file

            Graphics gra = Graphics.FromImage(bitmap);
            gra.DrawImage(airlineBitmap, new Point(70, 70));
            return bitmap;
        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmm");
        }

        public ActionResult Get(string path)
        {

            return File(path, "image/png");
        }
    }
}
