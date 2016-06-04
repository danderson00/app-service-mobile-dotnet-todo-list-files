using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.WindowsAzure.MobileServices.Files.Managed;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices.Files;

namespace MobileAppsFilesSample
{
    public class TodoItemImageViewModel : ViewModel
    {
        private string name;
        private Action<TodoItemImageViewModel> deleteHandler;
        private TodoItemManager manager;

        public MobileServiceManagedFile File { get; private set; }

        public TodoItemImageViewModel(TodoItemManager manager, MobileServiceManagedFile file, TodoItem todoItem, Action<TodoItemImageViewModel> deleteHandler)
        {
            this.manager = manager;
            this.deleteHandler = deleteHandler;
            this.name = file.Name;
            this.File = file;

            this.Source = ImageSource.FromStream(() => manager.GetImageAsync(todoItem, file.Name).Result);

            this.InitializeCommands();
        }

        private void InitializeCommands()
        {
            DeleteCommand = new DelegateCommand(o => deleteHandler(this));
        }

        public ICommand DeleteCommand { get; set; }

        public ImageSource Source { get; set; }

        public string Name
        {
            get { return name; }
            set
            {
                if (string.Compare(name, value) != 0) {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
