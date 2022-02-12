using Microsoft.AspNetCore.Http;

namespace AddieSoft.Foundation.Services
{
    public interface IFileStoreUtility
    {
        public (string fileName, string filePath) StoreFile(IFormFile file);
    }
}