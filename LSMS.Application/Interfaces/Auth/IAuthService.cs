using LSMS.Application.DTO.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Application.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<UserDTO?> LoginAsync(LoginDTO request,CancellationToken cancellationToken = default);
      
      
    }
}
