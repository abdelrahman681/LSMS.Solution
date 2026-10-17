using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Infrastructure.Identity
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
