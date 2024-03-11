

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using System.Linq;

namespace LibraryDataAccess
{
    public class DataAccess : IDisposable, IDataAccess
    {
        private readonly string connectionString;
        private MySqlConnection connection;

        public DataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        private void EnsureConnectionIsOpen()
        {
            if (connection == null)
            {
                connection = GetConnection();
            }

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        
        public void AddLibrary(Library newLibrary)
{
    EnsureConnectionIsOpen();

    if (newLibrary == null)
    {
        Console.WriteLine("Error adding book: Library object is null.");
        return;
    }

    try
    {
        // Retrieve severity level and price based on the damage type
        (int severityLevel, decimal price) = GetSeverityLevelAndPriceByDamageType(newLibrary.Damage);

        // Add the new book to the database
        string query = "INSERT INTO books (title, author_id, genre_id, publication_year, Damage, RepairStatus) " +
                       "VALUES (@title, @author_id, @genre_id, @publication_year, @Damage, @RepairStatus)";
        using (MySqlCommand cmd = new MySqlCommand(query, connection))
        {
            cmd.Parameters.AddWithValue("@title", newLibrary.title);
            cmd.Parameters.AddWithValue("@author_id", newLibrary.author_id);
            cmd.Parameters.AddWithValue("@genre_id", newLibrary.genre_id);
            cmd.Parameters.AddWithValue("@publication_year", newLibrary.publication_year);
            cmd.Parameters.AddWithValue("@Damage", newLibrary.Damage);

            // Get the status based on the budget
            decimal budget = 1000; // Replace 1000 with your actual budget
            List<string> statuses = GetDamageTypesWithinBudget(budget);
            string repairStatus = statuses.FirstOrDefault(); // Assuming only one status per book for simplicity

            cmd.Parameters.AddWithValue("@RepairStatus", repairStatus);

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
                string query = "SELECT book_id, title, author_id, genre_id, publication_year FROM books";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Library library = new Library
                        {
                            bookId = Convert.ToInt32(reader["book_id"]),
                            title = reader["title"].ToString(),
                            author_id = Convert.ToInt32(reader["author_id"]),
                            genre_id = Convert.IsDBNull(reader["genre_id"]) ? 0 : Convert.ToInt32(reader["genre_id"]),
                            publication_year = Convert.IsDBNull(reader["publication_year"]) ? 0 : Convert.ToInt32(reader["publication_year"]),
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

        // public bool UpdateBook(Library updatedBook)
        // {
        //     EnsureConnectionIsOpen();

        //     try
        //     {
        //         string query = "UPDATE books SET title = @title, author_id = @author_id, "
        //                      + "genre_id = @genre_id, publication_year = @publication_year "
        //                      + "WHERE book_id = @bookId";
        //         using (MySqlCommand cmd = new MySqlCommand(query, connection))
        //         {
        //             cmd.Parameters.AddWithValue("@title", updatedBook.title);
        //             cmd.Parameters.AddWithValue("@author_id", updatedBook.author_id);
        //             cmd.Parameters.AddWithValue("@genre_id", updatedBook.genre_id);
        //             cmd.Parameters.AddWithValue("@publication_year", updatedBook.publication_year);
        //             cmd.Parameters.AddWithValue("@bookId", updatedBook.bookId);
        //             int rowsAffected = cmd.ExecuteNonQuery();
        //             Console.WriteLine($"Book updated for {rowsAffected} record(s).");

        //             return rowsAffected > 0;
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"Error updating book: {ex.Message}");
        //         return false;
        //     }
        // }
        public bool UpdateBookById(int bookId, Library updatedBook)
{
    EnsureConnectionIsOpen();

    try
    {
        if (!BookExists(bookId))
        {
            Console.WriteLine($"Error updating book: Book ID {bookId} does not exist.");
            return false;
        }

        string query = "UPDATE books SET title = @title, author_id = @author_id, "
                     + "genre_id = @genre_id, publication_year = @publication_year, Damage = @Damage "
                     + "WHERE book_id = @bookId";
        using (MySqlCommand cmd = new MySqlCommand(query, connection))
        {
            cmd.Parameters.AddWithValue("@title", updatedBook.title);
            cmd.Parameters.AddWithValue("@author_id", updatedBook.author_id);
            cmd.Parameters.AddWithValue("@genre_id", updatedBook.genre_id);
            cmd.Parameters.AddWithValue("@publication_year", updatedBook.publication_year);
            cmd.Parameters.AddWithValue("@Damage", updatedBook.Damage);
            cmd.Parameters.AddWithValue("@bookId", bookId);
            int rowsAffected = cmd.ExecuteNonQuery();
            Console.WriteLine($"Book updated for {rowsAffected} record(s).");

            return rowsAffected > 0;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error updating book: {ex.Message}");
        return false;
    }
}


       
        public bool DeleteBookById(int bookId)
{
    EnsureConnectionIsOpen();

    try
    {
        if (!BookExists(bookId))
        {
            Console.WriteLine($"Error deleting book: Book ID {bookId} does not exist.");
            return false;
        }

        string deleteBookQuery = "DELETE FROM books WHERE book_id = @bookId";
        using (MySqlCommand deleteBookCmd = new MySqlCommand(deleteBookQuery, connection))
        {
            deleteBookCmd.Parameters.AddWithValue("@bookId", bookId);
            int booksDeleted = deleteBookCmd.ExecuteNonQuery();
            Console.WriteLine($"{booksDeleted} book deleted with ID {bookId}.");

            return booksDeleted > 0;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error deleting book: {ex.Message}");
        return false;
    }
}

private bool BookExists(int bookId)
{
    try
    {
        string query = "SELECT COUNT(*) FROM books WHERE book_id = @bookId";
        using (MySqlCommand cmd = new MySqlCommand(query, connection))
        {
            cmd.Parameters.AddWithValue("@bookId", bookId);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count > 0;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error checking book existence: {ex.Message}");
        return false;
    }
}

        public Library GetBookById(int bookId)
        {
            EnsureConnectionIsOpen();

            try
            {
                string query = "SELECT * FROM books WHERE book_id = @bookId";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@bookId", bookId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Library library = new Library
                            {
                                bookId = Convert.ToInt32(reader["book_id"]),
                                title = reader["title"].ToString(),
                                author_id = Convert.ToInt32(reader["author_id"]),
                                genre_id = Convert.IsDBNull(reader["genre_id"]) ? 0 : Convert.ToInt32(reader["genre_id"]),
                                publication_year = Convert.IsDBNull(reader["publication_year"]) ? 0 : Convert.ToInt32(reader["publication_year"])
                            };

                            return library;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving book by ID: {ex.Message}");
            }

            return null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("Connection closed.");
                }
            }
        }

        public (int severityLevel, decimal price) GetSeverityLevelAndPriceByDamageType(string damageType)
        {
            EnsureConnectionIsOpen();

            try
            {
                string query = "SELECT severity_level, price FROM Damage WHERE damage_type = @damageType";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@damageType", damageType);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int severityLevel = Convert.ToInt32(reader["severity_level"]);
                            decimal price = Convert.ToDecimal(reader["price"]);
                            return (severityLevel, price);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving severity level and price by damage type: {ex.Message}");
            }

            return (-1, -1); // Return (-1, -1) if severity_level and price not found or an error occurs
        }

       public List<string> GetDamageTypesWithinBudget(decimal budget)
{
    EnsureConnectionIsOpen();

    List<string> statuses = new List<string>();

    try
    {
        string query = "SELECT b.book_id, SUM(d.price) AS total_price " +
                       "FROM books b " +
                       "JOIN Damage d ON b.Damage = d.damage_type " +
                       "GROUP BY b.book_id";
        using (MySqlCommand cmd = new MySqlCommand(query, connection))
        using (MySqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                decimal totalPrice = Convert.ToDecimal(reader["total_price"]);
                string status = GetStatus(totalPrice, budget);
                statuses.Add(status);
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error retrieving book statuses: {ex.Message}");
    }

    return statuses;
}

private string GetStatus(decimal totalPrice, decimal budget)
{
    if (totalPrice > budget)
    {
        return "Pending";
    }
    else if (totalPrice <= budget)
    {
        return "Completed";
    }
    else
    {
        return "In Progress";
    }
}

    }
}




