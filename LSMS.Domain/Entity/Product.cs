using LSMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Domain.Entity
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Barcode { get; set; }
        public string SKU { get; set; }
        public decimal UnitCost { get; set; }
        public decimal SellingPrice { get; set; }
        public int StockQuantity { get; set; }
        public int ReorderLevel { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ProductImage> Images { get; private set; } = new List<ProductImage>();

        public void AddImage(ProductImage image)
        {
            ArgumentNullException.ThrowIfNull(image);

            Images.Add(image);
        }

        public void RemoveImage(int imageId)
        {
            var image = Images.FirstOrDefault(i => i.Id == imageId);

            if (image is not null)
            {
                Images.Remove(image);
            }
        }
    }
}
