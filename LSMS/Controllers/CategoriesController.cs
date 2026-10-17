using LSMS.API.Errors;
using LSMS.Application.DTO.Category;
using LSMS.Application.DTO.Common;
using LSMS.Application.DTO.Product;
using LSMS.Application.Interfaces.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LSMS.API.Controllers
{
    [Authorize]
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [ProducesResponseType(typeof(IReadOnlyList<CategoryDto>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorApiResponse))]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetAll(CancellationToken cancellationToken)  
        {
            var categories = await _categoryService.GetAllAsync(cancellationToken);
            return Ok(categories);
        }
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorApiResponse))]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id,CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetByIdAsync(id, cancellationToken);
            return category is null ? NotFound(new ErrorApiResponse(404)) : Ok(category);
        }
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorApiResponse))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDto>> Create(CreateCategoryDto request,CancellationToken cancellationToken) 
        {
            try
            {
                var category = await _categoryService.CreateAsync(request, cancellationToken);
                return CreatedAtAction(nameof(GetById),new { id = category.Id },category);  
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorApiResponse))]
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDto>> Update(int id,UpdateCategoryDto request,CancellationToken cancellationToken)
        {
            try
            {
                var category = await _categoryService.UpdateAsync(id, request, cancellationToken);
                return category is null ? NotFound(new ErrorApiResponse(404)) : Ok(category);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            try
            {
                var deleted = await _categoryService.DeleteAsync(id, cancellationToken);
                return deleted ? NoContent() : NotFound(new ErrorApiResponse(404));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
