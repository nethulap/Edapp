using System;
using System.Collections.Generic;
using Edapp.Data;

namespace Edapp.Model.Request
{
    public class ItemRequest
    {
        public int Id { get; set; }
        public ItemStatus ItemStatus { get; set; }
    }
}