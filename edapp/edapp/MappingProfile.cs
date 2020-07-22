using System;
using AutoMapper;
using Edapp.Data;
using Edapp.Model.Request;
using Edapp.Model.Response;

namespace Edapp
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<Item, ItemResponse>();
            CreateMap<CreateBidRequest, Bid>();
            CreateMap<Bid, BidResponse>();
        }
    }
}
