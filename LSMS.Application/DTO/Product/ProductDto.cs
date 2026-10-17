using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Application.DTO.Product
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Barcode { get; set; }

        public string SKU { get; set; } = string.Empty;

        public decimal UnitCost { get; set; }

        public decimal SellingPrice { get; set; }

        public int StockQuantity { get; set; }

        public int ReorderLevel { get; set; }

        public bool IsActive { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;
        public IReadOnlyList<ProductImageDto> Images { get; set; }= [];
    
    }

}


