using LSMS.Domain.Common;
using LSMS.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Domain.Entity
{
    public class StockMovement: BaseEntity
    {
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public StockMovementType MovementType { get; set; }

        public int Quantity { get; set; }

        public int StockBefore { get; set; }

        public int StockAfter { get; set; }

        public DateTime MovementDate { get; set; } = DateTime.UtcNow;

        public string? ReferenceType { get; set; }

        public int? ReferenceId { get; set; }

        public string? Notes { get; set; }
    }
}
