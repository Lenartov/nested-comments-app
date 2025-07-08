using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace NestedComments.Api
{
    public class CommentHub : Hub
    {
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
