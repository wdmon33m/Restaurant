using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Services.ProductAPI.Data;
using Restaurant.Services.ProductAPI.Models;
using Restaurant.Services.ProductAPI.Models.Dto;
using System.Data;

namespace Restaurant.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    //[Produces("*/*")]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ResponseDto _response;
        private IMapper _mapper;

        public ProductAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new();
        }
        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Product> list = _db.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(list);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Product product = _db.Products.FirstOrDefault(c => c.ProductId == id);
                if (product == null)
                {
                    _response.ErrorMessages = new() { "No product found" };
                }
                _response.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return _response;
        }

        [HttpPost]
        [Authorize]
        public ResponseDto Post([FromForm]ProductDto productDto)
        {
            try
            {
                Product product = _db.Products.FirstOrDefault(c => c.Name == productDto.Name);
                if (product != null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>() { "Product name is already exist!" };
                    return _response;
                }
                else
                {
                    product = _mapper.Map<Product>(productDto);
                    _db.Products.Add(product);
                    _db.SaveChanges();

                    if (productDto.Image != null)
                    {
                        string fileName = product.ProductId + Path.GetExtension(productDto.Image.FileName);
                        string filePath = @"wwwroot\ProductImages\" + fileName;
                        var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                        using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                        {
                            productDto.Image.CopyTo(fileStream);
                        }
                        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                        product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                        product.ImageLocalPath = filePath;
                    }
                    else
                    {
                        product.ImageUrl = "https://placehold.co/600x400";
                    }

                    _db.Products.Update(product);
                    _db.SaveChangesAsync();
                    _response.Result = _mapper.Map<ProductDto>(product);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return _response;
        }

        [HttpPut]
        [Authorize]
        public ResponseDto Put([FromForm]ProductDto productDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDto);
                _db.Products.Update(product);
                _db.SaveChanges();

                if (productDto.Image != null)
                {
                    if (!string.IsNullOrEmpty(product.ImageLocalPath))
                    {
                        var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                        FileInfo file = new FileInfo(oldFilePathDirectory);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }

                    string fileName = product.ProductId + Path.GetExtension(productDto.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        productDto.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                    product.ImageLocalPath = filePath;
                }
                else
                {
                    product.ImageUrl = "https://placehold.co/600x400";
                }

                _db.Products.Update(product);
                _db.SaveChangesAsync();
                _response.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Product product = _db.Products.First(c => c.ProductId == id);

                if (!string.IsNullOrEmpty(product.ImageLocalPath))
                {
                    var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                    FileInfo file = new FileInfo(oldFilePathDirectory);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }

                _db.Products.Remove(product);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return _response;
        }
    }
}
