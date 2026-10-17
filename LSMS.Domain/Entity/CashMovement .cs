using LSMS.Domain.Common;
using LSMS.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Domain.Entity
{
    public class CashMovement:BaseEntity
    {
        public CashMovementType MovementType { get; set; }

        public decimal Amount { get; set; }

        public DateTime MovementDate { get; set; } = DateTime.UtcNow;

        public string? ReferenceType { get; set; }

        public int? ReferenceId { get; set; }

        public string? Notes { get; set; }

        public string UserId { get; set; }
    }
}
