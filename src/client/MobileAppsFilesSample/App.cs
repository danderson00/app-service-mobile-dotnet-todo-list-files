using MobileAppsFilesSample.ViewModels;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using System;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using MobileAppsFilesSample.Views;

namespace MobileAppsFilesSample
{
    public class App : Application
    {
        public static IMobileServiceClient Client { get; set; }
        public Func<IMobileServiceClient, MobileServiceSQLiteStore, Task> Initializer { get; private set; }

        public App(Func<IMobileServiceClient, MobileServiceSQLiteStore, Task> initializer)
        {
            Initializer = initializer;

            App.Client = new MobileServiceClient(Constants.ApplicationURL);
            var store = new MobileServiceSQLiteStore("localstore.db");
            store.DefineTable<TodoItem>();

            //await Initializer(App.Client, store);
            this.MainPage = new NavigationPage(new SplashScreen(async () => {
                await Initializer(App.Client, store);
            }));
            //Device.BeginInvokeOnMainThread(async () => {
            //    await initializer(App.Client, store);
            //});

            //initializer(App.Client, store).ContinueWith(x =>
            //{
            //    this.MainPage = new NavigationPage(new TodoList(new TodoListViewModel()));
            //}, TaskScheduler.FromCurrentSynchronizationContext());

            //this.Initializer = initializer;
        }

        public static object UIContext { get; set; }

        protected override async void OnStart()
        {
        }

        protected override void OnSleep () { }

        protected override void OnResume () { }
    }
}

