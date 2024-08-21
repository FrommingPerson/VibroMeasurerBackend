namespace WebApplication2;

public class Post
{
    public string Title { get; set; }
    public string Text { get; set; }
    public int Id { get; set; }

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public DateTime CreationDate { get; set; }
}