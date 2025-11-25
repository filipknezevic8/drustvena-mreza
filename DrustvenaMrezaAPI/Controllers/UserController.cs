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

        public UserController(IConfiguration configuration)
        {
            userDbRepository = new UserDbRepository(configuration);
        }

        [HttpGet]
        public ActionResult GetPaged([FromQuery] int pageSize = 3, [FromQuery] int page = 1)
        {
            if (pageSize < 1 ||  page < 1)
            {
                return BadRequest("Page and PageSize must be greater than zero.");
            }
            try
            {
                List<User> users = userDbRepository.GetPaged(pageSize, page);
                int totalCount = userDbRepository.CountAll();
                Object result = new
                {
                    Data = users,
                    TotalCount = totalCount
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while fetching the users.");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            try
            {
                User user = userDbRepository.GetById(id);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while fetching the user.");
            }
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

            try
            {
                return Ok(userDbRepository.Create(newUser));
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while creating the user.");
            }
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

            try
            {
                bool result = userDbRepository.Update(updatedUser);

                if (result == false)
                {
                    return NotFound();
                }

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while updating the user.");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                bool result = userDbRepository.Delete(id);

                if (result == false)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while deleting the user.");
            }
        }
    }
}
