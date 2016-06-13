using MobileAppsFilesSample.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileAppsFilesSample
{
    public partial class TodoList : ContentPage
    {
        private TodoListViewModel ViewModel
        {
            get { return this.BindingContext as TodoListViewModel; }
        }

        public TodoList(TodoListViewModel viewModel)
        {
            this.BindingContext = viewModel;

            InitializeComponent();

            if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
            {
                syncButton.IsVisible = true;
            }
        }        

        public async void todoList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var todoItem = e.SelectedItem as TodoItemViewModel;

            if (todoItem != null)
            {
                await ViewModel.NavigateToDetailsView(todoItem, Navigation);
            }

            todoList.SelectedItem = null;
        }

        public async void todoList_Refreshing(object sender, EventArgs e)
        {
            await SyncAsync();
            ((ListView)sender).EndRefresh();
        }

        public async void syncButton_Clicked(object sender, EventArgs e)
        {
            using (new ActivityIndicatorScope(syncIndicator, 2000))
            {
                await SyncAsync();
            }
        }

        private async Task SyncAsync()
        {
            try
            {
                // there is a problem with exceptions being swallowed by PullAsync
                await ViewModel.SyncItemsAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Refresh Error", $"Couldn't refresh data - {ex.Message}", "OK");
            }
        }
    }
}

