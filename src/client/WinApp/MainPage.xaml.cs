using Microsoft.WindowsAzure.Mobile.Files.Managed;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using MobileAppsFilesSample;

namespace WinApp
{
    public sealed partial class MainPage 
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new MobileAppsFilesSample.App(async (client, store) => {
                client.InitializeManagedFileSyncContext(store);
                await client.SyncContext.InitializeAsync(store, StoreTrackingOptions.NotifyLocalAndServerOperations);
            }));
        }
    }
}
