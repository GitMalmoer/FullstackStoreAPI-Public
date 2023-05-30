using System.Net;
using FullstackStoreAPI.Data;
using FullstackStoreAPI.Models;
using FullstackStoreAPI.Models.DTO;
using FullstackStoreAPI.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullstackStoreAPI.Controllers
{
    [Route("api/Order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private ApiResponse _apiResponse;

        public OrderController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _apiResponse = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetOrders(string? userId)
        {
            try
            {
                var orderHeaders = _dbContext.OrderHeaders
                    .Include(o => o.OrderDetails).ThenInclude(o => o.MenuItem)
                    .OrderByDescending(o => o.OrderHeaderId);

                if (!string.IsNullOrEmpty(userId))
                {
                    _apiResponse.Result = orderHeaders.Where(o => o.ApplicationUserId == userId);
                }
                else
                {
                    _apiResponse.Result = orderHeaders;
                }

                _apiResponse.HttpStatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception e)
            {
                _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                _apiResponse.isSuccess = false;
                _apiResponse.ErrorMessages.Add(e.ToString());
            }

            return _apiResponse;
        }

        [HttpGet("{orderId:int}")]
        public async Task<ActionResult<ApiResponse>> GetOrder(int orderId)
        {
            try
            {
                if (orderId == 0)
                {
                    _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.isSuccess = false;
                    _apiResponse.ErrorMessages.Add("Id cannot be 0");
                    return BadRequest(_apiResponse);
                }

                var orderHeaders = _dbContext.OrderHeaders.Include(u => u.OrderDetails)
                    .ThenInclude(u => u.MenuItem)
                    .Where(u => u.OrderHeaderId == orderId);

                if (orderHeaders == null)
                {
                    _apiResponse.HttpStatusCode = HttpStatusCode.NotFound;
                    return NotFound(_apiResponse);
                }


                _apiResponse.HttpStatusCode = HttpStatusCode.OK;
                _apiResponse.Result = orderHeaders;
                return Ok(_apiResponse);
            }
            catch (Exception e)
            {
                _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                _apiResponse.isSuccess = false;
                _apiResponse.ErrorMessages.Add(e.ToString());
            }

            return _apiResponse;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateOrder([FromBody] OrderHeaderCreateDTO orderHeaderDto)
        {
            try
            {
                OrderHeader order = new OrderHeader()
                {
                    ApplicationUserId = orderHeaderDto.ApplicationUserId,
                    PickupName = orderHeaderDto.PickupName,
                    PickupEmail = orderHeaderDto.PickupEmail,
                    PickupPhoneNumber = orderHeaderDto.PickupPhoneNumber,
                    OrderTotal = orderHeaderDto.OrderTotal,
                    OrderDate = DateTime.Now,
                    StripePaymentIntentId = orderHeaderDto.StripePaymentIntentId,
                    TotalItems = orderHeaderDto.TotalItems,
                    Status = string.IsNullOrEmpty(orderHeaderDto.Status) ? SD.status_pending : orderHeaderDto.Status,
                };

                if (ModelState.IsValid)
                {
                    _dbContext.OrderHeaders.Add(order);
                    _dbContext.SaveChanges();

                    foreach (var orderDetailDTO in orderHeaderDto.OrderDetailsDTO)
                    {
                        OrderDetails orderDetails = new()
                        {
                            OrderHeaderId = order.OrderHeaderId,
                            MenuItemId = orderDetailDTO.MenuItemId,
                            Price = orderDetailDTO.Price,
                            Quantity = orderDetailDTO.Quantity,
                            ItemName = orderDetailDTO.ItemName,
                        };
                        _dbContext.OrderDetails.Add(orderDetails);
                    }

                    _dbContext.SaveChanges();
                    _apiResponse.Result = order;
                    order.OrderDetails = null; // passed by reference
                    _apiResponse.HttpStatusCode = HttpStatusCode.Created;

                    return Ok(_apiResponse);
                }

            }
            catch (Exception e)
            {
                _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add(e.ToString());
                _apiResponse.isSuccess = false;
                return BadRequest(_apiResponse);
            }

            return _apiResponse;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> updateOrderHeader(int id, [FromBody] OrderHeaderUpdateDTO orderHeaderUpdate)
        {
            try
            {
                OrderHeader orderHeader = await _dbContext.OrderHeaders.FindAsync(id);

                if (orderHeader == null || orderHeaderUpdate == null || id != orderHeaderUpdate.OrderHeaderId)
                {
                    _apiResponse.isSuccess = false;
                    _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                if (!string.IsNullOrEmpty(orderHeaderUpdate.PickupName))
                {
                    orderHeader.PickupName = orderHeaderUpdate.PickupName;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdate.PickupEmail))
                {
                    orderHeader.PickupEmail = orderHeaderUpdate.PickupEmail;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdate.PickupPhoneNumber))
                {
                    orderHeader.PickupPhoneNumber = orderHeaderUpdate.PickupPhoneNumber;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdate.Status))
                {
                    orderHeader.Status = orderHeaderUpdate.Status;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdate.StripePaymentIntentId))
                {
                    orderHeader.StripePaymentIntentId = orderHeaderUpdate.StripePaymentIntentId;
                }

                _dbContext.SaveChanges();
                _apiResponse.Result = orderHeader;
                _apiResponse.isSuccess = true;
                _apiResponse.HttpStatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception e)
            {
                _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add(e.ToString());
                _apiResponse.isSuccess = false;
                return BadRequest(_apiResponse);
            }

            return _apiResponse;

        }

    }
}
