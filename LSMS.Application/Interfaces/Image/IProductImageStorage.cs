using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
namespace LSMS.Application.Interfaces.Image
{
    public interface IProductImageStorage
    {
        Task<IReadOnlyList<string>> SaveAsync(IReadOnlyList<IFormFile> files,CancellationToken cancellationToken = default);
      
    }
}
