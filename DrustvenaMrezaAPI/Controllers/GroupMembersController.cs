using DrustvenaMrezaAPI.Models;
using DrustvenaMrezaAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrustvenaMrezaAPI.Controllers
{
    [Route("api/groups/{groupId}/users")]
    [ApiController]
    public class GroupMembersController : ControllerBase
    {
        private GroupRepository groupRepository = new GroupRepository();
        private UserRepository userRepository = new UserRepository();

        [HttpGet]
        public ActionResult<List<User>> GetGroupMembers(int groupId)
        {
            if (!GroupRepository.Data.ContainsKey(groupId))
            {
                return NotFound();
            }

            Group group = GroupRepository.Data[groupId];
            List<User> korisnici = new List<User>();

            foreach (int userId in group.MemberIds)
            {
                if (UserRepository.Data.ContainsKey(userId))
                {
                    korisnici.Add(UserRepository.Data[userId]);
                }
            }

            return Ok(korisnici);
        }
    }
}
