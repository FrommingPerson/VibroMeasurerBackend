namespace WebApplication2;

public class ApplicationUser
{
    public int ApplicationUserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    // public ICollection<Post> Posts { get; set; } = new List<Post>();
}