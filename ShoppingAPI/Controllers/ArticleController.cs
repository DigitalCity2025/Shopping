using Microsoft.AspNetCore.Mvc;
using Shopping.DAL;
using Shopping.DAL.Entities;

namespace Shopping.API.Controllers 
{

    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController(ShoppingContext context) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(context.Articles.ToList()); // 200
        }

        [HttpPost]
        public IActionResult Post([FromBody] Article article)
        {
            context.Articles.Add(article);
            context.SaveChanges();
            return Created(); // 201
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            Article? articleToDelete = context.Articles.Find(id);
            if(articleToDelete == null)
            {
                return NotFound(); // 404
            }
            context.Articles.Remove(articleToDelete);
            context.SaveChanges();
            return NoContent(); // 204
        }

        [HttpDelete("all")]
        public IActionResult DeleteAll()
        {
            context.Articles.RemoveRange(context.Articles.ToList());
            context.SaveChanges();
            return NoContent();
        }

    }
}
