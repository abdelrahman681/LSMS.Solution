using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LSMS.Application.DTO.Category
{
    public class CreateCategoryDto
    {
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
