using AuctionProject.Data;
using AuctionProject.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private readonly ApplicationDbContext _context;

    public ChatHub(ApplicationDbContext context)
    {
        _context = context;
    }

    /*public async Task SendMessage(string user, string message, int idProduct)
    {
        var chatMessage = new ChatMessage { Username = user, Message = message, Idproduct = idProduct};

        // Save the message to the database
        _context.ChatMessages.Add(chatMessage);
        await _context.SaveChangesAsync();

        // Broadcast the message to all clients
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }*/
    public async Task SendMessage(string user, string message, int idProduct, string channel)
    {
        var chatMessage = new ChatMessage { Username = user, Message = message, Idproduct = idProduct, Channel = channel };

        // Save the message to the database
        _context.ChatMessages.Add(chatMessage);
        await _context.SaveChangesAsync();

        // Broadcast the message to all clients
        await Clients.All.SendAsync("ReceiveMessage", user, message, channel);
    }
    public async Task GetChatMessages()
    {
        var messages = await _context.ChatMessages.ToListAsync();
        await Clients.Caller.SendAsync("LoadChatMessages", messages);
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();

        // Send existing chat messages to the connected client
        await GetChatMessages();
    }
}