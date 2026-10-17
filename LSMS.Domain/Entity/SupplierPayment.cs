using LSMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Domain.Entity
{
    public class SupplierPayment:BaseEntity
    {
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public int? PurchaseId { get; set; }
        public Purchase? Purchase { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Notes { get; set; }
    }
}
