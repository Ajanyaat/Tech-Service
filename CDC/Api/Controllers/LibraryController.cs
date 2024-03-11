

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using LibraryDataAccess;
using LibraryAPI.Services; 

namespace LibraryController
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly LibraryService _libraryService;

        public LibraryController(IDataAccess dataAccess)
        {
            _libraryService = new LibraryService(dataAccess);
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] Library newLibrary)
        {
            try
            {
                if (newLibrary == null)
                {
                    return BadRequest("Invalid book data. Book object is null.");
                }

                if (newLibrary.publication_year > DateTime.Now.Year)
                {
                    return BadRequest("Invalid publication year. Publication year cannot exceed the current year.");
                }

                _libraryService.AddBook(newLibrary);
                return Ok("Book added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding book: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                List<Library> books = _libraryService.GetAllBooks();
                if (books == null || books.Count == 0)
                {
                    return NotFound("No books found.");
                }
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // [HttpPut("{genreId}")]
        // public IActionResult UpdateBook(int genreId, [FromBody] Library library)
        // {
        //     try
        //     {
        //         if (library == null)
        //         {
        //             return BadRequest("Invalid book data. Book object is null.");
        //         }

        //         bool isUpdated = _libraryService.UpdateBook(library);
        //         if (!isUpdated)
        //         {
        //             return NotFound($"Book with genre ID {genreId} not found.");
        //         }
        //         return Ok("Book updated successfully.");
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, ex.Message);
        //     }
        // }
         [HttpPut("{bookId}")]
        public IActionResult UpdateBook(int bookId, [FromBody] Library library)
        {
            try
            {
                if (library == null)
                {
                    return BadRequest("Invalid book data. Book object is null.");
                }

                bool isUpdated = _libraryService.UpdateBookById(bookId, library);
                if (!isUpdated)
                {
                    return NotFound($"Book with ID {bookId} not found.");
                }
                return Ok("Book updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // [HttpDelete("{genreId}")]
        // public IActionResult DeleteBooksByGenre(int genreId)
        // {
        //     try
        //     {
        //         bool isDeleted = _libraryService.DeleteBooksByGenre(genreId);
        //         if (!isDeleted)
        //         {
        //             return NotFound($"Books with genre ID {genreId} not found.");
        //         }
        //         return Ok($"Books with genre ID {genreId} deleted successfully.");
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, ex.Message);
        //     }
        // }
         [HttpDelete("{bookId}")]
        public IActionResult DeleteBookById(int bookId)
        {
            try
            {
                bool isDeleted = _libraryService.DeleteBookById(bookId);
                if (!isDeleted)
                {
                    return NotFound($"Book with ID {bookId} not found.");
                }
                return Ok($"Book with ID {bookId} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
