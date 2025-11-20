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

            return Ok(UserDbRepository.Create(newUser));
        }

        [HttpPut("{id}")]
        public ActionResult<User> Update(int id, [FromBody] User updatedUser)
        {
            if (string.IsNullOrWhiteSpace(updatedUser.Username) ||
                string.IsNullOrWhiteSpace(updatedUser.FirstName) ||
                string.IsNullOrWhiteSpace(updatedUser.LastName))
            {
                return BadRequest();
            }

            bool result = UserDbRepository.Update(updatedUser);

            if (result == false)
            {
                return NotFound();
            }

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            bool result = UserDbRepository.Delete(id);

            if (result == false)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
