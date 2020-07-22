using System.Collections.Generic;
using System.Threading.Tasks;
using Edapp.Data;
using Edapp.Model.Request;
using Edapp.Model.Response;

namespace Edapp.Repository
{
    public interface IItemRepository
    {
        Task<IEnumerable<ItemResponse>> GetItemsAsync();
        Task<ItemResponse> GetItemAsync(int id);
        Task<ItemBidStatus> GetItemBidStatus(int id);
        Task<ItemResponse> CreateBid(CreateBidRequest createBidRequest);
        Task<ItemResponse> UpdateItemAsync(ItemRequest itemRequest);
    }
}