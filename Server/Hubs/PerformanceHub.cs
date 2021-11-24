using Microsoft.AspNetCore.SignalR;
using Optimus.Tracing;

namespace Optimus.Server.Hubs
{
    public class PerformanceHub : Hub
    {
        public async Task UpdateProcessAsync(ProcessEvent ev)
        {
            await Clients.All.SendAsync("OnProcess", ev);
        }
    }
}
