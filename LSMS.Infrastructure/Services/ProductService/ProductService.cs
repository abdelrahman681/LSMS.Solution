using LSMS.Application.DTO.Common;
using LSMS.Application.DTO.Product;
using LSMS.Application.Interfaces.Product;
using LSMS.Domain.Entity;
using LSMS.Domain.Enum;
using LSMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Infrastructure.Services.ProductService
{
    public class ProductService: IProductService
    {
        private readonly LSMSDbContext _context;
        private readonly IConfiguration _configuration;

        public ProductService(LSMSDbContext context,IConfiguration configuration)
        {
            _context = context;
            this._configuration = configuration;
        }


        public async Task<PagedResult<ProductDto>> GetAllAsync(ProductParams productParams,CancellationToken cancellationToken = default)  
        {
            // Validate pagination parameters


            // Build query without loading data yet
            var query = _context.Products.AsNoTracking();
            // Count all matching products
            var totalCount = await query.CountAsync(cancellationToken);

            // Fetch only the requested page
            var products = await query
                .OrderBy(p => p.Id)
                .Skip((productParams.PageIndex - 1) * productParams.PageSize)
                .Take(productParams.PageSize)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Barcode = p.Barcode,
                    SKU = p.SKU,
                    UnitCost = p.UnitCost,
                    SellingPrice = p.SellingPrice,
                    StockQuantity = p.StockQuantity,
                    ReorderLevel = p.ReorderLevel,
                    IsActive = p.IsActive,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    Images = p.Images.OrderBy(i => i.DisplayOrder)
                    .Select(i => new ProductImageDto
                    {
                        Id = i.Id,
                        ImageUrl = $"{_configuration["BaseUrl"]}{i.ImageUrl}",
                        DisplayOrder = i.DisplayOrder,
                        IsPrimary = i.IsPrimary
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<ProductDto>
            {
                Data = products,
                PageIndex = productParams.PageIndex,
                PageSize = productParams.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<ProductDto?> GetByIdAsync(int id,CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Barcode = p.Barcode,
                    SKU = p.SKU,
                    UnitCost = p.UnitCost,
                    SellingPrice = p.SellingPrice,
                    StockQuantity = p.StockQuantity,
                    ReorderLevel = p.ReorderLevel,
                    IsActive = p.IsActive,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    Images = p.Images.OrderBy(i => i.DisplayOrder)
                    .Select(i => new ProductImageDto
                    {
                        Id = i.Id,
                        ImageUrl = $"{_configuration["BaseUrl"]}{i.ImageUrl}",
                        DisplayOrder = i.DisplayOrder,
                        IsPrimary = i.IsPrimary
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ProductDto?> GetByBarcodeAsync(string barcode,CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(barcode))
                return null;

            return await _context.Products
                .AsNoTracking()
                .Where(p => p.IsActive && p.Barcode == barcode.Trim())
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Barcode = p.Barcode,
                    SKU = p.SKU,
                    UnitCost = p.UnitCost,
                    SellingPrice = p.SellingPrice,
                    StockQuantity = p.StockQuantity,
                    ReorderLevel = p.ReorderLevel,
                    IsActive = p.IsActive,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    Images = p.Images.OrderBy(i => i.DisplayOrder)
                    .Select(i => new ProductImageDto
                    {
                        Id = i.Id,
                        ImageUrl = $"{_configuration["BaseUrl"]}{i.ImageUrl}",
                        DisplayOrder = i.DisplayOrder,
                        IsPrimary = i.IsPrimary
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ProductDto?> GetBySkuAsync(string sku,CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sku))
                return null;

            return await _context.Products
                .AsNoTracking()
                .Where(p => p.IsActive && p.SKU == sku.Trim())
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Barcode = p.Barcode,
                    SKU = p.SKU,
                    UnitCost = p.UnitCost,
                    SellingPrice = p.SellingPrice,
                    StockQuantity = p.StockQuantity,
                    ReorderLevel = p.ReorderLevel,
                    IsActive = p.IsActive,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    Images = p.Images.OrderBy(i => i.DisplayOrder)
                    .Select(i => new ProductImageDto
                    {
                        Id = i.Id,
                        ImageUrl = $"{_configuration["BaseUrl"]}{i.ImageUrl}",
                        DisplayOrder = i.DisplayOrder,
                        IsPrimary = i.IsPrimary
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto request,CancellationToken cancellationToken = default)

        {
            var sku = request.SKU.Trim();
            var barcode = NormalizeBarcode(request.Barcode);

            if (await _context.Products.AnyAsync(
                    p => p.SKU == sku, cancellationToken))
            {
                throw new InvalidOperationException(
                    "A product with this SKU already exists.");
            }

            if (barcode != null &&
                await _context.Products.AnyAsync(
                    p => p.Barcode == barcode, cancellationToken))
            {
                throw new InvalidOperationException(
                    "A product with this barcode already exists.");
            }

            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (!categoryExists)
            {
                throw new InvalidOperationException(
                    "The selected category does not exist.");
            }

            if (request.UnitCost < 0 || request.SellingPrice < 0)
            {
                throw new InvalidOperationException(
                    "Product prices cannot be negative.");
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync(cancellationToken);

            var product = new Product
            {
                Name = request.Name.Trim(),
                Description = request.Description?.Trim(),
                Barcode = barcode,
                SKU = sku,
                UnitCost = request.UnitCost,
                SellingPrice = request.SellingPrice,
                StockQuantity = request.StockQuantity,
                ReorderLevel = request.ReorderLevel,
                CategoryId = request.CategoryId,
                IsActive = true,

            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync(cancellationToken);

            if (product.StockQuantity > 0)
            {
                var movement = new StockMovement
                {
                    ProductId = product.Id,
                    MovementType = StockMovementType.AdjustmentIncrease,
                    Quantity = product.StockQuantity,
                    StockBefore = 0,
                    StockAfter = product.StockQuantity,
                    MovementDate = DateTime.UtcNow,
                    ReferenceType = "ProductInitialStock",
                    ReferenceId = product.Id,
                    Notes = "Initial stock on product creation"
                };

                _context.StockMovements.Add(movement);

                await _context.SaveChangesAsync(cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);

            return (await GetByIdAsync(product.Id, cancellationToken))!;
        }

        public async Task<ProductDto?> UpdateAsync(int id,UpdateProductDto request,CancellationToken cancellationToken = default)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (product == null)
                return null;

            var sku = request.SKU.Trim();
            var barcode = NormalizeBarcode(request.Barcode);

            if (await _context.Products.AnyAsync(
                    p => p.Id != id && p.SKU == sku, cancellationToken))
            {
                throw new InvalidOperationException(
                    "A product with this SKU already exists.");
            }

            if (barcode != null &&
                await _context.Products.AnyAsync(
                    p => p.Id != id && p.Barcode == barcode,
                    cancellationToken))
            {
                throw new InvalidOperationException(
                    "A product with this barcode already exists.");
            }

            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (!categoryExists)
            {
                throw new InvalidOperationException(
                    "The selected category does not exist.");
            }

            if (request.UnitCost < 0 || request.SellingPrice < 0)
            {
                throw new InvalidOperationException(
                    "Product prices cannot be negative.");
            }

            product.Name = request.Name.Trim();
            product.Description = request.Description?.Trim();
            product.Barcode = barcode;
            product.SKU = sku;
            product.UnitCost = request.UnitCost;
            product.SellingPrice = request.SellingPrice;
            product.ReorderLevel = request.ReorderLevel;
            product.CategoryId = request.CategoryId;
            product.IsActive = request.IsActive;


            // StockQuantity is intentionally not changed here.
            // Stock changes must be recorded as stock movements.

            await _context.SaveChangesAsync(cancellationToken);

            return await GetByIdAsync(id, cancellationToken);
        }

        public async Task<bool> DeleteAsync(int id,CancellationToken cancellationToken = default)  
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (product == null)
                return false;

            // Soft delete: preserve historical sales and purchase records.
            product.IsActive = false;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }


        public async Task<IReadOnlyList<ProductDto>> AddImagesAsync(int productId, IReadOnlyList<string> imageUrls,CancellationToken cancellationToken = default)  
        {
            if (imageUrls.Count == 0)
                throw new InvalidOperationException(
                    "At least one image is required.");

            if (imageUrls.Any(string.IsNullOrWhiteSpace))
                throw new InvalidOperationException(
                    "Image URLs cannot be empty.");

            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(
                    p => p.Id == productId,
                    cancellationToken);

            if (product is null)
                throw new KeyNotFoundException("Product not found.");

            foreach (var imageUrl in imageUrls)
            {
                product.AddImage(new ProductImage
                {
                    ImageUrl = imageUrl.Trim(),
                    DisplayOrder = product.Images.Count,
                    IsPrimary = product.Images.Count == 0
                });
            }

            await _context.SaveChangesAsync(cancellationToken);

            return await _context.Products
                .Where(p => p.Id == productId)
                .SelectMany(p => p.Images)
                .OrderBy(i => i.DisplayOrder)
                .Select(i => new ProductDto
                {
                    Id = i.ProductId,
                    Name = i.Product.Name,
                    Description = i.Product.Description,
                    Barcode = i.Product.Barcode,
                    SKU = i.Product.SKU,
                    UnitCost = i.Product.UnitCost,
                    SellingPrice = i.Product.SellingPrice,
                    StockQuantity = i.Product.StockQuantity,
                    ReorderLevel = i.Product.ReorderLevel,
                    IsActive = i.Product.IsActive,
                    CategoryId = i.Product.CategoryId,
                    CategoryName = i.Product.Category.Name
                })
                .ToListAsync(cancellationToken);
        }


        private static string? NormalizeBarcode(string? barcode)
        {
            return string.IsNullOrWhiteSpace(barcode)
                ? null
                : barcode.Trim();
        }
    }
}
