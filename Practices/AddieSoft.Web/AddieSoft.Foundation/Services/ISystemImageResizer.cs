using System.IO;
using System.Threading.Tasks;

namespace AddieSoft.Foundation.Services
{
    public interface ISystemImageResizer
    {
        Task<FileInfo> ProfileImageResizeAsync(FileInfo temporaryImageFile);
    }
}