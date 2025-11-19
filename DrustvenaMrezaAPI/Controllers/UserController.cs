using DrustvenaMrezaAPI.Models;
using DrustvenaMrezaAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace DrustvenaMrezaAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private GroupRepository groupRepository = new GroupRepository();
        private UserRepository userRepository = new UserRepository();

        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            List<User> users = GetAllFromDatabase();
            return Ok(users);
        }

        private static List<User> GetAllFromDatabase()
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

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            if (!UserRepository.Data.ContainsKey(id))
            {
                return NotFound();
            }

            return Ok(UserRepository.Data[id]);
        }

        [HttpPost]
        public ActionResult<User> Create([FromBody] User newUser)
        {
            if (string.IsNullOrWhiteSpace(newUser.Username) ||
                string.IsNullOrWhiteSpace(newUser.FirstName) ||
                string.IsNullOrWhiteSpace(newUser.LastName))
            {
                return BadRequest();
            }

            int newId = CalculateNewId(UserRepository.Data.Keys.ToList());
            newUser.Id = newId;
            UserRepository.Data[newId] = newUser;
            userRepository.Save();

            return Ok(newUser);
        }

        [HttpPut("{id}")]
        public ActionResult<User> Update(int id, [FromBody] User updatedUser)
        {
            if (!UserRepository.Data.ContainsKey(id))
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(updatedUser.Username) ||
                string.IsNullOrWhiteSpace(updatedUser.FirstName) ||
                string.IsNullOrWhiteSpace(updatedUser.LastName))
            {
                return BadRequest();
            }

            User user = UserRepository.Data[id];
            user.Username = updatedUser.Username;
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.BirthDate = updatedUser.BirthDate;

            userRepository.Save();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!UserRepository.Data.ContainsKey(id))
            {
                return NotFound();
            }

            UserRepository.Data.Remove(id);
            userRepository.Save();

            return NoContent();
        }

        private int CalculateNewId(List<int> ids)
        {
            if (ids.Count == 0) return 1;
            return ids.Max() + 1;
        }
    }
}
