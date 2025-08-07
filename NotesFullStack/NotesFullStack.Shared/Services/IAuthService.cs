using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NotesFullStack.Shared.DTOs;

namespace NotesFullStack.Shared.Services
{
    public interface IAuthService
    {
        Task<MethodResult> RegisterAsync(RegisterModel model);
        Task<MethodResult> LoginAsync(LoginModel model);
    }
}
