using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices.Sync;
using MobileAppsFilesSample.ViewModels;

namespace MobileAppsFilesSample
{
    public class App : Application
    {
        public static IMobileServiceClient Client { get; set; }
        public static object UIContext { get; set; }
        public static IPlatform Platform { get; set; }

        public App()
        {
            Platform = DependencyService.Get<IPlatform>();

            App.Client = new MobileServiceClient(Configuration.MobileAppURL);
            var store = new MobileServiceSQLiteStore("localstore.db");
            store.DefineTable<TodoItem>();
            Platform.InitializeFiles(App.Client, store);

            var model = new TodoListViewModel();
            this.MainPage = new NavigationPage(new TodoList(model));

            Device.BeginInvokeOnMainThread(async () =>
            {
                await App.Client.SyncContext.InitializeAsync(store, StoreTrackingOptions.NotifyLocalAndServerOperations);
                await model.SyncItemsAsync();
            });

        }
    }
}

