using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using LibraryController;
using LibraryDataAccess;
using LibraryAPI.Services;
using System;
using System.Collections.Generic;

namespace LibController
{
    [TestClass]
    public class LibC
    {
        [TestMethod]
        public void AddBook_ValidBook_Success()
        {
            // Arrange
            var mockLibraryService = new Mock<LibraryService>();
            mockLibraryService.Setup(service => service.AddBook(It.IsAny<Library>()));
            var controller = new LibraryController.LibraryController(null, mockLibraryService.Object); // Fully qualify the namespace
            var newLibrary = new Library { title = "Test Book", author_id = 1, genre_id = 1, publication_year = 2022 };

            // Act
            var result = controller.AddBook(newLibrary) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("Book added successfully.", result.Value);
        }
       [TestMethod]
        public void UpdateBook_ValidData_Success()
        {
            // Arrange
            var mockLibraryService = new Mock<LibraryService>();
            // var controller = new LibraryController(null, mockLibraryService.Object);
            var controller = new LibraryController.LibraryController(null, mockLibraryService.Object);

            var updatedBook = new Library { title = "Updated Book Title", author_id = 1, genre_id = 1, publication_year = 2023 };

            // Act
            var result = controller.UpdateBook(1, updatedBook) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("Book updated successfully.", result.Value);
        }

    }
}
