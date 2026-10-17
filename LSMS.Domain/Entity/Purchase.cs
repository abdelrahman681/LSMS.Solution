using LSMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Domain.Entity
{
    public class Purchase: BaseEntity
    {
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public ICollection<PurchaseItem> Items { get; set; }=new List<PurchaseItem>();
    }
}
