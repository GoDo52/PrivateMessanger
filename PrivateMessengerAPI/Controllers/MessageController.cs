using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateMessenger.DataAccess.Repository.IRepository;
using PrivateMessenger.Models;
using PrivateMessenger.Services;

namespace PrivateMessengerAPI.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public MessageController(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        [Authorize(Roles = ("Administrator"))]
        [HttpGet]
        public async Task<IActionResult> GetAllMessages()
        {
            var messages = await _unitOfWork.Message.GetAllAsync();
            return Ok(messages);
        }

        [Authorize]
        [HttpGet("chat/{chatId}")]
        public async Task<IActionResult> GetMessagesForChat(int chatId)
        {
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            var isChatMember = await _unitOfWork.UserChat.GetAsync(uc => uc.UserId == currentUser.Id && uc.ChatId == chatId);

            if (isChatMember == null || currentUser.AdministrationRoleId < 2) return Forbid(); // Must be in the chat to see messages or administrator roles

            var messages = await _unitOfWork.Message.GetAllAsync(m => m.Id == chatId);
            return Ok(messages);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage(int id)
        {
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            var message = await _unitOfWork.Message.GetAsync(m => m.Id == id);
            if (message == null) return NotFound();

            var isChatMember = await _unitOfWork.UserChat.GetAsync(uc => uc.UserId == currentUser.Id && uc.ChatId == message.ChatId);
            if (isChatMember == null || currentUser.AdministrationRoleId < 2) return Forbid(); // Must be in the chat to see messages or administrator roles

            return Ok(message);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] Message message)
        {
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            var isChatMember = await _unitOfWork.UserChat.GetAsync(uc => uc.UserId == currentUser.Id && uc.ChatId == message.ChatId);

            if (isChatMember == null) return Forbid(); // Must be a chat member to send messages

            message.SenderId = currentUser.Id; // Ensure that the sender is the authenticated user

            await _unitOfWork.Message.AddAsync(message);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetMessage), new { id = message.Id }, message);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(int id, [FromBody] Message message)
        {
            var currentUser = await _currentUserService.GetCurrentUserAsync();

            var messageFromDb = await _unitOfWork.Message.GetAsync(m => m.Id == id);
            if (messageFromDb == null) return NotFound();

            if (messageFromDb.SenderId != currentUser.Id) return Forbid();

            messageFromDb.TextContent = message.TextContent;

            await _unitOfWork.Message.UpdateAsync(messageFromDb);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            var message = await _unitOfWork.Message.GetAsync(m => m.Id == id);
            if (message == null) return NotFound();

            var isChatMember = await _unitOfWork.UserChat.GetAsync(uc => uc.UserId == currentUser.Id && uc.ChatId == message.ChatId);
            bool isAppAdmin = currentUser.AdministrationRoleId >= 2;
            bool isChatAdmin = isChatMember != null && isChatMember.RoleId >= 2;

            if (message.SenderId == currentUser.Id || isChatAdmin || isAppAdmin)
            {
                await _unitOfWork.Message.RemoveAsync(message);
                await _unitOfWork.SaveAsync();
                return NoContent();
            }
            else return Forbid(); // Only the sender, chat admins, or app admins can delete messages
        }
    }
}
