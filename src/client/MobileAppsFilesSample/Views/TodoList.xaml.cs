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

            // OnPlatform<T> doesn't currently support the "Windows" target platform, so we have this check here.
            if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
            {
                syncButton.IsVisible = true;
            }
        }        

        public async void todoList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var todo = e.SelectedItem as TodoItemViewModel;

            if (todo != null) {
                await ViewModel.NavigateToDetailsView(todo, Navigation);
            }

            todoList.SelectedItem = null;
        }

        public async void todoList_Refreshing(object sender, EventArgs e)
        {
            var list = (ListView)sender;

            var success = false;

            try {
                await ViewModel.SyncItemsAsync();
                success = true;
            }
            catch (Exception) {
            }

            list.EndRefresh();
            if (!success)
                await DisplayAlert("Refresh Error", "Couldn't refresh data", "OK");
        }

        public async void syncButton_Clicked(object sender, EventArgs e)
        {
            using (var scope = new ActivityIndicatorScope(syncIndicator, true)) {
                await ViewModel.SyncItemsAsync();
            }
        }

        private class ActivityIndicatorScope : IDisposable
        {
            private bool showIndicator;
            private ActivityIndicator indicator;
            private Task indicatorDelay;

            public ActivityIndicatorScope(ActivityIndicator indicator, bool showIndicator)
            {
                this.indicator = indicator;
                this.showIndicator = showIndicator;

                if (showIndicator)
                {
                    indicatorDelay = Task.Delay(2000);
                    SetIndicatorActivity(true);
                }
                else
                {
                    indicatorDelay = Task.FromResult(0);
                }
            }

            private void SetIndicatorActivity(bool isActive)
            {
                this.indicator.IsVisible = isActive;
                this.indicator.IsRunning = isActive;
            }

            public void Dispose()
            {
                if (showIndicator)
                {
                    indicatorDelay.ContinueWith(t => SetIndicatorActivity(false), TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }
    }
}

