using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Edapp.Controllers;
using Edapp.Data;
using Edapp.Model.Request;
using Edapp.Model.Response;
using Edapp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace edapp.tests
{
    public class ItemRepositoryTests
    {
        [Fact]
        public async Task GetItemsAsyncReturnsItemResponseListAsync()
        {
            var mockLogger = new Mock<ILogger<ItemController>>();
            var items = GetMockItems();
            var user = GetMockUser();
            var itemRepository = new Mock<IItemRepository>();
            itemRepository.Setup(u => u.GetItemsAsync()).Returns(Task.FromResult(items));
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.GetUserAsync(It.IsAny<int>())).Returns(Task.FromResult(user));
            var itemController = new ItemController(mockLogger.Object, itemRepository.Object, userRepository.Object);
            var controller = await itemController.GetItemsAsync();
            var actionResult = Assert.IsType<OkObjectResult>(controller.Result);
            var result = actionResult.Value as List<ItemResponse>;

            Assert.Equal(5, result.Count);
        }

        [Fact]
        public async Task GetItemAsyncReturnsItemResponse()
        {
            var mockLogger = new Mock<ILogger<ItemController>>();
            var item = GetMockItem();
            var user = GetMockUser();
            var itemRepository = new Mock<IItemRepository>();
            itemRepository.Setup(u => u.GetItemAsync(It.IsAny<int>())).Returns(Task.FromResult(item));
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.GetUserAsync(It.IsAny<int>())).Returns(Task.FromResult(user));
            var itemController = new ItemController(mockLogger.Object, itemRepository.Object, userRepository.Object);
            var controller = await itemController.GetItemAsync(1);
            var actionResult = Assert.IsType<OkObjectResult>(controller.Result);
            var result = actionResult.Value as ItemResponse;

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(2345, result.ItemNumber);
            Assert.Equal("Iphone X 250 GB Space Gray", result.Description);
        }

        [Fact]
        public async Task GetItemAsyncThrowsNotFoundException()
        {
            var mockLogger = new Mock<ILogger<ItemController>>();
            ItemResponse item = null;
            var user = GetMockUser();
            var itemRepository = new Mock<IItemRepository>();
            itemRepository.Setup(u => u.GetItemAsync(It.IsAny<int>())).Returns(Task.FromResult(item));
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.GetUserAsync(It.IsAny<int>())).Returns(Task.FromResult(user));
            var itemController = new ItemController(mockLogger.Object, itemRepository.Object, userRepository.Object);
            var controller = await itemController.GetItemAsync(1);
            var result = Assert.IsType<NotFoundResult>(controller.Result);
            Assert.Equal(404, result.StatusCode);
        } 

        [Fact]
        public async Task UpdateItemBidAsyncReturnsItemResponse()
        {
            var mockLogger = new Mock<ILogger<ItemController>>();
            var item = GetMockItemBid();
            var user = GetMockUser();
            var createBidRequest = new CreateBidRequest();
            createBidRequest.BidAmount = 450;
            var itemBidStatus = new ItemBidStatus();
            itemBidStatus.CanCreateBid = true;
            itemBidStatus.ErrorMessage = string.Empty;
            var itemRepository = new Mock<IItemRepository>();
            itemRepository.Setup(t => t.GetItemAsync(It.IsAny<int>())).Returns(Task.FromResult(item));
            itemRepository.Setup(u => u.GetItemBidStatus(It.IsAny<int>())).Returns(Task.FromResult(itemBidStatus));
            itemRepository.Setup(s => s.CreateBid(It.IsAny<CreateBidRequest>())).Returns(Task.FromResult(item));

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.GetUserAsync(It.IsAny<int>())).Returns(Task.FromResult(user));
            var itemController = new ItemController(mockLogger.Object, itemRepository.Object, userRepository.Object);
            var controller = await itemController.UpdateItemBidAsync(createBidRequest, "1", 2);
            var actionResult = Assert.IsType<OkObjectResult>(controller.Result);
            var result = actionResult.Value as ItemResponse;

            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
            Assert.Equal(2346, result.ItemNumber);
            Assert.Equal("Iphone X 512 GB White", result.Description);
            Assert.Single(result.Bids);
            Assert.Equal(450, result.Bids[0].BidAmount);
        }

        [Fact]
        public async Task UpdateItemBidAsyncThowsUnauthorisedException()
        {
            var mockLogger = new Mock<ILogger<ItemController>>();
            ItemResponse item = null;
            UserResponse user = null;
            var createBidRequest = new CreateBidRequest();
            createBidRequest.BidAmount = 450;
            var itemRepository = new Mock<IItemRepository>();
            itemRepository.Setup(t => t.GetItemAsync(It.IsAny<int>())).Returns(Task.FromResult(item));
            
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.GetUserAsync(It.IsAny<int>())).Returns(Task.FromResult(user));
            var itemController = new ItemController(mockLogger.Object, itemRepository.Object, userRepository.Object);
            var controller = await itemController.UpdateItemBidAsync(createBidRequest, "1", 2);
            var result = Assert.IsType<UnauthorizedResult>(controller.Result);
            Assert.Equal(401, result.StatusCode);
        }

        [Fact]
        public async Task UpdateItemBidAsyncThowsNotFoundException()
        {
            var mockLogger = new Mock<ILogger<ItemController>>();
            ItemResponse item = null;
            var user = GetMockUser();
            var createBidRequest = new CreateBidRequest();
            createBidRequest.BidAmount = 450;
            var itemRepository = new Mock<IItemRepository>();
            itemRepository.Setup(t => t.GetItemAsync(It.IsAny<int>())).Returns(Task.FromResult(item));
            
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.GetUserAsync(It.IsAny<int>())).Returns(Task.FromResult(user));
            var itemController = new ItemController(mockLogger.Object, itemRepository.Object, userRepository.Object);
            var controller = await itemController.UpdateItemBidAsync(createBidRequest, "1", 2);
            var result = Assert.IsType<NotFoundResult>(controller.Result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task UpdateItemBidAsyncThowsBadRequestException()
        {
            var mockLogger = new Mock<ILogger<ItemController>>();
            var item = GetMockItemBid();
            var user = GetMockUser();
            var createBidRequest = new CreateBidRequest();
            createBidRequest.BidAmount = 450;
            var itemBidStatus = new ItemBidStatus();
            itemBidStatus.CanCreateBid = false;
            itemBidStatus.ErrorMessage = "Auction End Date expired";
            var itemRepository = new Mock<IItemRepository>();
            itemRepository.Setup(t => t.GetItemAsync(It.IsAny<int>())).Returns(Task.FromResult(item));
            itemRepository.Setup(u => u.GetItemBidStatus(It.IsAny<int>())).Returns(Task.FromResult(itemBidStatus));
            
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.GetUserAsync(It.IsAny<int>())).Returns(Task.FromResult(user));
            var itemController = new ItemController(mockLogger.Object, itemRepository.Object, userRepository.Object);
            var controller = await itemController.UpdateItemBidAsync(createBidRequest, "1", 2);
            var result = Assert.IsType<BadRequestObjectResult>(controller.Result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Auction End Date expired", result.Value);
        }

        private ItemResponse GetMockItem()
        {
            var item = new ItemResponse();
            item.Id = 1;
            item.ItemNumber = 2345;
            item.Description = "Iphone X 250 GB Space Gray";
            item.CreatedDate = DateTime.Now;
            item.AuctionStartDate = DateTime.Now;
            item.AuctionEndDate = DateTime.Today.AddDays(1);

            return item;
        }

        private ItemResponse GetMockItemBid()
        {
            var bid = new BidResponse();
            var item = new ItemResponse();
            
            item.Id = 2;
            item.ItemNumber = 2346;
            item.Description = "Iphone X 512 GB White";
            item.CreatedDate = DateTime.Now;
            item.AuctionStartDate = DateTime.Now;
            item.AuctionEndDate = DateTime.Today.AddDays(1);
            
            item.Bids = new List<BidResponse>();
            bid.Id = 1;
            bid.BidAmount = 450;
            bid.UserId = 1;
            bid.ItemId = 2;
            bid.BidCreatedDate = DateTime.Now;
            item.Bids.Add(bid);

            return item;
        }

        private UserResponse GetMockUser()
        {
            var user = new UserResponse();
            user.Id = 1;
            user.Name = "Ralph";
            user.Email = "ralph@gmail.com";
            user.Phone = "+61412367895";

            return user;
        }

        private IEnumerable<ItemResponse> GetMockItems()
        {
            var items = new List<ItemResponse>();
            var item1 = new ItemResponse
            {
                Id = 1,
                ItemNumber = 2345,
                Description = "Iphone X 250 GB Space Gray",
                CreatedDate = DateTime.Now,
                AuctionStartDate = DateTime.Now,
                AuctionEndDate = DateTime.Today.AddDays(1)
            };

            var item2 = new ItemResponse
            {
                Id = 2,
                ItemNumber = 2346,
                Description = "Iphone X 512 GB White",
                CreatedDate = DateTime.Now,
                AuctionStartDate = DateTime.Now,
                AuctionEndDate = DateTime.Today.AddDays(1)
            };

            var item3 = new ItemResponse
            {
                Id = 3,
                ItemNumber = 2347,
                Description = "Iphone X 64 GB Gray",
                CreatedDate = DateTime.Now,
                AuctionStartDate = DateTime.Now,
                AuctionEndDate = DateTime.Today.AddDays(1)
            };

            var item4 = new ItemResponse
            {
                Id = 4,
                ItemNumber = 2348,
                Description = "Iphone X 128 GB Red",
                CreatedDate = DateTime.Now,
                AuctionStartDate = DateTime.Now,
                AuctionEndDate = DateTime.Today.AddDays(1)
            };

            var item5 = new ItemResponse
            {
                Id = 5,
                ItemNumber = 2349,
                Description = "Iphone X 250 GB",
                CreatedDate = DateTime.Now,
                AuctionStartDate = DateTime.Now,
                AuctionEndDate = DateTime.Today.AddDays(1)
            };

            items.Add(item1);
            items.Add(item2);
            items.Add(item3);
            items.Add(item4);
            items.Add(item5);

            return items;
        }
    }
}
