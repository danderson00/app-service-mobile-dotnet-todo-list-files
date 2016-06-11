using System;
using System.Windows.Input;
using Microsoft.WindowsAzure.MobileServices.Files.Managed;
using Xamarin.Forms;

namespace MobileAppsFilesSample
{
    public class TodoItemImageViewModel : ViewModel
    {
        private string name;
        private Action<TodoItemImageViewModel> deleteHandler;
        private TodoItemManager manager;

        public MobileServiceManagedFile File { get; private set; }
        public ICommand DeleteCommand { get; set; }
        public ImageSource Source { get; set; }
        public string Name { get; set; }

        public TodoItemImageViewModel(TodoItemManager manager, MobileServiceManagedFile file, TodoItem todoItem, Action<TodoItemImageViewModel> deleteHandler)
        {
            this.manager = manager;
            this.deleteHandler = deleteHandler;
            this.name = file.Name;
            this.File = file;
            this.Source = ImageSource.FromStream(() => manager.GetImageAsync(todoItem, file.Name).Result);
            this.DeleteCommand = new DelegateCommand(o => deleteHandler(this));
        }
    }
}
