using LSMS.API.Errors;
using LSMS.Application.DTO.Common;
using LSMS.Application.DTO.Product;
using LSMS.Application.Interfaces.Image;
using LSMS.Application.Interfaces.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace LSMS.API.Controllers
{
    [Authorize]
    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IProductImageStorage _imageStorage;

        public ProductsController(IProductService productService,
            IProductImageStorage imageStorage)
        {
            _productService = productService;
            this._imageStorage = imageStorage;
        }

        // GET: api/Products
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<PagedResult<ProductDto>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorApiResponse))]
        public async Task<ActionResult<IReadOnlyList<PagedResult<ProductDto>>>> GetAll(CancellationToken cancellationToken,[FromQuery] ProductParams @params)
        {
            var products = await _productService.GetAllAsync(@params, cancellationToken);
            return Ok(products);
        }

        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorApiResponse))]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetById(int id,CancellationToken cancellationToken)  
        {
            if (id <= 0)
                return BadRequest(new ErrorApiResponse(400, "Invalid product ID."));

            var product = await _productService.GetByIdAsync(id, cancellationToken);
                
            if (product is null)
                return NotFound(new ErrorApiResponse(404, "Product not found."));

            return Ok(product);
        }

        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorApiResponse))]
        [HttpGet("barcode/{barcode}")]
        public async Task<ActionResult<ProductDto>> GetByBarcode(string barcode,CancellationToken cancellationToken)
        {
            var product = await _productService.GetByBarcodeAsync(
                barcode, cancellationToken);

            if (product is null)
                return NotFound(new ErrorApiResponse(404, "Active product not found."));

            return Ok(product);
        }

        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorApiResponse))]
        [HttpGet("sku/{sku}")]
        public async Task<ActionResult<ProductDto>> GetBySku(string sku,CancellationToken cancellationToken) 
        {
            var product = await _productService.GetBySkuAsync(sku, cancellationToken);

            if (product is null)
                return NotFound(new ErrorApiResponse(404, "Active product not found."));

            return Ok(product);
        }


        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductDto>> Create(
            [FromForm] CreateProductDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                var product = await _productService.CreateAsync(request,cancellationToken);
                if (request.Images is { Count: > 0 })
                {
                    var imageUrls = await _imageStorage.SaveAsync(request.Images,cancellationToken);
                    var productWithImages = await _productService.AddImagesAsync(
                        product.Id,
                        imageUrls,
                        cancellationToken);

                    return CreatedAtAction(
                        nameof(GetById),
                        new { id = product.Id },
                        productWithImages);
                }

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = product.Id },
                    product);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ErrorApiResponse(400, ex.Message));
            }
        }


        // PUT: api/Products/5

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductDto>> Update(int id,[FromForm] UpdateProductDto request,CancellationToken cancellationToken)  
        {
            try
            {
                var product = await _productService.UpdateAsync(
                    id,
                    request,
                    cancellationToken);

                if (product is null)
                    return NotFound(new ErrorApiResponse(404, "Product not found."));

                // لو المستخدم رفع صور جديدة، نضيفها للصور الموجودة
                if (request.Images is { Count: > 0 })
                {
                    var imageUrls = await _imageStorage.SaveAsync(
                        request.Images,
                        cancellationToken);

                    var productWithImages = await _productService.AddImagesAsync(
                        id,
                        imageUrls,
                        cancellationToken);

                    return Ok(productWithImages);
                }

                // لو مفيش صور جديدة، نرجع بيانات المنتج بصوره الحالية
                return Ok(product);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ErrorApiResponse(400, ex.Message));
            }
        }


        // DELETE: api/Products/5
        // Soft delete: deactivates the product instead of physically deleting it.
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            if (id <= 0)
                return BadRequest(new ErrorApiResponse(400, "Invalid product ID."));

            var deleted = await _productService.DeleteAsync(id, cancellationToken);
            if (!deleted)
                return NotFound(new ErrorApiResponse(404, "Product Not Found"));

            return NoContent();
        }
    }

}

