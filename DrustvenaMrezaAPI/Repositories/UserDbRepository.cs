using System.Runtime.InteropServices;
using DrustvenaMrezaAPI.Models;
using Microsoft.Data.Sqlite;

namespace DrustvenaMrezaAPI.Repositories
{
    public class UserDbRepository
    {
        public static List<User> GetAll()
        {
            List<User> users = new List<User>();
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=database/korisnici-grupe.db");
                connection.Open();

                string query = "SELECT * FROM Users";
                using SqliteCommand command = new SqliteCommand(query, connection);

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int Id = Convert.ToInt32(reader["Id"]);
                    string Username = reader["Username"].ToString();
                    string FirstName = reader["Name"].ToString();
                    string LastName = reader["Surname"].ToString();
                    DateTime BirthDate = Convert.ToDateTime(reader["Birthday"]);
                    User user = new User(Id, Username, FirstName, LastName, BirthDate);
                    users.Add(user);
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }
            return users;
        }

        public static User GetById (int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=database/korisnici-grupe.db");
                connection.Open();

                string query = "SELECT * FROM Users WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);
                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int Id = Convert.ToInt32(reader["Id"]);
                    string Username = reader["Username"].ToString();
                    string FirstName = reader["Name"].ToString();
                    string LastName = reader["Surname"].ToString();
                    DateTime BirthDate = Convert.ToDateTime(reader["Birthday"]);
                    User user = new User(Id, Username, FirstName, LastName, BirthDate);
                    return user;
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }
            return null;
        }

        public static User Create (User user)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=database/korisnici-grupe.db");
                connection.Open();

                string query = @"INSERT INTO Users (Username, Name, Surname, Birthday) VALUES (@Username, @Name, @Surname, @Birthday); SELECT LAST_INSERT_ROWID();";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Name", user.FirstName);
                command.Parameters.AddWithValue("@Surname", user.LastName);
                command.Parameters.AddWithValue("@Birthday", user.BirthDate);
                user.Id = Convert.ToInt32(command.ExecuteScalar());
                return user;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }
            return null;
        }

        public static bool Update (User user)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=database/korisnici-grupe.db");
                connection.Open();

                string query = @"UPDATE Users SET Username = @Username, Name = @Name, Surname = @Surname, Birthday = @Birthday WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Name", user.FirstName);
                command.Parameters.AddWithValue("@Surname", user.LastName);
                command.Parameters.AddWithValue("@Birthday", user.BirthDate);
                command.Parameters.AddWithValue("@Id", user.Id);
                int affectedRows = command.ExecuteNonQuery();
                return affectedRows > 0;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }
            return false;
        }

        public static bool Delete (int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=database/korisnici-grupe.db");
                connection.Open();

                string query = @"DELETE FROM Users WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                int affectedRows = command.ExecuteNonQuery();
                return affectedRows > 0;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }
            return false;
        }
    }
}
