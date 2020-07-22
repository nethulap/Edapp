using System;
using System.Collections.Generic;
using Edapp.Data;

namespace Edapp.Model.Request
{
    public class CreateBidRequest
    {
        public int Id { get; set; }
        public decimal BidAmount { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public DateTimeOffset BidCreatedDate { get; set; }
    }
}