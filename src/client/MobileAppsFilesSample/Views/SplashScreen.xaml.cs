using MobileAppsFilesSample.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MobileAppsFilesSample.Views
{
    public partial class SplashScreen : ContentPage
    {
        private readonly Func<Task> initialize;

        public SplashScreen(Func<Task> initialize)
        {
            this.initialize = initialize;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.initialize().ContinueWith(x =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PushAsync(new TodoList(new TodoListViewModel()));
                });
            });
        }
    }
}
