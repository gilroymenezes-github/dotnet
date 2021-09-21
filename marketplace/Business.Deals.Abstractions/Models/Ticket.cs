using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Deals.Abstractions.Models
{
    public class Ticket : Inventory
    {
        public string TicketId { get; set; }
        public string CustomerId { get; set; }
        public string LocationId { get; set; }
        public DateTime BeginTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
        public TimeSpan ShowDuration { get; set; }
        public string AssignedSeat { get; set; }
        public string AssignedHall { get; set; }
        public string AssignedName { get; set; }

    }
}
