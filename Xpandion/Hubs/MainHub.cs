using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

namespace Xpandion.WebSite.Hubs
{
    public class MainHub : Hub
    {
        public async Task AssociateJob(string guid)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, guid);
        }
    }
}
