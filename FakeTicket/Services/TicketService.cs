using FakeTicket.Infrastructure;
using FakeTicket.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace FakeTicket.Services
{
    public class TicketService
    {
        public TicketViewModel Generate(Ticket ticket)
        {
            var templatePath = HttpContext.Current.Server.MapPath($"/Content/TicketTemplate/template_{ticket.Template}.png");

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

            var fileName = $"{Guid.NewGuid()}.png";
            var filePath = HttpContext.Current.Server.MapPath($"/GeneratedTickets/{fileName}");
            bitmap.Save(filePath, ImageFormat.Png);

            return new TicketViewModel
            {
                FirstName = ticket.FirstName,
                LastName = ticket.LastName,
                Email = ticket.Email,
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
                GeneratedTicket = new GeneratedTicket
                {
                    FileName = fileName,
                    Path = filePath
                }
            };
        }

        public void SendEmail(Email email)
        {
            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();

            smtpClient.Host = Settings.SmtpHost;
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(Settings.GoogleAccount, Settings.GooglePassword); ;
            smtpClient.EnableSsl = true;
            smtpClient.Timeout = (60 * 5 * 1000);

            message.From = new MailAddress(Settings.GoogleAccount);
            message.Subject = email.Subject;
            message.IsBodyHtml = true;
            message.Body = email.Body;
            message.To.Add(email.EmailAddress);

            foreach (var attachment in email.Attachments)
            {
                System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
                contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;
                contentType.Name = attachment.Name;
                message.Attachments.Add(new Attachment(attachment.Path, contentType));
            }
            smtpClient.Send(message);
        }

        private Bitmap AddAirlineLogo(Bitmap bitmap, string airlineLogo)
        {
            if (!string.IsNullOrEmpty(airlineLogo))
            {
                var airlineLogoImage = HttpContext.Current.Server.MapPath($"/Content/Airlines/{airlineLogo}");
                Bitmap airlineBitmap = (Bitmap)Image.FromFile(airlineLogoImage); //load the image file

                Graphics gra = Graphics.FromImage(bitmap);
                gra.DrawImage(airlineBitmap, new Point(70, 70));
            }
            return bitmap;
        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmm");
        }
    }
}