using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Application.DTO.Product
{
    public class ProductImageDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsPrimary { get; set; }
    }
}
