using BlogApi.Models;
using BlogApi.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet("Author/{id}")]
        public async Task<ActionResult<Autor>> Get(int id)
        {
            var author = await _authorService.GetPostById(id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }
    }
}
