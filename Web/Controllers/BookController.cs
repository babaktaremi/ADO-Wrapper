using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AdoWrapper.Contracts;
using Web.Models;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IAdoProvider _ado;

        public BookController(IAdoProvider ado)
        {
            _ado = ado;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _ado.GetFirstOrDefaultAsync<Book>("EXEC GetFirstBook");

            return Ok(result);
        }

        [HttpGet("Books")]
        public async Task<IActionResult> GetBooks()
        {
            var result = await _ado.GetListAsync<Book>("SELECT * FROM [AdoTest].[dbo].[Books]");

            return Ok(result);
        }
    }
}