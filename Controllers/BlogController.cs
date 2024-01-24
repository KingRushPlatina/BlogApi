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
        private readonly  string address = "http://localhost:5108/";
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
            foreach (var item in post.Comments)
            {
                item.Commentator = await SearchAutor(item.Commentator.Id);
            }
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        [HttpGet("Posts")]
        public async Task<IActionResult> GetPosts(string title = null, int pageNumber = 1, int pageSize = 4)
        {
            List<Post> posts = await _blogService.GetPosts(title, pageNumber, pageSize);
            
            foreach (var post in posts)
            {
                post.Autor = await SearchAutor(post.Autor.Id);
            }
            var responsiveList = new ResponsiveList<Post>
            {
                PageSize = pageSize,
                List = posts
            };

            return Ok(responsiveList.List);
        }

        
      
        [HttpPost("Upload")]
        public async Task<ActionResult> AddUpload([FromForm] InputPost value)
        {
            try
            {
                if (await SearchAutor(value.AutorId) is not Autor author)
                {
                    return BadRequest("L'autore non esiste.");
                }
                var file = value.File;
                if (file != null && file.Length > 0)
                {
                    var filePath = Path.Combine("Images", file.FileName);
                    value.Path = address + filePath;
                    value.Path = value.Path.Replace("\\", "/");
   
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await _blogService.AddPost(value);
                        await file.CopyToAsync(stream);
                        
                    }   
                }         
               return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Errore interno del server"+ex);
            }
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
