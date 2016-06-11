using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices;

namespace MobileAppsFilesSample
{
    public interface IPlatform
    {
        Task<Stream> TakePhotoAsync(object context);
        void InitializeFiles(IMobileServiceClient client, MobileServiceSQLiteStore store);
    }
}
