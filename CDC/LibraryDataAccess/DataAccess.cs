



using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

public class DataAccess : IDisposable
{
    private readonly string connectionString;
    private MySqlConnection connection;

    public DataAccess(string connectionString)
    {
       
        this.connectionString = connectionString;
        OpenConnection();
    }

    public void OpenConnection()
    {
        connection = new MySqlConnection(connectionString);
        try
        {
            connection.Open();
            Console.WriteLine("Connection opened successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error opening connection: {ex.Message}");
        }
    }

    private void EnsureConnectionIsOpen()//to check if the database connection is open
    {
        if (connection.State != ConnectionState.Open)
        {
            throw new InvalidOperationException("Connection is not open. Cannot perform database operation.");
        }
    }

   
    
     public void AddLibrary(Library newLibrary)
    {
        if (newLibrary == null)
        {
            Console.WriteLine("Error adding book: Library object is null.");
            return;
        }

        if (string.IsNullOrWhiteSpace(newLibrary.title))
        {
            Console.WriteLine("Error adding book: Title cannot be empty.");
            return;
        }

        if (newLibrary.author_id <= 0)
        {
            Console.WriteLine("Error adding book: Invalid author_id.");
            return;
        }

        if (newLibrary.genre_id <= 0)
        {
            Console.WriteLine("Error adding book: Invalid genre_id.");
            return;
        }

        // Validate publication year to be exactly four characters long
        string publicationYearString = newLibrary.publication_year.ToString();
        if (publicationYearString.Length != 4)
        {
            Console.WriteLine("Error adding book: Publication year must be 4 characters long.");
            return;
        }

        EnsureConnectionIsOpen();

        try
        {
            string query = "INSERT INTO books (title, author_id, genre_id, publication_year) " +
                           "VALUES (@title, @author_id, @genre_id, @publication_year)";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@title", newLibrary.title);
                cmd.Parameters.AddWithValue("@author_id", newLibrary.author_id);
                cmd.Parameters.AddWithValue("@genre_id", newLibrary.genre_id);
                cmd.Parameters.AddWithValue("@publication_year", newLibrary.publication_year);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Book added successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding book: {ex.Message}");
        }
    }

    public List<Library> GetAllBooks()
    {
        EnsureConnectionIsOpen();

        List<Library> books = new List<Library>();

        try
        {
            string query = "SELECT * FROM books";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Library library = new Library
                    {
                        title = reader["title"].ToString(),
                        author_id = Convert.ToInt32(reader["author_id"]),
                        genre_id = Convert.IsDBNull(reader["genre_id"]) ? 0 : Convert.ToInt32(reader["genre_id"]),
                        publication_year = Convert.IsDBNull(reader["publication_year"]) ? 0 : Convert.ToInt32(reader["publication_year"])
                    };

                    books.Add(library);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving books: {ex.Message}");
        }

        return books;
    }

    public void UpdateTotalGross(int bookId, int newPublicationYear)
    {
        EnsureConnectionIsOpen();

        try
        {
            string query = "UPDATE books SET publication_year = @newPublicationYear WHERE book_id = @bookId";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@newPublicationYear", newPublicationYear);
                cmd.Parameters.AddWithValue("@bookId", bookId);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Publication year updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating publication year: {ex.Message}");
        }
    }

    
public void DeleteBooksByGenre(int genreId)
{
    EnsureConnectionIsOpen();

    try
    {
        // Check if the genreId exists in the books table
        if (!GenreExists(genreId))
        {
            Console.WriteLine($"Error deleting books by genre: Genre ID {genreId} does not exist.");
            return;
        }

        // Delete referencing records in the borrowers table
        string deleteBorrowersQuery = "DELETE FROM borrowers WHERE book_id IN (SELECT book_id FROM books WHERE genre_id = @genreId)";
        using (MySqlCommand deleteBorrowersCmd = new MySqlCommand(deleteBorrowersQuery, connection))
        {
            deleteBorrowersCmd.Parameters.AddWithValue("@genreId", genreId);
            int borrowersDeleted = deleteBorrowersCmd.ExecuteNonQuery();
            Console.WriteLine($"{borrowersDeleted} borrower records deleted for books with genre_id {genreId}.");
        }

        // Delete books with the specified genre_id
        string deleteBooksQuery = "DELETE FROM books WHERE genre_id = @genreId";
        using (MySqlCommand deleteBooksCmd = new MySqlCommand(deleteBooksQuery, connection))
        {
            deleteBooksCmd.Parameters.AddWithValue("@genreId", genreId);
            int booksDeleted = deleteBooksCmd.ExecuteNonQuery();
            Console.WriteLine($"{booksDeleted} books deleted for genre_id {genreId}.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error deleting books by genre: {ex.Message}");
    }
}

    private bool GenreExists(int genreId)
    {
    try
    {
        string query = "SELECT COUNT(*) FROM books WHERE genre_id = @genreId";
        using (MySqlCommand cmd = new MySqlCommand(query, connection))
        {
            cmd.Parameters.AddWithValue("@genreId", genreId);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count > 0;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error checking genre existence: {ex.Message}");
        return false;
    }
   }

    public void SoftDeleteBooksByGenre(int genreId)
    {
        EnsureConnectionIsOpen();

        try
        {
            // Soft delete referencing records in the borrowers table
            string softDeleteBorrowersQuery = "UPDATE borrowers SET is_deleted = 1 WHERE book_id IN (SELECT book_id FROM books WHERE genre_id = @genreId)";
            using (MySqlCommand softDeleteBorrowersCmd = new MySqlCommand(softDeleteBorrowersQuery, connection))//MySqlCommand object used to execute the soft delete query.
            {
                softDeleteBorrowersCmd.Parameters.AddWithValue("@genreId", genreId);
                int borrowersSoftDeleted = softDeleteBorrowersCmd.ExecuteNonQuery();
                Console.WriteLine($"{borrowersSoftDeleted} borrower records soft deleted for books with genre_id {genreId}.");
            }

            // Soft delete books with the specified genre_id
            string softDeleteBooksQuery = "UPDATE books SET is_deleted = 1 WHERE genre_id = @genreId";
            using (MySqlCommand softDeleteBooksCmd = new MySqlCommand(softDeleteBooksQuery, connection))
            {
                softDeleteBooksCmd.Parameters.AddWithValue("@genreId", genreId);
                int booksSoftDeleted = softDeleteBooksCmd.ExecuteNonQuery();//method is called to execute the soft delete query, which soft deletes the books with the specified genre_id.
                Console.WriteLine($"{booksSoftDeleted} books soft deleted for genre_id {genreId}.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error soft deleting books by genre: {ex.Message}");
        }
    }

    // Dispose method to close the connection

    // Dispose method to close the connection when done
    public void Dispose()
    {
        if (connection != null && connection.State == ConnectionState.Open)
        {
            connection.Close();
            Console.WriteLine("Connection closed.");
        }
    }
}













