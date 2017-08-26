using FakeTicket.Models;
using FakeTicket.Services;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FakeTicket.Controllers
{
    public class TicketController : BaseController
    {
        private TicketService _ticketService;
        public TicketController()
        {
            _ticketService = new TicketService();
        }

        [HttpPost]
        public ActionResult Generate(Ticket ticket)
        {
            var generatedTicket = _ticketService.Generate(ticket);
            var email = new Email
            {
                EmailAddress = generatedTicket.Email,
                Body = "to do",
                Subject = "Your fancy boarding pass",
                Attachments = new List<EmailAttachment>
                {
                    new EmailAttachment {
                        Name = "boarding_pass.png",
                        Path = System.Web.HttpContext.Current.Server.MapPath($"/GeneratedTickets/{generatedTicket.GeneratedTicket.FileName}")
                    }
                }
            };
            _ticketService.SendEmail(email);

            return View("Index", generatedTicket);
        }

        public ActionResult Get(string path)
        {
            return File(path, "image/png");
        }
    }
}
