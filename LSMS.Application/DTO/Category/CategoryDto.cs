using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Application.DTO.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
