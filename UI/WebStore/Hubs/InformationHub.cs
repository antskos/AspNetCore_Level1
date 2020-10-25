using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Hubs
{
    public class InformationHub : Hub
    {
        public async Task SendMessage(string message) => await Clients.All.SendAsync("MessageFromClient", message);
    }
}
