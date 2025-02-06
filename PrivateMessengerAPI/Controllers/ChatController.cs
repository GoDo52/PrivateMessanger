using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateMessenger.DataAccess.Repository.IRepository;
using PrivateMessenger.Models;
using PrivateMessenger.Services;

namespace PrivateMessengerAPI.Controllers
{
    [ApiController]
    [Route("api/chats")]
    public class ChatController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public ChatController(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAllChats()
        {
            var chats = await _unitOfWork.Chat.GetAllAsync();
            return Ok(chats);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChat(int id)
        {
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            if (currentUser.Id != id || currentUser.AdministrationRoleId < 2) return Forbid(); // Admin (2) or SuperAdmin (3)

            var chat = await _unitOfWork.Chat.GetAsync(c => c.Id == id);
            if (chat == null) return NotFound();

            return Ok(chat);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateChat([FromBody] Chat chat)
        {
            await _unitOfWork.Chat.AddAsync(chat);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetChat), new { id = chat.Id }, chat);
        }

        [Authorize]
        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateChat(int id, [FromBody] Chat chat)
        {
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            if (currentUser.Id != id || currentUser.AdministrationRoleId < 2) return Forbid(); // Admin (2) or SuperAdmin (3)

            var chatFromDb = await _unitOfWork.Chat.GetAsync(c => c.Id == id);
            if (chatFromDb == null) return NotFound();

            chatFromDb.Name = chat.Name;

            await _unitOfWork.Chat.UpdateAsync(chatFromDb);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChat(int id)
        { 
            var chat = await _unitOfWork.Chat.GetAsync(c => c.Id == id);
            if (chat == null) return NotFound();

            await _unitOfWork.Chat.RemoveAsync(chat);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
