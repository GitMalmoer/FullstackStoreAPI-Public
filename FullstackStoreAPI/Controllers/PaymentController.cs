using System.Net;
using FullstackStoreAPI.Data;
using FullstackStoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace FullstackStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private ApiResponse _apiResponse;

        public PaymentController(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _apiResponse = new ApiResponse();
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> MakePayment(string userId)
        {
            ShoppingCart shoppingCart = await _dbContext.ShoppingCarts
                .Include(s => s.CartItems).ThenInclude(c => c.MenuItem)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (shoppingCart == null || shoppingCart.CartItems == null || shoppingCart.CartItems.Count == 0)
            {
                _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                _apiResponse.isSuccess = false;
                return BadRequest(_apiResponse);
            }

            #region Create Payment Intent
            StripeConfiguration.ApiKey = _configuration.GetValue<string>("StripeSettings:SecretKey");
            shoppingCart.CartTotal = shoppingCart.CartItems.Sum(sc => sc.Quantity * sc.MenuItem.Price);

            PaymentIntentCreateOptions options = new PaymentIntentCreateOptions
            {
                Amount = (int)(shoppingCart.CartTotal * 100),
                Currency = "usd",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            };
            PaymentIntentService service = new PaymentIntentService();
            PaymentIntent response = service.Create(options);

            shoppingCart.StripePaymentIntentId = response.Id;
            shoppingCart.ClientSecret = response.ClientSecret;

            #endregion

            _apiResponse.Result = shoppingCart;
            _apiResponse.isSuccess = true;
            _apiResponse.HttpStatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

    }
}
