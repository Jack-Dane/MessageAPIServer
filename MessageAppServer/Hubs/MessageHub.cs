using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageAppServer.Models;
using MessageAppServer.DAL;

namespace MessageAppServer.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendMessageToUser(int senderId, int recieverId, string message)
        {
            // TODO - check the user is logged in
            using var db = new MessageContext();

            // find the clients connectionId in the LiveUsers table
            LiveUser reciever = db.LiveUsers.Where(liveUser => liveUser.UserId == recieverId).FirstOrDefault();
            string recieverConnectionId = reciever.connectionString;

            // find the sender based on the senderId
            User sender = db.Users.Where(user => user.UserId == senderId).FirstOrDefault();
            string senderName = sender.Name;

            // TODO - create the new message in the database so can be picked up by API

            // send the notification to the client
            await Clients.Client(recieverConnectionId).SendAsync("RecieveMessage", message, senderName);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.All.SendAsync("ReceiveMessage", "Bot", "User has joined group " + groupName);
        }
    }
}
