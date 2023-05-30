using System.Net;
using FullstackStoreAPI.Data;
using FullstackStoreAPI.Models;
using FullstackStoreAPI.Models.DTO;
using FullstackStoreAPI.Services;
using FullstackStoreAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullstackStoreAPI.Controllers
{
    [Route("api/MenuItem")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IBlobService _blobService;
        private ApiResponse _apiResponse;

        public MenuItemController(AppDbContext dbContext,IBlobService blobService)
        {
            _dbContext = dbContext;
            _blobService = blobService;
            _apiResponse = new ApiResponse();
        }


        [HttpGet]
        public async Task<IActionResult> getMenuItems()
        {
            _apiResponse.HttpStatusCode = HttpStatusCode.OK;
            _apiResponse.Result = _dbContext.MenuItems;
            return Ok(_apiResponse);
        }

        [HttpGet("{id:int}",Name = "getMenuItem")]
        //[Route("{id:int}")]
        
        public async Task<IActionResult> getMenuItem([FromRoute]int id)
        {
            if (id == 0)
            {
                _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                _apiResponse.isSuccess = false;
                return BadRequest(_apiResponse);
            }
            MenuItem menuItem = await _dbContext.MenuItems.FirstOrDefaultAsync(item => item.Id == id);

            if (menuItem != null)
            {
                _apiResponse.HttpStatusCode = HttpStatusCode.OK;
                _apiResponse.Result = menuItem;
                _apiResponse.isSuccess = true;
                return Ok(_apiResponse);
            }
            else
            {
                _apiResponse.HttpStatusCode = HttpStatusCode.NotFound;
                _apiResponse.isSuccess = false;
                return NotFound(_apiResponse);
            }
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> createMenuItem([FromForm] MenuItemCreateDTO menuItemCreateDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (menuItemCreateDto.File == null || menuItemCreateDto.File.Length == 0)
                    {
                        _apiResponse.isSuccess = false;
                        _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                        return BadRequest(_apiResponse);
                    }

                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(menuItemCreateDto.File.FileName)}";
                    MenuItem menuItem = new MenuItem()
                    {
                        Category = menuItemCreateDto.Category,
                        Description = menuItemCreateDto.Description,
                        Name = menuItemCreateDto.Name,
                        Price = menuItemCreateDto.Price,
                        SpecialTag = menuItemCreateDto.SpecialTag,
                        Image = await _blobService.UploadBlob(fileName, SD.SD_Storage_Container, menuItemCreateDto.File)
                    };
                    _dbContext.MenuItems.Add(menuItem);
                    await _dbContext.SaveChangesAsync();
                    _apiResponse.isSuccess = true;
                    _apiResponse.Result = menuItem;
                    _apiResponse.HttpStatusCode = HttpStatusCode.Created;
                    // This returns location headers (the uri where the object is available) /api/menuitem/id
                    //_apiResponse is the response body
                    return CreatedAtRoute("getMenuItem", new { id = menuItem.Id }, _apiResponse); 
                }
                else
                {
                    _apiResponse.isSuccess = false;
                }
            }
            catch (Exception e)
            {
                _apiResponse.isSuccess = false;
                _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages = new List<string> { e.ToString() };
            }

            return _apiResponse;
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> updateMenuItem(int id,[FromForm]MenuItemUpdateDTO menuItemUpdateDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (menuItemUpdateDto == null || id != menuItemUpdateDto.Id)
                    {
                        _apiResponse.isSuccess = false;
                        _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                        return BadRequest(_apiResponse);
                    }

                    MenuItem menuItemFromDb = await _dbContext.MenuItems.FindAsync(id);
                    if (menuItemFromDb == null)
                    {
                        _apiResponse.isSuccess = false;
                        _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                        return BadRequest(_apiResponse);
                    }

                    menuItemFromDb.Category = menuItemUpdateDto.Category;
                    menuItemFromDb.Description = menuItemUpdateDto.Description;
                    menuItemFromDb.Name = menuItemUpdateDto.Name;
                    menuItemFromDb.SpecialTag = menuItemUpdateDto.SpecialTag;
                    menuItemFromDb.Price = menuItemUpdateDto.Price;

                    

                    if (menuItemUpdateDto.File != null && menuItemUpdateDto.File.Length != 0)
                    {
                        if (!string.IsNullOrEmpty(menuItemFromDb.Image))
                        {
                            await _blobService.DeleteBlob(menuItemFromDb.Image.Split('/').Last(),
                                SD.SD_Storage_Container);
                        }
                        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(menuItemUpdateDto.File.FileName)}";
                        menuItemFromDb.Image = await _blobService.UploadBlob(fileName, SD.SD_Storage_Container,
                            menuItemUpdateDto.File);
                    }

                    _apiResponse.isSuccess = true;
                    _apiResponse.HttpStatusCode = HttpStatusCode.OK;

                    _dbContext.MenuItems.Update(menuItemFromDb);
                    await _dbContext.SaveChangesAsync();
                    _apiResponse.Result = menuItemFromDb;
                    return Ok(_apiResponse);

                }
            }
            catch (Exception e)
            {
                _apiResponse.isSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { e.ToString()};
            }

            return _apiResponse;
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse>> deleteMenuItem(int id)
        {
            try
            {
                MenuItem menuItem = await _dbContext.MenuItems.FindAsync(id);
                if (id > 0 && menuItem != null)
                {
                    if (!string.IsNullOrEmpty(menuItem.Image))
                    {
                        await _blobService.DeleteBlob(menuItem.Image.Split('/').Last(), SD.SD_Storage_Container);
                    }

                    int miliseconds = 2000;
                    Thread.Sleep(miliseconds);


                    _dbContext.MenuItems.Remove(menuItem);
                    await _dbContext.SaveChangesAsync();

                    _apiResponse.isSuccess = true;
                    _apiResponse.HttpStatusCode = HttpStatusCode.NoContent;
                    return Ok(_apiResponse);
                }
                else
                {
                    _apiResponse.isSuccess = false;
                    _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }
            }
            catch (Exception e)
            {
                _apiResponse.ErrorMessages = new List<string>() { e.ToString()};
                _apiResponse.isSuccess = false;
            }


            return _apiResponse;
        }

    }

   

}
