using System;

namespace Facade.Task1.OrderPlacement
{
    public class Invoice
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string CustomerEmail { get; set; }
        public Guid InvoiceNumber { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
