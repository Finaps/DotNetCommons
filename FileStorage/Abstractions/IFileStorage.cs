using System.IO;
using System.Threading.Tasks;
namespace Finaps.Commons.FileStorage
{
  public interface IFileStorage
  {
    Task UploadFile(Stream fileStream, string fileName, string containerName);
    Task<string> GenerateUrlToFile(string fileName, string containerName, string downloadFileName = null);
    Task DownloadFileToStream(Stream stream, string fileName, string containerName);
  }
}
