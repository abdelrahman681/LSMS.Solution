using LSMS.Application.Interfaces.Image;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
namespace LSMS.Infrastructure.Services.Images
{
    public class ProductImageStorage:IProductImageStorage
    {
        private readonly IWebHostEnvironment _environment;

        public ProductImageStorage(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<IReadOnlyList<string>> SaveAsync(IReadOnlyList<IFormFile> files,CancellationToken cancellationToken = default)
        {
            if (files.Count == 0)
                return [];

            const long maxFileSize = 5 * 1024 * 1024;

            var allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName);

                if (file.Length <= 0 || file.Length > maxFileSize)
                    throw new InvalidOperationException(
                        "Each image must be between 1 byte and 5 MB.");

                if (!allowedExtensions.Contains(extension))
                    throw new InvalidOperationException(
                        "Only JPG, PNG and WEBP images are allowed.");
            }

            var webRoot = _environment.WebRootPath;

            if (string.IsNullOrWhiteSpace(webRoot))
            {
                webRoot = Path.Combine(_environment.ContentRootPath,"wwwroot");
                Directory.CreateDirectory(webRoot);
            }

            var folder = Path.Combine(webRoot,"Images");
                
            Directory.CreateDirectory(folder);

            var savedUrls = new List<string>();

            try
            {
                foreach (var file in files)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var extension = Path.GetExtension(file.FileName);
                    var fileName = $"{Guid.NewGuid():N}{extension}";
                    var filePath = Path.Combine(folder, fileName);

                    await using var stream = new FileStream(
                        filePath,
                        FileMode.CreateNew,
                        FileAccess.Write,
                        FileShare.None);

                    await file.CopyToAsync(stream, cancellationToken);

                    savedUrls.Add($"/images/products/{fileName}");
                }

                return savedUrls;
            }
            catch
            {
                foreach (var url in savedUrls)
                {
                    var fileName = Path.GetFileName(url);
                    var filePath = Path.Combine(folder, fileName);

                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }

                throw;
            }
        }
    }
}
