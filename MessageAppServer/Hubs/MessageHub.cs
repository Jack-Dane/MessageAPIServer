using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageAppServer.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(string user, string message, string groupName)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.All.SendAsync("ReceiveMessage", "Bot", "User has joined group " + groupName);
        }
    }
}
