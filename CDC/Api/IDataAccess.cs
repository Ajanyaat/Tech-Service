// File: IDataAccess.cs
//Methods declared in interfaces do not have implementations; they only define the method signature.
using System.Collections.Generic;

namespace LibraryDataAccess
{
    public interface IDataAccess
    {
        void AddLibrary(Library newLibrary);// It declares a method named GetAllBooks that returns a list of Library objects. 
        List<Library> GetAllBooks();
        // bool UpdateTotalGross(int bookId, int newPublicationYear);
        // bool UpdateBook(Library updatedBook); 
       // bool DeleteBooksByGenre(int genreId);
        bool DeleteBookById(int bookId);
        Library GetBookById(int bookId); 
        bool UpdateBookById(int bookId, Library updatedBook);
      
      
      
    }
}
