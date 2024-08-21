using Microsoft.AspNetCore.Mvc;
using WebApplication2.RequestModels;
using WebApplication2.Services;

namespace WebApplication2.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController: ControllerBase
{
    private readonly MineDbContext _context;
    private readonly IdentityService _identityService;

    public AuthController(MineDbContext context, IdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    [HttpGet]
    public string GetToken(string email, string password)
    {
        var applicationUser = _context.ApplicationUsers.FirstOrDefault(x => x.Email == email);
        if (applicationUser == null)
            throw new Exception("The user is not found");
        
        var isValid = BCrypt.Net.BCrypt.Verify(password, applicationUser.Password);
        if (!isValid)
            throw new Exception("The user is not found");
        return _identityService.GenerateJwtToken(applicationUser, DateTime.Now.AddDays(2));
    }
}