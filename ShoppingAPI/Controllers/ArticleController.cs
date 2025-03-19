using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Shopping.DAL;
using Shopping.DAL.Entities;

namespace Shopping.API.Controllers 
{

    [ApiController]
    [Route("api/[controller]")]
    // authorisation globale sur toutes les routes du controller
    //[Authorize(Roles = "Admin,Noob")]
    public class ArticleController(ShoppingContext context) : ControllerBase
    {
        [HttpGet]
        [Authorize] // doit etre authentifier peut importe le role
        public IActionResult Get()
        {
            return Ok(context.Articles.ToList()); // 200
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // doit avoir le role admin
        public IActionResult Post([FromBody] Article article)
        {
            context.Articles.Add(article);
            context.SaveChanges();
            return Created(); // 201
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAll()
        {
            context.Articles.RemoveRange(context.Articles.ToList());
            context.SaveChanges();
            return NoContent();
        }

    }
}
