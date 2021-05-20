using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MessageAppServer.Models;
using MessageAppServer.Filters;
using MessageAppServer.Repository;

namespace MessageAppServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BasicAuthorisationFilter]
    public class MessagesController : ControllerBaseAuthMethods
    {
        private readonly IMessageRepository _messageRepo;

        public MessagesController(IMessageRepository messageRepo)
        {
            _messageRepo = messageRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            int? userId = GetUserId();
            List<Message> messages = await _messageRepo.GetMessageBasedOnUser(userId);

            return messages;
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<ActionResult<Message>> GetMessage(int id)
        {
            var message = await _messageRepo.FindMessageAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(int id, Message message)
        {
            if (id != message.MessageId)
            {
                return BadRequest();
            }

            _messageRepo.MarkAsModified(message);

            try
            {
                await _messageRepo.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message)
        {
            _messageRepo.AddMessage(message);
            await _messageRepo.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.MessageId }, message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _messageRepo.FindMessageAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _messageRepo.RemoveMessage(message);
            await _messageRepo.SaveChangesAsync();

            return NoContent();
        }

        private bool MessageExists(int id)
        {
            return _messageRepo.MessageExists(id);
        }
    }
}
