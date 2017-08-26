using System.Collections.Generic;

namespace FakeTicket.Services
{
    public class Email
    {
        public IEnumerable<EmailAttachment> Attachments { get; set; }
        public string Body { get; set; }
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
    }

    public class EmailAttachment
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}