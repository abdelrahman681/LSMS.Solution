using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Application.DTO.Auth
{
    public class UserDTO
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
