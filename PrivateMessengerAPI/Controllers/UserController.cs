using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateMessenger.DataAccess.Repository.IRepository;
using PrivateMessenger.Models;
using PrivateMessenger.Services;

namespace PrivateMessengerAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UserController(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        [Authorize(Roles = "Manager,Administrator")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _unitOfWork.User.GetAllAsync();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            if (currentUser.Id != id || currentUser.AdministrationRoleId < 2) return Forbid(); // Admin (2) or SuperAdmin (3)

            var user = await _unitOfWork.User.GetAsync(u => u.Id == id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            await _unitOfWork.User.AddAsync(user);
            await _unitOfWork.SaveAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {

            var currentUser = await _currentUserService.GetCurrentUserAsync();
            if (currentUser.Id != id || currentUser.AdministrationRoleId < 2) return Forbid(); // Admin (2) or SuperAdmin (3)

            var userFromDb = await _unitOfWork.User.GetAsync(u => u.Id == id);
            if (userFromDb == null) return NotFound();

            userFromDb.UserName = user.UserName;
            userFromDb.Tag = user.Tag;

            await _unitOfWork.User.UpdateAsync(user);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _unitOfWork.User.GetAsync(u => u.Id == id);
            if (user == null) return NotFound();

            await _unitOfWork.User.RemoveAsync(user);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
