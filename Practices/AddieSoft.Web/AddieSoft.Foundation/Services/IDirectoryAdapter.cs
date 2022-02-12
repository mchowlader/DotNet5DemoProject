using System.IO;

namespace AddieSoft.Foundation.Services
{
    public interface IDirectoryAdapter
    {
        bool Exists(string path);
        DirectoryInfo CreateDirectory(string path);
    }
}