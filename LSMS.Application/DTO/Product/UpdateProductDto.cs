using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LSMS.Application.DTO.Product
{
    public class UpdateProductDto
    {
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? Barcode { get; set; }

        [Required]
        [StringLength(100)]
        public string SKU { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "9999999999999999")]
        public decimal UnitCost { get; set; }

        [Range(typeof(decimal), "0.01", "9999999999999999")]
        public decimal SellingPrice { get; set; }

        [Range(0, int.MaxValue)]
        public int ReorderLevel { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public bool IsActive { get; set; }
        public List<IFormFile?> Images { get; set; }

    }
}

