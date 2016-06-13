using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Media.Plugin;
using Media.Plugin.Abstractions;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.Mobile.Files.Managed;

[assembly: Xamarin.Forms.Dependency(typeof(MobileAppsFilesSample.Android.AndroidPlatform))]
namespace MobileAppsFilesSample.Android
{
    public class AndroidPlatform : IPlatform
    {
        public void InitializeFiles(IMobileServiceClient client, MobileServiceSQLiteStore store)
        {
            client.InitializeManagedFileSyncContext(store, Environment.DirectoryPictures);
        }

        public async Task<Stream> TakePhotoAsync(object context)
        {
            try {
                var uiContext = context as Context;
                if (uiContext != null) {
                    var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());
                    var stream = (FileStream) photo.GetStream();

                    // breaking here works around a bug that makes this shit work... ?
                    return stream;
                }
            }
            catch (TaskCanceledException) { }

            return null;
        }
    }
}