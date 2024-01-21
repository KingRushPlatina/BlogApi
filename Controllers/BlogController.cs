using BlogApi.Models;
using BlogApi.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IAuthorService _authorService;

        public BlogController(IBlogService blogService, IAuthorService authorService)
        {
            _authorService = authorService;
            _blogService = blogService;
        }

        [HttpGet("Post/{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            Post post = await _blogService.GetPostById(id);
            post.Autor = await SearchAutor(post.Autor.Id);
            if (post == null)
            {
                return NotFound(); 
            }

            return Ok(post);
        }

        [HttpGet("Posts")]
        public async Task<IActionResult> GetPosts(int pageNumber=1, int pageSize = 4)
        {
            var posts = await _blogService.GetPosts(pageNumber, pageSize);

            var responsiveList = new ResponsiveList<Post>
            {
                PageSize = pageSize,
                List = posts
            };

            return Ok(responsiveList.List);
        }

        [HttpPost("Post")]
        public async Task<ActionResult> AddPost(int id)
        {
            //if (await SearchAutor(value.Autor.Id) is not Autor author)
            //    return BadRequest("L'autore non esiste.");

            //await _blogService.AddPost(value);

            return Ok("Aggiunto Corretamente");
        }

        #region private methods
        private async Task<Autor> SearchAutor(int id)
        {
            Autor? author = await _authorService.GetPostById(id);
            return author;
        }

        #endregion
    }
}
