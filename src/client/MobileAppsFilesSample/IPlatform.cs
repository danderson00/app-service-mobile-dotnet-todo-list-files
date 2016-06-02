using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.Files;
using Microsoft.WindowsAzure.MobileServices.Files.Metadata;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.IO;

namespace MobileAppsFilesSample
{
    public interface IPlatform
    {
        Task <string> GetTodoFilesPathAsync();

        Task<IMobileServiceFileDataSource> GetFileDataSource(MobileServiceFileMetadata metadata);

        Task<Stream> TakePhotoAsync(object context);

        Task DownloadFileAsync<T>(IMobileServiceSyncTable<T> table, MobileServiceFile file);
    }
}
