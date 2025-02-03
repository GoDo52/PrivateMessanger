using Microsoft.AspNetCore.Mvc;
using PrivateMessanger.DataAccess.Repository.IRepository;
using PrivateMessanger.Models;

namespace PrivateMessengerAPI.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MessageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMessages()
        {
            var messages = await _unitOfWork.Message.GetAllAsync();
            return Ok(messages);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage(int id)
        {
            var message = await _unitOfWork.Message.GetAsync(m  => m.Id == id);
            if (message == null) return NotFound();

            return Ok(message);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] Message message)
        {
            await _unitOfWork.Message.AddAsync(message);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetMessage), new { id = message.Id }, message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(int id, [FromBody] Message message)
        {
            var messageFromDb = await _unitOfWork.Message.GetAsync(m => m.Id == id);
            if (messageFromDb == null) return NotFound();

            messageFromDb.TextContent = message.TextContent;

            await _unitOfWork.Message.UpdateAsync(messageFromDb);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _unitOfWork.Message.GetAsync(m => m.Id == id);
            if (message == null) return NotFound();

            await _unitOfWork.Message.RemoveAsync(message);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
