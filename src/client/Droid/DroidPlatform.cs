using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Media.Plugin;
using Media.Plugin.Abstractions;

[assembly: Xamarin.Forms.Dependency(typeof(MobileAppsFilesSample.Droid.DroidPlatform))]
namespace MobileAppsFilesSample.Droid
{
    public class DroidPlatform : IPlatform
    {
        public async Task<Stream> TakePhotoAsync(object context)
        {
            try {
                var uiContext = context as Context;
                if (uiContext != null) {
                    var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());
                    var stream = photo.GetStream();
                    // breaking here works around a bug that makes this shit work... ?
                    return stream;
                }
            }
            catch (TaskCanceledException) { }

            return null;
        }
    }
}