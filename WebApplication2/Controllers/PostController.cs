using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Hubs;
using WebApplication2.Migrations;
using WebApplication2.RequestModels;
using WebApplication2.Services;

namespace WebApplication2.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly MineDbContext _context;
    private readonly IHubContext<MainHub> _hubContext;
    // private readonly CurrentUserService _currentUserService;

    public PostController(MineDbContext context, IHubContext<MainHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
        // _currentUserService = currentUserService;
    }

    [AllowAnonymous]
    [HttpPost]
    public Post AddPost(PostModel post)
    {
        int? userId = int.Parse(HttpContext?.User?.FindFirstValue("Id"));

        var applicationUser =
            _context.ApplicationUsers.FirstOrDefault(x => x.ApplicationUserId == userId);

        var newPost = new Post()
        {
            Title = post.Title,
            Text = post.Text,
            CreationDate = DateTime.Now
        };

        _context.Posts.Add(newPost);
        _context.SaveChanges();
        _hubContext.Clients.All.SendAsync("ReceivePost", newPost);
        return newPost;
    }

    [AllowAnonymous]
    [HttpDelete]
    public int Delete(int id)
    {
        var post = GetOne(id);
        // int? userId = int.Parse(HttpContext?.User?.FindFirstValue("Id"));
        // var applicationUser = _context.ApplicationUsers
            // .Include(x => x.Posts)
            // .FirstOrDefault(x => x.ApplicationUserId == userId);
        if (!_context.Posts.Contains(post))
            throw new Exception("You don't have permissions");
        _context.Posts.Remove(post);
        _context.SaveChanges();
        _hubContext.Clients.All.SendAsync("RemovePost", post);
        return post.Id;
    }

    [AllowAnonymous]
    [HttpGet]
    public List<Post> GetAll()
    {
        return _context.Posts
            .Include(x => x.Comments)
            .ToList();
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public Post GetOne(int id)
    {
        var post = _context.Posts
            .Include(x => x.Comments)
            .FirstOrDefault(x => x.Id == id);
        if (post == null) throw new Exception($"post is not found, by id:{id}");
        return post;
    }

    [AllowAnonymous]
    [HttpGet("GetAllMy")]
    public List<Post> GetAllMy()
    {
        // int? userId = int.Parse(HttpContext?.User?.FindFirstValue("Id"));

        // var applicationUser =
            // _context.ApplicationUsers
                // .Include(x => x.Posts)
                // .FirstOrDefault(x => x.ApplicationUserId == userId);
        return _context.Posts.ToList();
    }
}