using LSMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Domain.Entity
{
    public class AuditLog:BaseEntity
    {
        public string UserId { get; set; }

        public string Action { get; set; } = string.Empty;

        public string EntityName { get; set; } = string.Empty;

        public int? EntityId { get; set; }

        public string? OldValues { get; set; }

        public string? NewValues { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? IpAddress { get; set; }
    }
}
