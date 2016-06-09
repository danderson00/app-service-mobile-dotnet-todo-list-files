using Android.App;
using Android.Content.PM;
using Android.OS;
using Java.Lang;
using Microsoft.WindowsAzure.Mobile.Files.Managed;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace MobileAppsFilesSample.Droid
{
    [Activity(Label = "MobileAppsFilesSample.Droid", 
        Icon = "@drawable/icon", 
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        Theme = "@android:style/Theme.Holo.Light")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            CurrentPlatform.Init();
            App.UIContext = this;

            LoadApplication(new App(async (client, store) => {
                client.InitializeManagedFileSyncContext(store, Environment.DirectoryPictures);
                await client.SyncContext.InitializeAsync(store, StoreTrackingOptions.NotifyLocalAndServerOperations);
            }));
        }
    }
}

