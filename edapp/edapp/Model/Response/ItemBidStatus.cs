using System;
using System.Collections.Generic;
using Edapp.Data;

namespace Edapp.Model.Response
{
    public class ItemBidStatus
    {
        public bool CanCreateBid { get; set; }
        public string ErrorMessage { get; set; }
    }
}