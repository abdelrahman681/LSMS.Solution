using LSMS.Application.DTO.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Application.Interfaces.Category
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CategoryDto?> GetByIdAsync(int id,CancellationToken cancellationToken = default);
        Task<CategoryDto> CreateAsync(CreateCategoryDto request,CancellationToken cancellationToken = default);

        Task<CategoryDto?> UpdateAsync(int id,UpdateCategoryDto request,CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id,CancellationToken cancellationToken = default);
            
            
    }
}
