using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Edapp.Model.Request;
using Edapp.Model.Response;
using Edapp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Edapp.Controllers
{
    [ApiController]
    [Route("api/item")]
    public class ItemController : ControllerBase
    {
        private readonly ILogger<ItemController> _logger;
        private readonly IItemRepository _itemRepository;
        private readonly IUserRepository _userRepository;


        public ItemController(ILogger<ItemController> logger, IItemRepository itemRepository, IUserRepository userRepository)
        {
            _logger = logger;
            _itemRepository = itemRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemResponse>>> GetItemsAsync()
        {
            var items = await _itemRepository.GetItemsAsync();
            return Ok(items);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ItemResponse>> GetItemAsync(int id)
        {
            var item = await _itemRepository.GetItemAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPut]
        [Route("{id}/bid")]
        public async Task<ActionResult<ItemResponse>> UpdateItemBidAsync([FromBody] CreateBidRequest createBidRequest, [FromHeader (Name = "user-id")] string userid, [FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(userid);
                var user = await _userRepository.GetUserAsync(userId);
                if (user == null)
                {
                    return Unauthorized();
                }

                var item = await _itemRepository.GetItemAsync(id);
                if (item == null)
                {
                    return NotFound();
                }

                var itemBidStatus = await _itemRepository.GetItemBidStatus(item.Id);
                if (itemBidStatus.CanCreateBid)
                {
                    createBidRequest.UserId = userId;
                    createBidRequest.BidCreatedDate = DateTime.Now;
                    createBidRequest.ItemId = item.Id;
                    var updatedItem = await _itemRepository.CreateBid(createBidRequest);
                    return Ok(updatedItem);
                }
                else
                {
                    return BadRequest(itemBidStatus.ErrorMessage);
                }
            }

            return BadRequest();
        }
    }
}
