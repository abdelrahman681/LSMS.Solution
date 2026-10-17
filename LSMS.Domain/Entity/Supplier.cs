using LSMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Domain.Entity
{
    public class Supplier: BaseEntity
    {
        public string Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public decimal OpeningBalance { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Purchase> Purchases { get; set; }=new List<Purchase>();
        public ICollection<SupplierPayment> Payments { get; set; } = new List<SupplierPayment>();
    }
}
