using LSMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Domain.Entity
{
    public class PurchaseItem: BaseEntity
    {
        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal LineTotal { get; set; }
    }
}
