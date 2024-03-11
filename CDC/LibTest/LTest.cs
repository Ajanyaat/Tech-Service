


using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using LibraryDataAccess;
using LibraryAPI.Services;
using System.Linq;

namespace LibTest
{
    [TestClass]
    public class LTest
    {
        private readonly MockDataAccess _mockDataAccess = new MockDataAccess(); // Use MockDataAccess directly

        [TestMethod]
        public void AddBook_ValidBook_Success()
        {
            // Arrange
            LibraryService libraryService = new LibraryService(_mockDataAccess);
            Library newBook = new Library { title = "Test Book", author_id = 1, genre_id = 1, publication_year = 2022 };

            // Act
            libraryService.AddBook(newBook);

            // Assert
            List<Library> books = _mockDataAccess.GetAllBooks();
            Assert.IsTrue(books.Exists(book => book.title == newBook.title));
        }

        [TestMethod]
        public void GetAllBooks_ReturnsListOfBooks()
        {
            // Arrange
            // No need to arrange anything here as the MockDataAccess always returns an empty list by default

            // Act
            LibraryService libraryService = new LibraryService(_mockDataAccess);
            List<Library> books = libraryService.GetAllBooks();

            // Assert
            Assert.IsNotNull(books);
            Assert.AreEqual(0, books.Count); // Since MockDataAccess returns an empty list by default
        }

        

        [TestMethod]
public void UpdateBook_ValidBook_Success()
{
    // Arrange
    LibraryService libraryService = new LibraryService(_mockDataAccess);
    Library updatedBook = new Library { bookId = 1, genre_id = 1, publication_year = 2023 }; // Ensure a valid genre ID is provided

    // Act
    bool isUpdated = libraryService.UpdateBook(updatedBook);

    // Assert
    Assert.IsTrue(isUpdated, "UpdateBook method should return true for a successful update");
}


        [TestMethod]
        public void DeleteBooksByGenre_ValidGenreId_Success()
        {
            // Arrange
            LibraryService libraryService = new LibraryService(_mockDataAccess);
            int genreId = 1;

            // Act
            libraryService.DeleteBooksByGenre(genreId);

            // Assert
            // You can add assertions to verify the behavior, like checking if books with the specified genre ID were deleted successfully
            List<Library> books = _mockDataAccess.GetAllBooks();
            Assert.IsFalse(books.Any(book => book.genre_id == genreId), $"No books should have genre ID {genreId} after deletion");
        }

        [TestMethod]
        public void AddBook_InvalidPublicationYear_Failure()
        {
            // Arrange
            LibraryService libraryService = new LibraryService(_mockDataAccess);
            Library newBook = new Library { title = "Test Book", author_id = 1, genre_id = 1, publication_year = DateTime.Now.Year + 1 };

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => libraryService.AddBook(newBook), "Adding a book with invalid publication year should throw an exception");
        }

       
       [TestMethod]
public void UpdateBook_InvalidGenreId_Failure()
{
    // Arrange
    LibraryService libraryService = new LibraryService(_mockDataAccess);
    int invalidGenreId = -1; // Assume -1 is an invalid genre ID

    Library updatedBook = new Library { 
        bookId = 1, 
        author_id = 1, 
        genre_id = invalidGenreId, // Assigning the invalid genre ID
        publication_year = 2023 
    };

    // Act & Assert
    Assert.ThrowsException<ArgumentException>(() => libraryService.UpdateBook(updatedBook), "Updating a book with invalid genre ID should throw an exception");
}

        [TestMethod]
        public void DeleteBooksByGenre_InvalidGenreId_Failure()
        {
            // Arrange
            LibraryService libraryService = new LibraryService(_mockDataAccess);
            int invalidGenreId = -1; // Assume -1 is an invalid genre ID

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => libraryService.DeleteBooksByGenre(invalidGenreId), "Deleting books with invalid genre ID should throw an exception");
        }
    }


public class MockDataAccess : IDataAccess
{
    private readonly List<Library> _books = new List<Library>();

    public void AddLibrary(Library newLibrary)
    {
        _books.Add(newLibrary);
    }

    public List<Library> GetAllBooks()
    {
        return _books;
    }

    public bool UpdateBook(Library updatedBook)
    {
        // Mock implementation
        return true;
    }

    public bool DeleteBooksByGenre(int genreId)
    {
        // Mock implementation
        return true;
    }

    public Library GetBookById(int bookId)
    {
        return _books.FirstOrDefault(book => book.bookId == bookId);
    }
}
}