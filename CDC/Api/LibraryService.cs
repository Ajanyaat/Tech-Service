

// using System;
// using System.Collections.Generic;
// using LibraryDataAccess;
// using System.Linq;

// namespace LibraryAPI.Services
// {
//     public class LibraryService : ILibraryService
//     {
//         private readonly IDataAccess _dataAccess;

//         public LibraryService(IDataAccess dataAccess)
//         {
//             _dataAccess = dataAccess;
//         }

//         // Method to add a book to the library
//         public void AddBook(Library newLibrary)
//         {
//             // Perform any necessary validation or business logic here
//             if (newLibrary.publication_year > DateTime.Now.Year)
//             {
//                 throw new ArgumentException("Invalid publication year. Publication year cannot exceed the current year.");
//             }

//             // Add the new book to the database
//             _dataAccess.AddLibrary(newLibrary);
//         }

//         public void UpdateRepairStatusForDamagedBooks()
//         {
//             // Retrieve damaged books
//             List<Library> damagedBooks = _dataAccess.GetDamagedBooks();

//             // Iterate through damaged books and update repair status
//             foreach (var book in damagedBooks)
//             {
//                 string repairStatus = _dataAccess.GetRepairStatusForBook(book.bookId);
//                 // Update the repair status for the damaged book
//                 _dataAccess.UpdateRepairStatus(book.bookId, repairStatus);
//             }
//         }

//         public string GetRepairStatus(decimal budget)
//         {
//             // Query the Damage table to determine the repair status based on the provided budget
//             return _dataAccess.GetRepairStatus(budget);
//         }

//         // Method to retrieve all books from the library
//         public List<Library> GetAllBooks()
//         {
//             return _dataAccess.GetAllBooks();
//         }

//         // Method to update a book in the library
//         public bool UpdateBook(Library updatedBook)
//         {
//             // Perform any necessary validation or business logic here
//             if (updatedBook.genre_id <= 0)
//             {
//                 throw new ArgumentException("Invalid genre ID. Genre ID must be greater than 0.");
//             }

//             return _dataAccess.UpdateBook(updatedBook);
//         }

//         // Method to delete books by genre from the library
//         public bool DeleteBooksByGenre(int genreId)
//         {
//             // Perform any necessary validation or business logic here
//             if (genreId <= 0)
//             {
//                 throw new ArgumentException("Invalid genre ID. Genre ID must be greater than 0.");
//             }
//             return _dataAccess.DeleteBooksByGenre(genreId);
//         }

//         // Method to identify damaged books
//         public List<Library> GetDamagedBooks()
//         {
//             // Query the database for books with non-empty damages field
//             return _dataAccess.GetDamagedBooks();
//         }
//     }
// }
using System;
using System.Collections.Generic;
using LibraryDataAccess;
using System.Linq;

namespace LibraryAPI.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IDataAccess _dataAccess;

        public LibraryService(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        // Method to add a book to the library
        public void AddBook(Library newLibrary)
        {
            // Perform any necessary validation or business logic here
            if (newLibrary.publication_year > DateTime.Now.Year)
            {
                throw new ArgumentException("Invalid publication year. Publication year cannot exceed the current year.");
            }

            // Add the new book to the database
            _dataAccess.AddLibrary(newLibrary);
        }

       // Method to get all books from the library
    public List<Library> GetAllBooks()
{
    // Query the database to get all books
    return _dataAccess.GetAllBooks();
}

      // Method to update a book in the library by its ID
        public bool UpdateBookById(int bookId, Library updatedBook)
        {
            // Perform any necessary validation or business logic here
            if (bookId <= 0)
            {
                throw new ArgumentException("Invalid book ID. Book ID must be greater than 0.");
            }

            return _dataAccess.UpdateBookById(bookId, updatedBook);
        } 

        // // Method to update a book in the library
        // public bool UpdateBook(Library updatedBook)
        // {
        //     // Perform any necessary validation or business logic here
        //     if (updatedBook.genre_id <= 0)
        //     {
        //         throw new ArgumentException("Invalid genre ID. Genre ID must be greater than 0.");
        //     }

        //     return _dataAccess.UpdateBook(updatedBook);
        // }

        // Method to delete books by genre from the library
        // public bool DeleteBooksByGenre(int genreId)
        // {
        //     // Perform any necessary validation or business logic here
        //     if (genreId <= 0)
        //     {
        //         throw new ArgumentException("Invalid genre ID. Genre ID must be greater than 0.");
        //     }
        //     return _dataAccess.DeleteBooksByGenre(genreId);
        // }
         public bool DeleteBookById(int bookId)
        {
            // Perform any necessary validation or business logic here
            if (bookId <= 0)
            {
                throw new ArgumentException("Invalid book ID. Book ID must be greater than 0.");
            }
            return _dataAccess.DeleteBookById(bookId);
        }


        // Method to identify damaged books
        // public List<Library> GetDamagedBooks()
        // {
        //     // Query the database for books with non-empty damages field
        //     return _dataAccess.GetDamagedBooks();
        // }
    }
}
