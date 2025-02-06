using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateMessenger.DataAccess.Repository.IRepository;

namespace PrivateMessengerAPI.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "Menager,Administrator")]
        [HttpPost("users/{id}/approve")]
        public async Task<IActionResult> ApproveUser(int id)
        {
            var user = await _unitOfWork.User.GetAsync(u => u.Id == id);
            if (user == null) return NotFound("User not found");

            user.IsApproved = true;
            await _unitOfWork.User.UpdateAsync(user);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = $"User {user.Tag} has been approved." });
        }
    }
}
