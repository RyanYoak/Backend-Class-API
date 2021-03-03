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

        [HttpGet("/books")]
        public async Task<ActionResult> GetAllBooks()
        {
            var data = await _context.Books.Where(b => b.IsAvailiable)
                .ProjectTo<BookSummaryItem>(_config)
                .ToListAsync();
            var response = new GetBooksSummaryResponse
            {
                Data = data
            };
            return Ok(response);
        }

        [HttpPost("/books")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 15)]
        public async Task<ActionResult> AddABook([FromBody] PostBookRequst request)
        {
            // 1 Validate it. If Not - return a 400 Bad Request, optionally with some info
            // progamatic, imparative validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // 2. Save it in the database
            //    - Turn a PostBookRequest -> Book
            var bookToSave = _mapper.Map<Book>(request);
            //    - Add it to the Context
            _context.Books.Add(bookToSave);
            //    - Tell the context to save the changes.
            await _context.SaveChangesAsync();
            //    - Turn that saved book into a GetBookDetailsResponse
            var response = _mapper.Map<GetBookDetailsResponse>(bookToSave);
            // 3. Return:
            //    - 201 Created Status Code
            //    - A location header with the Url of the newly created book.
            //    - A copy of the newly created book (what they would get if they followed the location)
            return CreatedAtRoute("books#getbookbyrid", new { id = response.Id }, response);
        }

        [HttpGet("/books/{id:int}", Name = "books#getbookbyrid")]
        public async Task<ActionResult> GetBookById(int id)
        {
            var book = await _context.Books
                .Where(b => b.IsAvailiable && b.Id == id)
                .ProjectTo<GetBookDetailsResponse>(_config)
                .SingleOrDefaultAsync();

            if(book == null)
            {
                //_logger.LogWarning("No book with that id!", id);
                return NotFound();
            }
            else
            {
                return Ok(book);
            }
        }
    }
}
