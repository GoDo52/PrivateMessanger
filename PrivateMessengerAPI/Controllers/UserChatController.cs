using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateMessenger.DataAccess.Repository.IRepository;
using PrivateMessenger.Models;
using PrivateMessenger.Services;
using System;

namespace PrivateMessengerAPI.Controllers
{
    [ApiController]
    [Route("api/userchats")]

    public class UserChatController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UserChatController(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddUserToChat(int userId, int chatId)
        {
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            var chatAdmin = await _unitOfWork.UserChat.GetAsync(uc => uc.UserId == currentUser.Id && uc.ChatId == chatId);

            bool isChatAdmin = chatAdmin != null && chatAdmin.RoleId >= 2; // Chat Moderator (2) or Admin (3)
            bool isAppAdmin = currentUser.AdministrationRoleId >= 2; // App Admin (2) or SuperAdmin (3
            if (!isChatAdmin && !isAppAdmin) return Forbid(); // Must be chat admin/moderator or app admin

            var user = await _unitOfWork.User.GetAsync(u => u.Id == userId);
            var chat = await _unitOfWork.Chat.GetAsync(c => c.Id == chatId);

            if (user == null || chat == null) return NotFound("User or Chat not found");

            var userChat = new UserChat { UserId = userId, ChatId = chatId };
            await _unitOfWork.UserChat.AddAsync(userChat);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = $"User {user.Tag} added to chat {chat.Name} successfully" });
        }

        [Authorize]
        [HttpDelete]
        [Route("remove")]
        public async Task<IActionResult> RemoveUserFromChat(int userId, int chatId)
        {
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            var chatAdmin = await _unitOfWork.UserChat.GetAsync(uc => uc.UserId == currentUser.Id && uc.ChatId == chatId);

            bool isChatAdmin = chatAdmin != null && chatAdmin.RoleId >= 2; // Chat Moderator (2) or Admin (3)
            bool isAppAdmin = currentUser.AdministrationRoleId >= 2; // App Admin (2) or SuperAdmin (3)
            if (!isChatAdmin && !isAppAdmin) return Forbid(); 

            var userChat = await _unitOfWork.UserChat.GetAsync(uc => uc.UserId == userId && uc.ChatId == chatId);
            if (userChat == null) return NotFound("User is not part of the chat");

            var user = await _unitOfWork.User.GetAsync(u => u.Id == userChat.UserId);
            if (user == null) return NotFound("User not found");
            var chat = await _unitOfWork.Chat.GetAsync(c => c.Id == userChat.ChatId);
            if (chat == null) return NotFound("Chat not found");

            await _unitOfWork.UserChat.RemoveAsync(userChat);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = $"User {user.Tag} removed from chat {chat.Name} successfully" });
        }

        [Authorize]
        [HttpGet("chat/{chatId}/users")]
        public async Task<IActionResult> GetUsersInChat(int chatId)
        {
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            var userInChat = await _unitOfWork.UserChat.GetAsync(uc => uc.UserId == currentUser.Id && uc.ChatId == chatId);

            bool isAppAdmin = currentUser.AdministrationRoleId >= 2; // Admin (2) or SuperAdmin (3)
            bool isChatMember = userInChat != null;
            if (!isChatMember && !isAppAdmin) return Forbid(); // Must be in the chat OR be an app admin

            var userChats = await _unitOfWork.UserChat.GetAllAsync(includeProperties: "User");
            var usersInChat = userChats.Where(uc => uc.ChatId == chatId).Select(uc => uc.User);
            return Ok(usersInChat);
        }

        [Authorize(Roles = ("Manager,Administrator"))]
        [HttpGet("user/{userId}/chats")]
        public async Task<IActionResult> GetChatsForUser(int userId)
        {
            var userChats = await _unitOfWork.UserChat.GetAllAsync(includeProperties: "Chat");
            var chatsForUser = userChats.Where(uc => uc.UserId == userId).Select(uc => uc.Chat);
            return Ok(chatsForUser);
        }

        //Updates role for the user in the chat
        [Authorize]
        [HttpPut("update-role")]
        public async Task<IActionResult> UpdateUserRoleInChat(int userId, int chatId, int roleId)
        {
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            var chatAdmin = await _unitOfWork.UserChat.GetAsync(uc => uc.UserId == currentUser.Id && uc.ChatId == chatId);

            bool isChatAdmin = chatAdmin != null && chatAdmin.RoleId == 3; // Chat Admin only
            bool isAppAdmin = currentUser.AdministrationRoleId >= 2; // App Admin or SuperAdmin
            if (!isChatAdmin && !isAppAdmin) return Forbid(); // Must be Chat Admin or App Admin

            var userChat = await _unitOfWork.UserChat.GetAsync(uc => uc.UserId == userId && uc.ChatId == chatId);
            if (userChat == null) return NotFound("User is not part of the chat");

            var roleExists = await _unitOfWork.UserChatRole.GetAsync(r => r.Id == roleId);
            if (roleExists == null) return BadRequest("Invalid role ID");

            userChat.RoleId = roleId;
            await _unitOfWork.UserChat.UpdateAsync(userChat);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = $"User role updated to {roleExists.Name} in chat" });
        }
        
    }
}
