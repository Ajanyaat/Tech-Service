using System;
using System.Collections.Generic;

namespace LibraryAPI.Services
{
    public interface ILibraryService
    {
        void AddBook(Library book);
        List<Library> GetAllBooks();
        // bool UpdateBook(int bookId, int newPublicationYear);
       // bool UpdateBook(Library updatedBook);
        //bool DeleteBooksByGenre(int genreId);
         bool DeleteBookById(int bookId);
        // string GetRepairStatus(decimal budget);
         bool UpdateBookById(int bookId, Library updatedBook);
        // Other methods...
    }
}
