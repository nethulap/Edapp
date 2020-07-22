using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Edapp.Data;
using Edapp.Model.Request;
using Edapp.Model.Response;
using Microsoft.EntityFrameworkCore;

namespace Edapp.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly EdappContext _edappContext;
        private readonly IMapper _mapper;
        private const string AUCTION_END_DATE_EXPIRED = "Auction end date is expired";
        private const string ITEM_NOT_FOUND = "Item not found";

        public ItemRepository(EdappContext edappContext, IMapper mapper)
        {
            _edappContext = edappContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItemResponse>> GetItemsAsync()
        {
            var itemsEntityList = await _edappContext.Item.ToListAsync();
            var items = _mapper.Map<List<ItemResponse>>(itemsEntityList);
            return items;
        }

        public async Task<ItemResponse> GetItemAsync(int id)
        {
            var itemEntity = await _edappContext.Item.FirstOrDefaultAsync(u => u.Id == id);
            var item = _mapper.Map<ItemResponse>(itemEntity);
            return item;
        }

        public async Task<ItemResponse> UpdateItemAsync(ItemRequest itemRequest)
        {
            var itemEntity = await _edappContext.Item.FindAsync(itemRequest.Id);
            itemEntity.ItemStatus = ItemStatus.Sold;
            var result = _edappContext.Item.Update(itemEntity);
            await _edappContext.SaveChangesAsync();
            var item = _mapper.Map<ItemResponse>(itemEntity);
            return item;
        }

        public async Task<ItemBidStatus> GetItemBidStatus(int id)
        {
            var itemEntity = await _edappContext.Item.FindAsync(id);
            var itemBidStatus = new ItemBidStatus();
            if (itemEntity != null)
            {
                if (itemEntity.AuctionEndDate > DateTime.Now)
                {
                    itemBidStatus.CanCreateBid = true;
                    itemBidStatus.ErrorMessage = string.Empty;
                }
                else
                {
                    itemBidStatus.CanCreateBid = false;
                    itemBidStatus.ErrorMessage = AUCTION_END_DATE_EXPIRED;
                }
            }
            else
            {
                itemBidStatus.CanCreateBid = false;
                itemBidStatus.ErrorMessage = ITEM_NOT_FOUND;
            }

            return itemBidStatus;
        }

        public async Task<ItemResponse> CreateBid(CreateBidRequest createBidRequest)
        {
            var bidEntity = _mapper.Map<Bid>(createBidRequest);
            await _edappContext.Bid.AddAsync(bidEntity);
            await _edappContext.SaveChangesAsync();           
            var itemEntity = await _edappContext.Item.FindAsync(createBidRequest.ItemId);
            var item = _mapper.Map<ItemResponse>(itemEntity);
            var bids = await _edappContext.Bid.Where(b => b.ItemId == itemEntity.Id).ToListAsync();
            item.Bids = _mapper.Map<List<BidResponse>>(bids);
            return item;
        }

        public async Task<IEnumerable<Bid>> GetBids(int itemId)
        {
             var bids = await _edappContext.Bid.Where(b => b.ItemId == itemId).ToListAsync();
            return bids;
        }
    }
}