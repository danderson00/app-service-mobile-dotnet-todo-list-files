using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Xamarin.Media;

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
                    var mediaPicker = new MediaPicker(uiContext);
                    var photo = await mediaPicker.TakePhotoAsync(new StoreCameraMediaOptions());

                    return photo.GetStream();
                }
            }
            catch (TaskCanceledException) {
            }

            return null;
        }
    }
}