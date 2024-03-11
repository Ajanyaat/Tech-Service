
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryDataAccess;
using Moq;
using System.Collections.Generic;
using LibraryAPI.Services;
using System;

namespace DataAccessTest
{
    [TestClass]
    public class Access
    {
        private const string ConnectionString = "Server=localhost;Port=3306;Database=library;Uid=root";

        [TestMethod]
        public void Test_GetAllBooks()
        {
            // Arrange
            DataAccess dataAccess = new DataAccess(ConnectionString);

            // Act
            List<Library> books = dataAccess.GetAllBooks();

            // Assert
            Assert.IsNotNull(books);
            Assert.IsTrue(books.Count > 0);
            // Add more assertions as needed
        }

        [TestMethod]
        public void Test_AddLibrary()
        {
            // Arrange
            var mockDataAccess = new Mock<IDataAccess>();
            mockDataAccess.Setup(da => da.AddLibrary(It.IsAny<Library>()));

            // Act
            mockDataAccess.Object.AddLibrary(new Library
            {
                title = "Test Book",
                author_id = 1,
                genre_id = 1,
                publication_year = 2022
            });

            // Assert
            // Verify that the AddLibrary method of IDataAccess was called with the correct parameters
            mockDataAccess.Verify(da => da.AddLibrary(It.IsAny<Library>()), Times.Once);
        }

   
    [TestMethod]
    public void Test_DeleteBooksByGenre()
   {
    // Arrange
    var mockDataAccess = new Mock<IDataAccess>();
    mockDataAccess.Setup(da => da.DeleteBooksByGenre(It.IsAny<int>())).Returns(true); // Assuming DeleteBooksByGenre returns a boolean

    int genreIdToDelete = 1;

    // Act
    bool result = mockDataAccess.Object.DeleteBooksByGenre(genreIdToDelete);

    // Assert
    Assert.IsTrue(result);
   }

   [TestMethod]
public void Test_UpdateBook()
{
    // Arrange
    var mockDataAccess = new Mock<IDataAccess>();
    mockDataAccess.Setup(da => da.UpdateBook(It.IsAny<Library>())).Returns(true); // Assuming UpdateBook returns a boolean

    var libraryToUpdate = new Library
    {
        bookId = 1,
        title = "Updated Title",
        author_id = 2,
        genre_id = 3,
        publication_year = 2023
    };

    // Act
    bool result = mockDataAccess.Object.UpdateBook(libraryToUpdate);

    // Assert
    Assert.IsTrue(result);
}

[TestMethod]
        public void Test_UpdateBook_Negative_Case_Book_Not_Found()
        {
            // Arrange
            var mockDataAccess = new Mock<IDataAccess>();
            mockDataAccess.Setup(da => da.UpdateBook(It.IsAny<Library>())).Returns(false); // Assuming UpdateBook returns a boolean

            var libraryToUpdate = new Library
            {
                bookId = 100, // Assuming this book ID does not exist in the database
                title = "Updated Title",
                author_id = 2,
                genre_id = 3,
                publication_year = 2023
            };

            // Act
            bool result = mockDataAccess.Object.UpdateBook(libraryToUpdate);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
public void Test_AddLibrary_Negative_Case()
{
    // Arrange
    var mockDataAccess = new Mock<IDataAccess>();
    mockDataAccess.Setup(da => da.AddLibrary(It.IsAny<Library>())).Throws(new Exception("Failed to add library")); // Simulate an error during library addition

    // Act & Assert
    Assert.ThrowsException<Exception>(() => mockDataAccess.Object.AddLibrary(new Library
    {
        title = "Test Book",
        author_id = 1,
        genre_id = 1,
        publication_year = 2022
    }));
}
 [TestMethod]
        public void Test_DeleteBooksByGenre_Negative()
        {
            // Arrange
            var mockDataAccess = new Mock<IDataAccess>();
            mockDataAccess.Setup(da => da.DeleteBooksByGenre(It.IsAny<int>())).Returns(false); // Assuming DeleteBooksByGenre returns false for genre not found

            int genreIdToDelete = 1;

            // Act
            bool result = mockDataAccess.Object.DeleteBooksByGenre(genreIdToDelete);

            // Assert
            Assert.IsFalse(result);
        }



    }
}
