using AutoMapper;
using LibraryApi.Domain;
using LibraryApi.Models.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

namespace LibraryApi.Controllers
{
    public class BooksController : ControllerBase
    {
        private readonly LibraryDataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _config;

        public BooksController(LibraryDataContext context, IMapper mapper, MapperConfiguration config = null)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }

        [HttpGet("/books/{id:int}")]
        public async Task<ActionResult> GetBookById(int id)
        {
            var book = await _context.Books
                .Where(b => b.IsAvailiable && b.Id == id)
                .ProjectTo<GetBookDetailsResponse>(_config)
                .SingleOrDefaultAsync();

            if(book == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(book);
            }
        }
    }
}
