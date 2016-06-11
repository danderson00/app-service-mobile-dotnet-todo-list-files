using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Xamarin.Forms;

namespace MobileAppsFilesSample.ViewModels
{
    public class TodoListViewModel : ViewModel
    {
        private TodoItemManager manager;

        private ObservableCollection<TodoItemViewModel> todoItems;
        private string newItemText = "";
        private long pendingChanges;
        private bool isStatusBarVisible;

        public ICommand AddItemCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }

        public TodoListViewModel()
        {
            this.AddItemCommand = new DelegateCommand(AddItem);
            this.DeleteItemCommand = new DelegateCommand(DeleteItem);

            this.manager = new TodoItemManager(App.Client);
            this.manager.MobileServiceClient.EventManager.Subscribe<StoreOperationCompletedEvent>(UpdatePendingChanges);
        }

        public ObservableCollection<TodoItemViewModel> TodoItems
        {
            get { return todoItems; }
            set
            {
                todoItems = value;
                OnPropertyChanged();
            }
        }

        public string NewItemText
        {
            get { return newItemText; }
            set
            {
                newItemText = value;
                OnPropertyChanged();
            }
        }

        public long PendingChanges
        {
            get { return pendingChanges; }
            set
            {
                pendingChanges = value;
                OnPropertyChanged();
            }
        }

        public bool IsStatusBarVisible
        {
            get { return isStatusBarVisible; }
            set
            {
                isStatusBarVisible = value;
                OnPropertyChanged();
            }
        }

        private async void UpdatePendingChanges(StoreOperationCompletedEvent mobileServiceEvent)
        {
            await Task.Delay(500);
            PendingChanges = manager.MobileServiceClient.SyncContext.PendingOperations;
            IsStatusBarVisible = PendingChanges > 0;
        }

        private async Task LoadItems()
        {
            var items = await manager.GetTodoItemsAsync();
            var models = items.Select(x => new TodoItemViewModel(manager, x));
            TodoItems = new ObservableCollection<TodoItemViewModel>(models);
        }

        public async Task SyncItemsAsync()
        {
            await manager.SyncAsync();
            await LoadItems();
        }

        private async void AddItem(object data)
        {
            var newItem = new TodoItem();
            newItem.Name = NewItemText;

            await manager.SaveTaskAsync(newItem);
            await LoadItems();

            NewItemText = "";
        }

        async void DeleteItem(object data)
        {
            await manager.DeleteTaskAsync(((TodoItemViewModel)data).TodoItem);
            await LoadItems();
        }

        public async Task NavigateToDetailsView(TodoItemViewModel model, INavigation navigation)
        {
            await model.LoadImagesAsync();
            await navigation.PushAsync(new TodoItemDetailsView { BindingContext = model });
        }
    }
}
