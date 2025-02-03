using Microsoft.AspNetCore.Mvc;
using PrivateMessanger.DataAccess.Repository.IRepository;
using PrivateMessanger.Models;
using System;

namespace PrivateMessengerAPI.Controllers
{
    [ApiController]
    [Route("api/userchats")]

    public class UserChatController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserChatController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddUserToChat(int userId, int chatId)
        {
            var user = await _unitOfWork.User.GetAsync(u => u.Id == userId);
            var chat = await _unitOfWork.Chat.GetAsync(c => c.Id == chatId);

            if (user == null || chat == null) return NotFound("User or Chat not found");

            var userChat = new UserChat { UserId = userId, ChatId = chatId };
            await _unitOfWork.UserChat.AddAsync(userChat);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = $"User {user.Tag} added to chat {chat.Name} successfully" });
        }

        // Remove a user from a chat
        [HttpDelete]
        [Route("remove")]
        public async Task<IActionResult> RemoveUserFromChat(int userId, int chatId)
        {
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

        // List users in a chat
        [HttpGet("chat/{chatId}/users")]
        public async Task<IActionResult> GetUsersInChat(int chatId)
        {
            var userChats = await _unitOfWork.UserChat.GetAllAsync(includeProperties: "User");
            var usersInChat = userChats.Where(uc => uc.ChatId == chatId).Select(uc => uc.User);
            return Ok(usersInChat);
        }

        // List chats for a user
        [HttpGet("user/{userId}/chats")]
        public async Task<IActionResult> GetChatsForUser(int userId)
        {
            var userChats = await _unitOfWork.UserChat.GetAllAsync(includeProperties: "Chat");
            var chatsForUser = userChats.Where(uc => uc.UserId == userId).Select(uc => uc.Chat);
            return Ok(chatsForUser);
        }
    }
}
