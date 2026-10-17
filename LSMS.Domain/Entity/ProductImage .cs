using LSMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Domain.Entity
{
    public class ProductImage:BaseEntity
    {
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public string ImageUrl { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }

        public bool IsPrimary { get; set; }
    }
}
