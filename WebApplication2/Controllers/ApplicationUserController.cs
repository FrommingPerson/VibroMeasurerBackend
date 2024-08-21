using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Dto;
using WebApplication2.RequestModels;

namespace WebApplication2.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ApplicationUserController : ControllerBase
{
    private readonly MineDbContext _context;

    public ApplicationUserController(MineDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    [HttpPost]
    public ApplicationUser Create(ApplicationUserModel request)
    {
        var applicationUser = _context.ApplicationUsers.FirstOrDefault(x => x.Email == request.Email);
        if (applicationUser != null)
            throw new Exception("This password is totally wrong");
        var newApplicationUser = new ApplicationUser()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password, 12)
        };
        _context.ApplicationUsers.Add(newApplicationUser);
        _context.SaveChanges();
        return newApplicationUser;
    }

    [HttpGet]
    public ApplicationUserDto GetCurrentUser()
    {
        int? userId = int.Parse(HttpContext?.User?.FindFirstValue("Id"));
        var applicationUser = _context.ApplicationUsers
            .FirstOrDefault(x => x.ApplicationUserId == userId);
        if (applicationUser == null)
        {
            throw new Exception("Something is going wrong");
        }
        return new ApplicationUserDto()
        {
            ApplicationUserId = applicationUser.ApplicationUserId,
            Email = applicationUser.Email,
            FirstName = applicationUser.FirstName,
            LastName = applicationUser.LastName,
        };
    }
}