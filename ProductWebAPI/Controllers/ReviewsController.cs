using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductWebAPI.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<ReviewsController>
        [HttpGet]
        public IActionResult Get()
        {
            var reviews = _context.Reviews.ToList();
            return Ok(reviews);
        }

        // GET api/<ReviewsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var review = _context.Reviews.Find(id);
            if (review == null) { return NotFound(); }
            else { return Ok(review); }

        }

        // GET api/<ReviewsController>/5
        [HttpGet("Product/{id}")]
        public IActionResult GetByProduct(int id)
        {
            var reviews = _context.Reviews.Where(r => r.ProductId == id);
            if (reviews == null) { return NotFound(); }
            else 
            { 
                return Ok(reviews); 
            }

        }

        // POST api/<ReviewsController>
        [HttpPost]
        public IActionResult Post([FromBody] Models.Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
            return StatusCode(201, review);
        }

        // PUT api/<ReviewsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Models.Review updatedReview)
        {
            var review = _context.Reviews.SingleOrDefault(p => p.Id == id);
            if (review == null) { return NotFound(); }
            else
            {
                review.Text = updatedReview.Text;
                review.Rating = updatedReview.Rating;
                review.ProductId = updatedReview.ProductId;
                _context.Reviews.Update(review);
                _context.SaveChanges();
                return Ok(review);
            }
        }

        // DELETE api/<ReviewsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var review = _context.Reviews.Find(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                _context.SaveChanges();
                return NoContent();
            }
            else { return NotFound(); }
        }
    }
}
