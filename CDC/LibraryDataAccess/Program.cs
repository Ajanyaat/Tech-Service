

using System;

class Program
{
    static void Main()
    {
        // Retrieve connection string from Config class
        string connectionString = Config.GetConnectionString();

        // Instantiate DataAccess class with the retrieved connection string
        using (var dAccess = new DataAccess(connectionString))
        {
            // Open connection (no need to explicitly call OpenConnection method as it's called in the DataAccess constructor)

            Library newLibrary = new Library
            {
                title = "sjjhhjhrasdfghjk",
                author_id = 5,
                genre_id = 5,
                publication_year = 2025
            };

            dAccess.AddLibrary(newLibrary);

            // Retrieve all books
            var books = dAccess.GetAllBooks();
            foreach (var book in books)
            {
                Console.WriteLine($"Title: {book.title}, Author ID: {book.author_id}, Genre ID: {book.genre_id}, Publication Year: {book.publication_year}");
            }

            // Update publication year for a specific book
            int bookIdToUpdate = 2;
            int newPublicationYear = 1999;
            dAccess.UpdateTotalGross(bookIdToUpdate, newPublicationYear);

            // Delete books by genre
            int genreIdToDelete = 3;
            dAccess.DeleteBooksByGenre(genreIdToDelete);

            // Soft delete books by genre
            int genreIdToSoftDelete = 3;
            dAccess.SoftDeleteBooksByGenre(genreIdToSoftDelete);
        }
    }
}




         
 