using BlogApi.Models;
using BlogApi.Service;
using BlogApi.Service.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService  authorService)
        {
            _authorService = authorService;
        }


        // GET api/<AuthorsController>/Post/5
        [HttpGet("Author/{id}")]
        public ActionResult<Autor> Get(int id)
        {
            var author = _authorService.GetPostById(id);

            if (author == null)
            {
                return NotFound(); 
            }

            return author;
        }
    }
}
