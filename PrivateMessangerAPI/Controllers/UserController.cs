using Microsoft.AspNetCore.Mvc;
using PrivateMessanger.DataAccess.Repository.IRepository;
using PrivateMessanger.Models;

namespace PrivateMessengerAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _unitOfWork.User.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _unitOfWork.User.GetAsync(u => u.Id == id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            await _unitOfWork.User.AddAsync(user);
            await _unitOfWork.SaveAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            var userFromDb = await _unitOfWork.User.GetAsync(u => u.Id == id);
            if (userFromDb == null) return NotFound();

            userFromDb.UserName = user.UserName;
            userFromDb.Tag = user.Tag;

            await _unitOfWork.User.UpdateAsync(user);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

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
