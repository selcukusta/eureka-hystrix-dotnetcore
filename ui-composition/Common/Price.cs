using System;

namespace Common
{
    public class Price
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
