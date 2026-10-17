using LSMS.Application.DTO.Common;
using LSMS.Application.DTO.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Application.Interfaces.Product
{
    public interface IProductService
    {
        Task<PagedResult<ProductDto>> GetAllAsync(ProductParams productParams, CancellationToken cancellationToken = default);
            
        Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<ProductDto?> GetByBarcodeAsync(string barcode,CancellationToken cancellationToken = default);

        Task<ProductDto?> GetBySkuAsync(string sku,CancellationToken cancellationToken = default);

        Task<ProductDto> CreateAsync(CreateProductDto request,CancellationToken cancellationToken = default);
            
        Task<ProductDto?> UpdateAsync(int id,UpdateProductDto request,CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(int id,CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ProductDto>> AddImagesAsync(int productId,IReadOnlyList<string> imageUrls,CancellationToken cancellationToken = default);

    }
}
