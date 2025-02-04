using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrivateMessenger.Models;
using PrivateMessenger.DataAccess.Data;
using PrivateMessenger.Services;
using PrivateMessenger.DTOs;
using PrivateMessenger.DataAccess.Repository.IRepository;


namespace PrivateMessengerWeb.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AuthService _authService;

        public AuthController(IUnitOfWork unitOfWork, AuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
        {
            if (await _unitOfWork.User.GetAsync(u => u.Tag == request.Tag) != null)
                return BadRequest("This Tag is already taken");

            var user = new User
            {
                Tag = request.Tag,
                UserName = request.UserName,
                PasswordHash = _authService.HashPassword(request.Password),
                IsApproved = false
            };

            await _unitOfWork.User.AddAsync(user);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Registration submitted. Awaiting admin approval." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto request)
        {
            var user = await _unitOfWork.User.GetAsync(u => u.Tag == request.Tag);
            if (user == null || !_authService.VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            if (!user.IsApproved)
                return Unauthorized("Your account is pending admin approval.");

            var token = _authService.GenerateJwtToken(user);
            return Ok(new { message = "Login successful!", token });
        }
    }
}