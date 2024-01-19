using BlogApi.Models;
using Microsoft.AspNetCore.Mvc;


namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        // GET: api/Post/{id}
        [HttpGet("Post/{id}")]
        public ActionResult<Post> GetPost(int id)
        {
            return null;
        }

        // GET: api/Blog/Posts?pageNumber=1&pageSize=10
        [HttpGet("Posts")]
        public ActionResult<List<Post>> GetPosts(int pageNumber, int pageSize)
        {

            return null;
        }

        // POST: api/Blog/AddPost
        [HttpPost("Post")]
        public ActionResult<string> AddPost([FromBody] Post value)
        {

            return null;
        }
    }
}