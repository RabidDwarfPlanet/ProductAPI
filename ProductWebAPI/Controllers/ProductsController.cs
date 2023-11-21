using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductWebAPI.Data;
using ProductWebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public IActionResult Get([FromQuery] int? maxPrice)
        {
            var products = _context.Products.ToList();
            if(maxPrice != null)
            {
                products = products.Where(p => p.Price < maxPrice).ToList();
            }
            return Ok(products);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = _context.Products.Include(p => p.Reviews)
            .Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Reviews = p.Reviews.Select(r => new ReviewDTO
                {
                    Id = r.Id,
                    Text = r.Text,
                    Rating = r.Rating,
                }).ToList(),
                AverageRating = p.Reviews.Average(r => r.Rating)
            }).Where(p => p.Id == id);
            if (product == null) { return NotFound(); }
            else { return Ok(product); }

        }

        // POST api/<ProductsController>
        [HttpPost]
        public IActionResult Post([FromBody] Models.Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return StatusCode(201, product);
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Models.Product updatedProduct)
        {
            var product = _context.Products.SingleOrDefault(p => p.Id == id);
            if (product == null) { return NotFound(); }
            else
            {
                product.Name = updatedProduct.Name;
                product.Price = updatedProduct.Price;
                _context.Products.Update(product);
                _context.SaveChanges();
                return Ok(product);
            }
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                return NoContent();
            }
            else { return NotFound(); }
        }
    }
}
