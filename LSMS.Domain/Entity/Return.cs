using LSMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Domain.Entity
{
    public class Return: BaseEntity
    {
        public int SaleId { get; set; }
        public Sale Sale { get; set; }
        public DateTime ReturnDate { get; set; }
        public string CashierId { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
    }
}
