using System;
using System.Collections.Generic;

namespace Edapp.Data
{
    public class Item
    {
        public int Id { get; set; }
        public int ItemNumber { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset AuctionStartDate { get; set; }
        public DateTimeOffset AuctionEndDate { get; set; }
        public ItemStatus ItemStatus { get; set; }
    }
}