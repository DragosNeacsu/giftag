using FakeTicket.Infrastructure;
using FakeTicket.Models;
using FakeTicket.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace FakeTicket.Controllers
{
    public class PayPalController : BaseController
    {
        private TicketService _ticketService;

        public PayPalController()
        {
            _ticketService = new TicketService();
        }

        public void PayWithPaypal(Ticket ticket)
        {
            var generatedTicket = _ticketService.Generate(ticket);
            var custom = JsonConvert.SerializeObject(
                new
                {
                    email = generatedTicket.Email,
                    file = generatedTicket.GeneratedTicket.FileName
                });

            var price = 1;
            var builder = new StringBuilder();
            builder.Append(Settings.PaypalUrl);
            builder.Append("?cmd=_cart");
            builder.Append($"&business={Settings.PaypalEmail}");
            builder.Append("&upload=1");
            builder.Append("&item_name_1=fakeTicket");
            builder.Append("&item_number_1=p1");
            builder.Append($"&amount_1={price}");
            builder.Append("&quantity_1=1");
            builder.Append("&currency_code=GBP");
            builder.Append($"&custom={custom}");
            builder.Append($"&return=http://{Request.Url.Authority}/en-US/PayPal/Return");
            builder.Append($"&cancel_return=http://{Request.Url.Authority}/en-US/PayPal/Cancel");

            Response.Redirect(builder.ToString());
        }

        public void Return()
        {
            var query = $"cmd=_notify-synch&tx={Request.QueryString.Get("tx")}&at={Settings.PaypalAuthToken}";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Settings.PaypalUrl);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = query.Length;

            // Write the request back IPN strings
            var stream = req.GetRequestStream();
            var stOut = new StreamWriter(stream);
            stOut.Write(query);
            stOut.Close();

            // Do the request to PayPal and get the response
            StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream());
            var response = ProcessPaypalResponse(reader);

            var obj = JsonConvert.DeserializeObject<dynamic>(Uri.UnescapeDataString(response["custom"]));
            var email = new Email
            {
                EmailAddress = obj.email,
                Body = "to do",
                Subject = "Your fancy boarding pass",
                Attachments = new List<EmailAttachment>
                {
                    new EmailAttachment {
                        Name = "boarding_pass.png",
                        Path = System.Web.HttpContext.Current.Server.MapPath($"/GeneratedTickets/{obj.file}")
                    }
                }
            };
            _ticketService.SendEmail(email);
        }

        private Dictionary<string, string> ProcessPaypalResponse(StreamReader reader)
        {
            string line = reader.ReadLine();
            Dictionary<string, string> results = new Dictionary<string, string>();
            if (line == "SUCCESS")
            {
                while ((line = reader.ReadLine()) != null)
                {
                    var result = line.Split('=');
                    results.Add(result[0], result[1]);
                }
            }
            else if (line == "FAIL")
            {
                Response.Write("Unable to retrive transaction detail");
            }
            reader.Close();
            return results;
        }

        public void Cancel()
        {
            var asd = Request;
        }
    }
}