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

        //[HttpPost("Post")]
        //public async Task<ActionResult> AddPost([FromBody] Post value)
        //{
        //    if (await SearchAutor(value.Autor.Id) is not Autor author)
        //        return BadRequest("L'autore non esiste.");

        //    await _blogService.AddPost(value);

        //    return Ok();
        //}
        [HttpPost("Post")]
        public async Task<ActionResult> AddPost([FromForm] Post formData)
        {
            try
            {
                // Estrai i dati del post
                var title = formData.Title;
                var body = formData.Body;
                var authorId = formData.Autor.Id;

                // Estrai l'immagine
                var file = formData.File;
                if (file != null && file.Length > 0)
                {
                    // Esegui l'upload del file nel tuo server (ad esempio, salva il file su disco)
                    var filePath = Path.Combine("Images", file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                // Esegui l'inserimento del post nel database
                await _blogService.AddPost(new Post
                {
                    Title = title,
                    Body = body,
                    Autor = new Autor { Id = authorId }
                });

                return Ok();
            }
            catch (Exception ex)
            {
                // Gestisci gli errori
                return StatusCode(StatusCodes.Status500InternalServerError, "Errore interno del server");
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
