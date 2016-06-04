using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.Files;
using Microsoft.WindowsAzure.MobileServices.Files.Metadata;
using Microsoft.WindowsAzure.MobileServices.Files.Sync;
using Microsoft.WindowsAzure.MobileServices.Sync;
using MobileAppsFilesSample;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Storage;

[assembly: Xamarin.Forms.Dependency(typeof(WinApp.WindowsStorePlatform))]
namespace WinApp
{
    public class WindowsStorePlatform : IPlatform
    {
        public async Task<Stream> TakePhotoAsync(object context)
        {
            try {
                CameraCaptureUI dialog = new CameraCaptureUI();
                Size aspectRatio = new Size(16, 9);
                dialog.PhotoSettings.CroppedAspectRatio = aspectRatio;

                StorageFile file = await dialog.CaptureFileAsync(CameraCaptureUIMode.Photo);
                return await file.OpenStreamForReadAsync();
            }
            catch (TaskCanceledException) {
                return null;
            }
        }
    }
}
