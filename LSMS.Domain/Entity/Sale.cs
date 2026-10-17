using LSMS.Domain.Common;
using LSMS.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Domain.Entity
{
    public class Sale: BaseEntity
    {
        public string InvoiceNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public string CashierId { get; set; }
        public decimal Subtotal { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
    }
}
