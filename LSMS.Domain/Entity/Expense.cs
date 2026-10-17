using LSMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Domain.Entity
{
    public class Expense: BaseEntity
    {
        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime ExpenseDate { get; set; } = DateTime.UtcNow;

        public string? Notes { get; set; }

        public string UserId { get; set; }
    }
}
