using Microsoft.AspNetCore.Mvc;
using PrivateMessanger.DataAccess.Repository.IRepository;
using PrivateMessanger.Models;

namespace PrivateMessengerAPI.Controllers
{
    [ApiController]
    [Route("api/chats")]
    public class ChatController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChatController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllChats()
        {
            var chats = await _unitOfWork.Chat.GetAllAsync();
            return Ok(chats);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChat(int id)
        {
            var chat = await _unitOfWork.Chat.GetAsync(c => c.Id == id);
            if (chat == null) return NotFound();

            return Ok(chat);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat([FromBody] Chat chat)
        {
            await _unitOfWork.Chat.AddAsync(chat);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetChat), new { id = chat.Id }, chat);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateChat(int id, [FromBody] Chat chat)
        {
            var chatFromDb = await _unitOfWork.Chat.GetAsync(c => c.Id == id);
            if (chatFromDb == null) return NotFound();

            chatFromDb.Name = chat.Name;

            await _unitOfWork.Chat.UpdateAsync(chat);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

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
