using System.Security.Claims;

namespace WebApplication2.Services;

    public class CurrentUserService 
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId => int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue("Id"));
    }
