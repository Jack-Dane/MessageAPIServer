using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MessageAppServer.DAL;
using MessageAppServer.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MessageAppServer.Filters;

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

        // GET: api/Messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            int? userId = GetUserId();
            List<Message> messages = await _messageRepo.GetMessageBasedOnUser(userId);

            return messages;
        }

        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(int id)
        {
            var message = await _messageRepo.FindMessageAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }

        // PUT: api/Messages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // POST: api/Messages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message)
        {
            _messageRepo.AddMessage(message);
            await _messageRepo.SaveChangesAsync();

            // Hub.Clients.All.newMessage(message.Body);
            return CreatedAtAction("GetMessage", new { id = message.MessageId }, message);
        }

        // DELETE: api/Messages/5
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
