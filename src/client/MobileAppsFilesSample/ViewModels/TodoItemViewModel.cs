using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.WindowsAzure.MobileServices.Files.Managed;
using System.Linq;

namespace MobileAppsFilesSample
{
    public class TodoItemViewModel : ViewModel
    {
        private TodoItemManager itemManager;

        public ICommand AddImageCommand { get; set; }
        public ICollection<TodoItemImageViewModel> Images { get; set; }
        public TodoItem TodoItem { get; private set; }

        public TodoItemViewModel(TodoItemManager manager, TodoItem todoItem)
        {
            this.itemManager = manager;
            this.TodoItem = todoItem;
            AddImageCommand = new DelegateCommand(AddImage);
        }

        public async Task LoadImagesAsync()
        {
            var files = await this.itemManager.GetImageFilesAsync(this.TodoItem);
            var models = files.Select(x => new TodoItemImageViewModel(this.itemManager, x, this.TodoItem, DeleteImage));
            this.Images = new ObservableCollection<TodoItemImageViewModel>(models);
        }

        private async void DeleteImage(TodoItemImageViewModel imageViewModel)
        {
            await this.itemManager.DeleteImageAsync(this.TodoItem, imageViewModel.File);
            this.Images.Remove(imageViewModel);
        }

        private async void AddImage(object obj)
        {
            var imageStream = await App.Platform.TakePhotoAsync(App.UIContext);

            if (imageStream != null)
            {
                MobileServiceManagedFile file = await this.itemManager.AddImageAsync(this.TodoItem, Guid.NewGuid().ToString(), imageStream);
                this.Images.Add(new TodoItemImageViewModel(this.itemManager, file, this.TodoItem, DeleteImage));
            }
        }
    }
}
