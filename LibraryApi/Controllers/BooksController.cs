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
using Microsoft.AspNetCore.Http;

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
        public async Task<ActionResult<GetBooksSummaryResponse>> GetAllBooks([FromQuery] string genre = null)
        {
            //var data = await _context.AvailiableBooks
            //    .ProjectTo<BookSummaryItem>(_config)
            //    .ToListAsync();

            var query = _context.AvailiableBooks;
            if(genre != null)
            {
                query = query.Where(b => b.Genre == genre);
            }

            var data = await query.ProjectTo<BookSummaryItem>(_config).ToListAsync();

            var response = new GetBooksSummaryResponse
            {
                Data = data,
                GenreFilter = genre
            };
            return Ok(response);
        }

        /// <summary>
        /// Use this to add a book to our inventory
        /// </summary>
        /// <param name="request">Which book you want to add</param>
        /// <returns>A new book</returns>

        [HttpPost("/books")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 15)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<GetBookDetailsResponse>> AddABook([FromBody] PostBookRequst request)
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetBookDetailsResponse>> GetBookById(int id)
        {
            var book = await _context.AvailiableBooks
                .Where(b => b.Id == id)
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

        [HttpDelete("/books/{id:int}")]
        public async Task<ActionResult> RemoveBooksFromInventory(int id)
        {
            var book = await _context.AvailiableBooks.SingleOrDefaultAsync(b => b.Id == id);
            if(book != null)
            {
                book.IsAvailiable = false;
                await _context.SaveChangesAsync();
            }
            return NoContent();
        }
    }
}
