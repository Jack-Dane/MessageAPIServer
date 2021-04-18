using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageAppServer.Models;
using MessageAppServer.DAL;
using MessageAppServer.Filters;
using Microsoft.AspNetCore.Authorization;

namespace MessageAppServer.Hubs
{
    [Authorize("MyAuthorizationPolicy")]
    public class MessageHub : Hub
    {
        private readonly MessageContext _context;
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

        public MessageHub(MessageContext context)
        {
            _context = context;
        }

        public override Task OnConnectedAsync()
        {
            // TODO string name = Context.User.Identity.Name;
            string name = "test";

            _connections.Add(name, Context.ConnectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            // TODO string name = Context.User.Identity.Name;
            string name = "test";

            _connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessageToUser(string senderUsername, string recieverUsername, string message)
        {
            IEnumerable<string> connectionIds = FindConnectionIdBasedOnUsername(recieverUsername);
            foreach (string connectionId in connectionIds)
            {
                await Clients.Client(connectionId).SendAsync("RecieveMessage", message, senderUsername);
            }

            AddMessageToDatabase(senderUsername, recieverUsername, message);
        }

        private IEnumerable<string> FindConnectionIdBasedOnUsername(string username)
        {
            IEnumerable<string> connectionIds = _connections.GetConnections(username);
            return connectionIds;
        }

        private async void AddMessageToDatabase(string senderUsername, string recieverUsername, string message)
        {
            User sender = FindAndReturnUserByUsername(senderUsername);
            User reciever = FindAndReturnUserByUsername(recieverUsername);
            if (sender is not null && reciever is not null)
            {
                await _context.Messages.AddAsync(new Message()
                {
                    Sender = sender,
                    Reciever = reciever,
                    Body = message
                });
                await _context.SaveChangesAsync();
            }
        }

        private User FindAndReturnUserByUsername(string username)
        {
            return _context.Users.Where(user => user.Username == username).FirstOrDefault();
        }
    }
}
