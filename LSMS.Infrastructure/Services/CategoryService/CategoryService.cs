using LSMS.Application.DTO.Category;
using LSMS.Application.Interfaces.Category;
using LSMS.Domain.Entity;
using LSMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Infrastructure.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly LSMSDbContext _context;

        public CategoryService(LSMSDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id,CancellationToken cancellationToken = default)  
        {
              return await _context.Categories.AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                }).FirstOrDefaultAsync(cancellationToken);   
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto request,CancellationToken cancellationToken = default) 
        {
            var name = request.Name.Trim();

            var exists = await _context.Categories
                .AnyAsync(c => c.Name == name, cancellationToken);

            if (exists)
                throw new InvalidOperationException(
                    "A category with this name already exists.");

            var category = new Category
            {
                Name = name,
                Description = request.Description?.Trim()
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync(cancellationToken);

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<CategoryDto?> UpdateAsync(
            int id,
            UpdateCategoryDto request,
            CancellationToken cancellationToken = default)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (category is null)
                return null;

            var name = request.Name.Trim();

            var duplicate = await _context.Categories
                .AnyAsync(c => c.Id != id && c.Name == name,
                    cancellationToken);

            if (duplicate)
                throw new InvalidOperationException(
                    "A category with this name already exists.");

            category.Name = name;
            category.Description = request.Description?.Trim();

            await _context.SaveChangesAsync(cancellationToken);

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<bool> DeleteAsync(int id,CancellationToken cancellationToken = default)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (category is null)
                return false;

            if (category.Products.Any())
                throw new InvalidOperationException(
                    "Cannot delete a category that has products.");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
