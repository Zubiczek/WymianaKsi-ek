using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using WymianaKsiazek.Database.Entities;

namespace WymianaKsiazek.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendToUser(string user, string receiverConnectionId, string myconnectionid, string message)
        {
            await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", user, myconnectionid, message);
        }

        public string GetConnectionId() => Context.ConnectionId;
    }
}
