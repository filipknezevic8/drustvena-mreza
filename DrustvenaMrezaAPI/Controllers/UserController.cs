using DrustvenaMrezaAPI.Models;
using DrustvenaMrezaAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(UserRepository.Data.Values.ToList());
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

        private int CalculateNewId(List<int> ids)
        {
            if (ids.Count == 0) return 1;
            return ids.Max() + 1;
        }
    }
}
