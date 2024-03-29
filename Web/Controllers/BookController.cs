﻿using System.Collections.Generic;
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

        [HttpGet("BooksAsync")]
        public async Task<IActionResult> GetBooksAsync()
        {
            var result = await _ado.GetListAsync<Author>("select * from Authors Left join Books on Authors.ID=Books.AuthorId");

            return Ok(result);
        }

        [HttpGet("Books")]
        public IActionResult GetBooks()
        {
            var result =  _ado.GetList<Author>("select * from Authors Left join Books on Authors.ID=Books.AuthorId");

            return Ok(result);
        }

        [HttpGet("SingleBook")]
        public IActionResult SingleBook()
        {
            var result =
                _ado.GetFirstOrDefault<SingleAuthor>(
                    "select * from Authors Left join Books on Authors.ID=Books.AuthorId where Authors.ID=1");

            return Ok(result);
        }

        [HttpGet("SingleBookAsync")]
        public async Task<IActionResult> SingleBookAsync()
        {
            var result =
              await  _ado.GetFirstOrDefaultAsync<SingleAuthor>(
                    "select * from Authors Left join Books on Authors.ID=Books.AuthorId where Authors.ID=2");

            return Ok(result);
        }

        [HttpGet("SingleBookSingleNavigation")]
        public IActionResult SingleBookSingleNavigation()
        {
            var result =
                _ado.GetFirstOrDefault<SingleAuthorSingleNavigation>(
                    "select * from Authors Left join Books on Authors.ID = Books.AuthorId where Authors.ID = 1 and Books.ID = 4");

            return Ok(result);
        }

        [HttpGet("SingleBookSingleNavigationAsync")]
        public async Task<IActionResult> SingleBookSingleNavigationAsync()
        {
            var result =await 
                _ado.GetFirstOrDefaultAsync<SingleAuthorSingleNavigation>(
                    "select * from Authors Left join Books on Authors.ID = Books.AuthorId where Authors.ID = 1 and Books.ID = 4");

            return Ok(result);
        }

        [HttpGet("SimpleAuthorListAsync")]
        public async Task<IActionResult> GetSimpleAuthorListAsync()
        {
            var result = await
                _ado.GetListAsync<SimpleAuthor>(
                    "Select * from Authors");

            return Ok(result);
        }

        [HttpGet("SimpleAuthorList")]
        public  IActionResult GetSimpleAuthorList()
        {
            var result = 
                _ado.GetList<SimpleAuthor>(
                    "Select * from Authors");

            return Ok(result);
        }

        [HttpGet("SimpleSingleAuthor")]
        public IActionResult GetSimpleSingleAuthor()
        {
            var result = _ado.GetFirstOrDefault<SimpleAuthor>("Select top(1) * from Authors Order By Authors.ID desc");
            return Ok(result);
        }

        [HttpGet("SimpleSingleAuthorAsync")]
        public async Task<IActionResult> GetSimpleSingleAuthorAsync()
        {
            var result =await _ado.GetFirstOrDefaultAsync<SimpleAuthor>("Select top(1) * from Authors Order By Authors.ID desc");
            return Ok(result);
        }
    }
}