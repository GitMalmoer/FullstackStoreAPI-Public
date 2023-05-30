using System.Net;
using FullstackStoreAPI.Data;
using FullstackStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullstackStoreAPI.Controllers
{
    [ApiController]
    [Route("api/ShoppingCart")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private ApiResponse _apiResponse;

        public ShoppingCartController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _apiResponse = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetShoppingCart(string userId)
        {
            try
            {
                ShoppingCart userShoppingCart;

                if (string.IsNullOrEmpty(userId))
                {
                    userShoppingCart = new();
                }
                else
                {
                    userShoppingCart = _dbContext
                        .ShoppingCarts
                        .Include(u => u.CartItems).ThenInclude(u => u.MenuItem)
                        .FirstOrDefault(x => x.UserId == userId);
                }

                if (userShoppingCart == null)
                {
                    _apiResponse.ErrorMessages.Add("COULD NOT FIND SHOPPING CART WITH THAT USER ID!");
                    _apiResponse.isSuccess = false;
                    return BadRequest(_apiResponse);
                }


                if (userShoppingCart.CartItems != null && userShoppingCart.CartItems.Count > 0)
                {
                    userShoppingCart.CartTotal = userShoppingCart.CartItems.Sum(u => u.Quantity * u.MenuItem.Price);
                }

                _apiResponse.Result = userShoppingCart;
                _apiResponse.isSuccess = true;
                _apiResponse.HttpStatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception e)
            {
                _apiResponse.ErrorMessages.Add(e.ToString());
                _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                _apiResponse.isSuccess = false;
                return BadRequest(_apiResponse);

            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddOrUpdateCartItem(string userId, int menuItemId, int updateQuantityBy)
        {
            // Shopping cart will have one entry per user id, even if a user has many items in cart.
            // Cart items will have all the items in shopping cart for a user
            // updatequantityby will have count by with an items quantity needs to be updated
            // if it is -1 that means we have lower a count if it is 5 it means we have to add 5 count to existing count.
            // if updatequantityby by is 0, item will be removed


            // when a user adds a new item to a new shopping cart for the first time
            // when a user adds a new item to an existing shopping cart (basically user has other items in cart)
            // when a user updates an existing item count
            // when a user removes an existing item

            ShoppingCart userShoppingCart = await _dbContext
                .ShoppingCarts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(u => u.UserId == userId);


            if (userShoppingCart == null && updateQuantityBy < 0)
            {
                _apiResponse.ErrorMessages.Add("You dont have a shopping cart and update quantity is lower than 0");
                _apiResponse.isSuccess = false;
                return BadRequest(_apiResponse);
            }

            // IF USER DOES NOT HAVE SHOPPING CART YET
            if (userShoppingCart == null)
            {
                userShoppingCart = new ShoppingCart()
                {
                    UserId = userId,
                    CartItems = new List<CartItem>(),
                };

                await _dbContext.ShoppingCarts.AddAsync(userShoppingCart);
                await _dbContext.SaveChangesAsync();
            }


            if (updateQuantityBy >= 1)
            {
                CartItem usersCartItem = userShoppingCart.CartItems.FirstOrDefault(c => c.MenuItemId == menuItemId);
                MenuItem chosenMenuItem = await _dbContext.MenuItems.FindAsync(menuItemId);

                if (chosenMenuItem == null)
                {
                    _apiResponse.ErrorMessages.Add("Menu item id is wrong");
                    return BadRequest(_apiResponse);
                }

                if (usersCartItem == null)
                {
                    CartItem newCartItem = new CartItem()
                    {
                        ShoppingCartId = userShoppingCart.Id,
                        Quantity = 0,
                        MenuItem = chosenMenuItem,
                        MenuItemId = chosenMenuItem.Id,
                    };
                    _dbContext.CartItems.Add(newCartItem);
                    await _dbContext.SaveChangesAsync();
                    usersCartItem = newCartItem; // ASIGNING NEW VALUES TO THE usersCartItem ? is it still being tracked by EF? - yes it is

                }

                if (usersCartItem.Quantity >= 0)
                {
                    usersCartItem.Quantity += updateQuantityBy;
                    await _dbContext.SaveChangesAsync();
                    _apiResponse.HttpStatusCode = HttpStatusCode.OK;
                    _apiResponse.Result = usersCartItem;
                    return Ok(_apiResponse);
                }
            }

            if (updateQuantityBy <= 0)
            {
                // searches for cart item in users shopping cart
                CartItem usersCartItem = userShoppingCart.CartItems.FirstOrDefault(c => c.MenuItemId == menuItemId);

                if (usersCartItem == null)
                {
                    return NotFound();
                }

                int newQuantity;

                if (updateQuantityBy == 0)
                {
                    newQuantity = 0;
                }
                else
                {
                    newQuantity = usersCartItem.Quantity += updateQuantityBy;
                }


                if (newQuantity > 0)
                {
                    usersCartItem.Quantity = newQuantity;
                    await _dbContext.SaveChangesAsync();

                    _apiResponse.isSuccess = true;
                    _apiResponse.HttpStatusCode = HttpStatusCode.OK;
                    _apiResponse.Result = usersCartItem;
                    return Ok(_apiResponse);
                }

                if (newQuantity <= 0)
                {
                    _dbContext.Remove(usersCartItem);
                    // if it was the only item in shopping cart then remove whole shopping cart
                    if (userShoppingCart.CartItems.Count == 1)
                    {
                        _dbContext.ShoppingCarts.Remove(userShoppingCart);
                    }
                    await _dbContext.SaveChangesAsync();

                    _apiResponse.isSuccess = true;
                    _apiResponse.HttpStatusCode = HttpStatusCode.OK;
                    return Ok(_apiResponse);
                }

            }

            //if (updateQuantityBy == 0)
            //{
            //    CartItem usersCartItem = userShoppingCart.CartItems.FirstOrDefault(c => c.MenuItemId == menuItemId);

            //    if (usersCartItem == null)
            //    {
            //        _apiResponse.isSuccess = false;
            //        _apiResponse.ErrorMessages.Add("you dont have that item in your shopping cart");
            //        return BadRequest(_apiResponse);
            //    }

            //    _dbContext.CartItems.Remove(usersCartItem);
            //    await _dbContext.SaveChangesAsync();
            //    _apiResponse.isSuccess = true;
            //    _apiResponse.HttpStatusCode = HttpStatusCode.OK;
            //    return Ok(_apiResponse);
            //}

            return _apiResponse;
        }
    }
}
