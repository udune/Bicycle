using Microsoft.AspNetCore.SignalR;

namespace Bicycle.Services;

public class MessageHub : Hub
{
    public async Task Send(string message, string userId)
    {
        await Clients.All.SendAsync("receive", message, userId);
    }
}