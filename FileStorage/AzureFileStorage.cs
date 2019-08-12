using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace InvoiceUploadService.API.Infrastructure.FileStorage
{
  public class AzureFileStorage : IFileStorage
  {
    private static int URL_ACCESS_TIME_IN_MINUTES = 10;

    private readonly AzureBlobStorageOptions _options;
    private readonly CloudBlobClient _azureBlobClient;

    public AzureFileStorage(
      AzureBlobStorageOptions options
    )
    {
      _options = options;
      _azureBlobClient = initializeCloudStorageClient(_options);
    }
    public async Task UploadFile(Stream fileStream, string fileName, string containerName)
    {
      if (fileStream == null)
      {
        throw new ArgumentNullException("Argument fileStream is required");
      }
      if (string.IsNullOrEmpty(fileName))
      {
        throw new ArgumentNullException("Argument fileName is required");
      }
      if (string.IsNullOrEmpty(containerName))
      {
        throw new ArgumentNullException("Argument containerName is required");
      }

      var container = await getOrCreatePrivateContainer(containerName);

      var blob = container.GetBlockBlobReference(fileName);

      await blob.UploadFromStreamAsync(fileStream);
    }

    public async Task<string> GenerateUrlToFile(string fileName, string containerName, string downloadFileName = null)
    {
      if (string.IsNullOrEmpty(fileName))
      {
        throw new ArgumentNullException("Argument fileName is required");
      }
      if (string.IsNullOrEmpty(containerName))
      {
        throw new ArgumentNullException("Argument containerName is required");
      }
      downloadFileName = downloadFileName ?? fileName;

      var container = await getOrCreatePrivateContainer(containerName);

      var blob = container.GetBlockBlobReference(fileName);

      var sasToken = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
      {
        Permissions = SharedAccessBlobPermissions.Read,
        SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(URL_ACCESS_TIME_IN_MINUTES),
      }, new SharedAccessBlobHeaders()
      {
        ContentDisposition = $"attachment; filename={downloadFileName}"
      });
      var blobUrl = string.Format("{0}{1}", blob.Uri, sasToken);
      return blobUrl;
    }

    public async Task DownloadFileToStream(Stream stream, string fileName, string containerName)
    {
      if (stream == null)
      {
        throw new ArgumentNullException("Argument stream is required");
      }
      if (string.IsNullOrEmpty(fileName))
      {
        throw new ArgumentNullException("Argument fileName is required");
      }
      if (string.IsNullOrEmpty(containerName))
      {
        throw new ArgumentNullException("Argument containerName is required");
      }

      var container = await getOrCreatePrivateContainer(containerName);

      var blob = container.GetBlockBlobReference(fileName);

      await blob.DownloadToStreamAsync(stream);
    }

    private static CloudBlobClient initializeCloudStorageClient(AzureBlobStorageOptions options)
    {
      string storageConnectionString = "DefaultEndpointsProtocol=https;"
        + $"AccountName={options.AccountName}"
        + $";AccountKey={options.AccessKey}"
        + ";EndpointSuffix=core.windows.net";


      var account = CloudStorageAccount.Parse(storageConnectionString);
      return account.CreateCloudBlobClient();
    }

    private async Task<CloudBlobContainer> getOrCreatePrivateContainer(string containerName)
    {
      var container = _azureBlobClient.GetContainerReference(containerName);
      await container.CreateIfNotExistsAsync();

      var permissions = new BlobContainerPermissions
      {
        PublicAccess = BlobContainerPublicAccessType.Off
      };
      await container.SetPermissionsAsync(permissions);

      return container;
    }

  }
}
