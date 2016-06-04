using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.Files;
using Microsoft.WindowsAzure.MobileServices.Files.Metadata;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.IO;

namespace MobileAppsFilesSample
{
    public interface IPlatform
    {
        Task<Stream> TakePhotoAsync(object context);
    }
}
