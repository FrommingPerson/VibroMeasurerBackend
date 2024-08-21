using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.RequestModels;

namespace WebApplication2.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class CommentController: ControllerBase
{
    private readonly MineDbContext _context;

    public CommentController(MineDbContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    public Comment Create(CommentModel request)
    {
        var post = _context.Posts.FirstOrDefault(x => x.Id == request.PostId);
        if (post == null)
            throw new Exception();
        var comment = new Comment()
        {
            Text = request.Text,
            CreationDate = DateTime.Now,

        };
        post.Comments.Add(comment);
        _context.SaveChanges();
        return comment;
    }


    [HttpDelete]
   public int DeleteComment(int id)
   {
       var comment = GetOneComment(id);
       _context.Comments.Remove(comment);
       _context.SaveChanges();
       return comment.CommentId;
   }
    
   [HttpGet("{id}")]
    public Comment GetOneComment(int id)
    {
        var comment = _context.Comments.FirstOrDefault(x => x.CommentId == id);
        if (comment == null) throw new Exception($"comment is not found, by id:{id}");
        return comment;
    }
}

