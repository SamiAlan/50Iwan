using Iwan.Server.Constants;
using Iwan.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Iwan.Server.Hubs
{
    [Authorize(Roles = $"{Roles.Admin}, {Roles.SuperAdmin}")]
    public class AdminHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, SignalRGroups.WatermarkProgress);
            await Groups.AddToGroupAsync(Context.ConnectionId, SignalRGroups.UnWatermarkProgress);
            await Groups.AddToGroupAsync(Context.ConnectionId, SignalRGroups.ProductsImagesResizeProgress);
            await Groups.AddToGroupAsync(Context.ConnectionId, SignalRGroups.CategoriesImagesResizeProgress);
            await Groups.AddToGroupAsync(Context.ConnectionId, SignalRGroups.CompositionsImagesResizeProgress);
            await Groups.AddToGroupAsync(Context.ConnectionId, SignalRGroups.SliderImagesResizeProgress);
            await Groups.AddToGroupAsync(Context.ConnectionId, SignalRGroups.AboutUsImagesResizeProgress);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, SignalRGroups.WatermarkProgress);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, SignalRGroups.UnWatermarkProgress);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, SignalRGroups.ProductsImagesResizeProgress);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, SignalRGroups.CategoriesImagesResizeProgress);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, SignalRGroups.CompositionsImagesResizeProgress);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, SignalRGroups.SliderImagesResizeProgress);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
