namespace FakeTicket.Models
{
    public class TicketViewModel : Ticket
    {
        public GeneratedTicket GeneratedTicket { get; set; }
    }

    public class GeneratedTicket
    {
        public string FileName { get; set; }
        public string Path { get; set; }
    }
}