using System.Collections.Generic;
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

        [HttpGet("Books")]
        public async Task<IActionResult> GetBooks()
        {
            var result = await _ado.GetListAsync<Author>("select * from Authors Left join Books on Authors.ID=Books.AuthorId");

            return Ok(result);
        }
    }
}