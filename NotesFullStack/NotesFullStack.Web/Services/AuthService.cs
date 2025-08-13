
using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using NotesFullStack.Shared.DTOs;
using NotesFullStack.Shared.Services;
using NotesFullStack.Web.Data;
using NotesFullStack.Web.Data.Entities;

namespace NotesFullStack.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IDbContextFactory<DataContext> _contextFactory;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IDbContextFactory<DataContext> contextFactory, IPasswordHasher<User> passwordHasher, IHttpContextAccessor httpContextAccessor)
        {
            _contextFactory = contextFactory;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<MethodResult<LoggedinUser>> LoginAsync(LoginModel model)
        {
            var context = _contextFactory.CreateDbContext();
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user is null)
            {
                return MethodResult<LoggedinUser>.Fail("Email not found in Database");
            }
            var isPasswordCorrect = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (isPasswordCorrect!= PasswordVerificationResult.Success)
            {
                return MethodResult<LoggedinUser>.Fail("Incorrect Password");
            }
            var loggedInUser = new LoggedinUser(user.Id, user.Name);
            return MethodResult<LoggedinUser>.Ok(loggedInUser);
        }

        public async Task<MethodResult> PlatformLoginAsync(LoggedinUser user)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            Claim[] claims = [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
                ];
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Web"));
            
            await httpContext.SignInAsync("Web", principal);

            return MethodResult.Ok();
        }

        public async Task<MethodResult> RegisterAsync(RegisterModel model)
        {
            var context = _contextFactory.CreateDbContext();
            var isEmailExist = await context.Users.AnyAsync(u=>u.Email == model.Email);
            if (isEmailExist)
            {
                return MethodResult.Fail("Email already exists");
            }

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return MethodResult.Ok();
        }


    }
}
