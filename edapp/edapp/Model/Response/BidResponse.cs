using System;

namespace Edapp.Model.Response
{
    public class BidResponse
    {
        public int Id { get; set; }
        public decimal BidAmount { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public DateTimeOffset BidCreatedDate { get; set; }
    }
}