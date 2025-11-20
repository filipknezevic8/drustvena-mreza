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

        private UserDbRepository userDbRepository;

        public UserController()
        {
            userDbRepository = new UserDbRepository();
        }

        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            List<User> users = UserDbRepository.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            User user = UserDbRepository.GetById(id);
            
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
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
