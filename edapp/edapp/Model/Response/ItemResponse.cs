using System;
using System.Collections.Generic;
using Edapp.Data;

namespace Edapp.Model.Response
{
    public class ItemResponse
    {
        public int Id { get; set; }
        public int ItemNumber { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset AuctionStartDate { get; set; }
        public DateTimeOffset AuctionEndDate { get; set; }
        public List<BidResponse> Bids { get; set; }
    }
}