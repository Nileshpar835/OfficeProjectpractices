using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleMvcApp.Models;
using SimpleMvcApp.Data;

namespace SimpleMvcApp.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetproductS()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProuct(Product product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(new { mess = "product not found" });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { mess = "deleted" });
        }

        //    private static List<Product> productlist = new List<Product>
        //    {
        //        new Product { id = 1, Name = "Loptop", Price = 999.99m, Category = "Electronics", IsInStock = true },
        //        new Product { id = 2, Name = "Wireless Mouse", Price = 90.99m, Category = "Accessories", IsInStock = true },
        //        new Product { id = 3, Name = "Earphones", Price = 95.99m, Category = "Electronics", IsInStock = true },
        //        new Product { id = 4, Name = "Loptop", Price = 99.99m, Category = "Electronics", IsInStock = false }
        //    };


        //    [HttpGet]
        //    public IActionResult GetProducts()
        //    {
        //        return Ok(productlist);
        //    }

        //    [HttpGet("{id}")]
        //    public IActionResult getProductByid(int id){
        //        var product = productlist.FirstOrDefault(p => p.id == id);

        //        if(product == null)
        //        {
        //            return NotFound(new {Message=$"product with id {id} not found" });
        //        }
        //        return Ok(product);
        //    }

        //    [HttpDelete("{id}")]
        //    public IActionResult deletebyid(int id)
        //    {
        //        var product = productlist.FirstOrDefault(p => p.id == id);

        //        if (product == null)
        //        {
        //            return NotFound(new { Message = $"product with id {id} not found" });
        //        }
        //        productlist.Remove(product);
        //        return Ok(new
        //        {
        //            Message = $"Product with id {id} deleted successfully"
        //        });
        //    }
        //    [HttpPost]
        //    public IActionResult createProduct(Product product)
        //    {
        //        product.id = productlist.Any() ? productlist.Max(p => p.id) + 1 : 1;


        //        productlist.Add(product);

        //        return Ok(new
        //        {
        //            Message = $"Product created successfully",
        //            Product = product
        //        });
        //    }
        //}

    }
}
