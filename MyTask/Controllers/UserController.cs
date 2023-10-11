using Microsoft.AspNetCore.Mvc;
using MyTask.DataModels;
using MyTask.Interfaces;
using MyTask.Mappers;
using MyTask.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.Json;
using System.Linq.Dynamic.Core;

namespace MyTask.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class UserController : Controller
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly DatabaseContext _context;

        public UserController(IUserRepository userRepository, IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository, DatabaseContext context)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserRole>))]
        public IActionResult GetUsers([FromQuery] PageParams pageParams)
        {
            if (pageParams.Pages < 1 || pageParams.PageSize < 1 || pageParams.OrderBy == null)
                return BadRequest(ModelState);

            var users = _userRepository.GetUsers(pageParams);

            if (users == null)
            {
                ModelState.AddModelError("", "Users not found!");
                return StatusCode(422, ModelState);
            }

            var userRolesModels = new List<UserRolesModel>();

            foreach (var user in users)
            {
                var roles = new List<Role>();
                var userRoles = (List<UserRole>)_userRoleRepository.GetByUserId(user);

                foreach (var userRole in userRoles)
                {
                    roles.Add(_roleRepository.GetRoleByRoleId(userRole.RoleId));
                }

                userRolesModels.Add(UsersMapper.Map(user, roles));
            }

            if (pageParams.FilteringByUserId != null)
            {
                userRolesModels = userRolesModels.Where(a => a.User.UserId == pageParams.FilteringByUserId).ToList();
            }

            if (pageParams.FilteringByUserAge != null)
            {
                userRolesModels = userRolesModels.Where(a => a.User.UserAge == pageParams.FilteringByUserAge).ToList();
            }

            if (pageParams.FilteringByRoleId != null)
            {
                List<UserRolesModel> roles = new List<UserRolesModel>();

                foreach (var userRolesModel in userRolesModels)
                {
                    if(userRolesModel.RolesModel.Where(a => a.RoleId == pageParams.FilteringByRoleId).ToList().Count > 0)
                    {
                        roles.Add(userRolesModel);
                    }
                }

                userRolesModels = roles;
            }

            if (pageParams.FilteringByUserName != null)
            {
                userRolesModels = userRolesModels.Where(a => a.User.UserName == pageParams.FilteringByUserName).ToList();
            }

            if (pageParams.FilteringByUserEmail != null)
            {
                userRolesModels = userRolesModels.Where(a => a.User.UserEmail == pageParams.FilteringByUserEmail).ToList();
            }

            if (pageParams.FilteringByRoleName != null)
            {
                List<UserRolesModel> roles = new List<UserRolesModel>();

                foreach (var userRolesModel in userRolesModels)
                {
                    if (userRolesModel.RolesModel.Where(a => a.RoleName == pageParams.FilteringByRoleName).ToList().Count > 0)
                    {
                        roles.Add(userRolesModel);
                    }
                }

                userRolesModels = roles;
            }

            PageMetadata<UserRolesModel> answer = PageMetadata<UserRolesModel>.ToPagedPaginationList(userRolesModels.AsQueryable(), pageParams.Pages, pageParams.PageSize);

            var metadata = new
            {
                answer.TotalCount,
                answer.PageSize,
                answer.CurrentPage,
                answer.TotalPages,
                answer.HasNext,
                answer.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(answer);
        }
       
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Role>))]
        public IActionResult GetRoles()
        {
            var roles = (List<Role>)_roleRepository.GetRoles();

            if (roles == null)
            {
                ModelState.AddModelError("", "Roles not found!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(roles);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(UserRolesModel))]
        [ProducesResponseType(400)]
        public IActionResult GetUserById(int userId)
        {
            if(!_userRepository.UserExists(userId))
            {
                ModelState.AddModelError("", "User not found!");
                return StatusCode(422, ModelState);
            }

            User user = _userRepository.GetUserByUserId(userId);
            var userRoles = (List<UserRole>)_userRoleRepository.GetByUserId(user);
            var roles = new List<Role>();

            foreach (var userRole in userRoles)
            {
                roles.Add(_roleRepository.GetRoleByRoleId(userRole.RoleId));
            }

            UserRolesModel userRolesModel = UsersMapper.Map(user, roles);
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(userRolesModel);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddNewUserRole([FromQuery] int userId, [FromQuery] int roleId)
        {
            if (!_userRepository.UserExists(userId))
            {
                ModelState.AddModelError("", "User not found!");
                return StatusCode(422, ModelState);
            }

            if (!_roleRepository.RoleExists(roleId))
            {
                ModelState.AddModelError("", "Role not found!");
                return StatusCode(422, ModelState);
            }

            UserRole userRole = _userRoleRepository.GetUserRoles().Where(c => c.UserId == userId
                    && c.RoleId == roleId).FirstOrDefault();

            if (userRole != null)
            {
                ModelState.AddModelError("", "UserRole already exists!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (!_userRoleRepository.AddNewUserRole(userId, roleId))
            {
                ModelState.AddModelError("", "Something went wrong while savin!");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created!");
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromQuery] List<int> rolesId, 
            [FromQuery] string userName, [FromQuery] int? userAge, [FromQuery] string userEmail)
        {
            if(rolesId.GroupBy(x=>x).Where(g => g.Count() > 1).Select(y => y.Key).ToList().Count > 0) 
            {
                ModelState.AddModelError("", "Roles should be different!");
                return StatusCode(422, ModelState);
            }

            foreach (int roleId in rolesId)
            {
                if (!_roleRepository.RoleExists(roleId))
                {
                    ModelState.AddModelError("", $"Role (roleId:{roleId.ToString()})  not found!");
                    return StatusCode(422, ModelState);
                }
            }

            if (userName == null) 
            {
                ModelState.AddModelError("", "Name shouldn't be empty!");
                return StatusCode(422, ModelState);
            }

            if (userAge == null)
            {
                ModelState.AddModelError("", "Age shouldn't be empty!");
                return StatusCode(422, ModelState);
            }

            if (userEmail == null)
            {
                ModelState.AddModelError("", "Email shouldn't be empty!");
                return StatusCode(422, ModelState);
            }

            if ((int)userAge < 1)
            {
                ModelState.AddModelError("", "Age must be greater than 0!");
                return StatusCode(422, ModelState);
            }

            if (_userRepository.GetUserByEmail(userEmail) != null)
            {
                ModelState.AddModelError("", "Email already exists!");
                return StatusCode(422, ModelState);
            }
                       
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var user = new User()
            {
                UserName = userName,
                UserAge = (int)userAge,
                UserEmail = userEmail
            };

            if (!_userRepository.CreateUser(rolesId, user))
            {
                ModelState.AddModelError("", "Something went wrong while savin!");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created!");
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUserByUserId(int userId, [FromQuery] List<int> rolesId,
            [FromQuery] string userName, [FromQuery] int? userAge, [FromQuery] string userEmail)
        {
            if (userName == null || rolesId == null || userEmail == null
                || userAge == null)
                return BadRequest(ModelState);

            if (userAge < 1)
            {
                ModelState.AddModelError("", "Age must be greater than 0!");
                return StatusCode(422, ModelState);
            }

            User user = _userRepository.GetUserByEmail(userEmail);

            if (user != null && user.UserId != userId)
            {
                ModelState.AddModelError("", "Email already exists!");
                return StatusCode(422, ModelState);
            }
            else
            {
                user = _userRepository.GetUserByUserId(userId);
            }

            foreach (var roleId in rolesId)
            {
                if (!_roleRepository.RoleExists(roleId))
                {
                    ModelState.AddModelError("", $"Role (roleId:{roleId.ToString()}) not found!");
                    return StatusCode(422, ModelState);
                }
            }

            if (!_userRepository.UserExists(userId))
            {
                ModelState.AddModelError("", "User not found!");
                return StatusCode(422, ModelState);
            }


            user.UserId = userId;
            user.UserName = userName;
            user.UserAge = (int)userAge;
            user.UserEmail = userEmail;

            var oldUserRoles = _userRoleRepository.GetByUserId(user).ToList();

            if (oldUserRoles.Count != rolesId.Count)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest();

            for (int i = 0; i < oldUserRoles.Count; i++)
            {
                if (!_userRoleRepository.UpdateUserRole(oldUserRoles[i], rolesId[i]))
                {
                    ModelState.AddModelError("", "Something went wrong updating userRoles!");
                    return StatusCode(500, ModelState);
                }
            }

            if (!_userRepository.UpdateUser(user))
            {
                ModelState.AddModelError("", "Something went wrong updating user!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUserByUserId(int userId)
        {
            if (!_userRepository.UserExists(userId))
            {
                ModelState.AddModelError("", "User not found!");
                return StatusCode(422, ModelState);
            }

            var userToDelete = _userRepository.GetUserByUserId(userId);
            var userRolesToDelete = _userRoleRepository.GetByUserId(userToDelete).ToList();


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRoleRepository.DeleteUserRoles(userRolesToDelete))
            {
                ModelState.AddModelError("", "Something went wrong when deleting userRoles!");
            }

            if (!_userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting user!");
            }

            return NoContent();
        }
    }
}
