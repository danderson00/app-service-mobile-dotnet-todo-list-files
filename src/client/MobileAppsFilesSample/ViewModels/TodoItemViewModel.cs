using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices.Files.Managed;

namespace MobileAppsFilesSample
{
    public class TodoItemViewModel : ViewModel
    {
        private TodoItem todoItem;
        private TodoItemManager itemManager;

        private TodoItemViewModel() { }

        public static async Task<TodoItemViewModel> CreateAsync(TodoItem todoItem, TodoItemManager itemManager)
        {
            if (todoItem == null) {
                throw new ArgumentNullException("todoItem");
            }

            if (itemManager == null) {
                throw new ArgumentNullException("itemManager");
            }

            TodoItemViewModel result = new TodoItemViewModel()
            {
                todoItem = todoItem,
                itemManager = itemManager
            };

			await result.LoadImagesAsync();
            result.InitializeCommands();

            return result;
        }

        private void InitializeCommands()
        {
            AddImageCommand = new DelegateCommand(AddImage);
        }

        public async Task LoadImagesAsync()
        {
            IEnumerable<MobileServiceManagedFile> files = await this.itemManager.GetImageFilesAsync(todoItem);
            this.Images = new ObservableCollection<TodoItemImageViewModel>();

            foreach (var file in files) {
                var viewModel = new TodoItemImageViewModel(this.itemManager, file, this.todoItem, DeleteImage);
                this.Images.Add(viewModel);
            }
        }

        private async void DeleteImage(TodoItemImageViewModel imageViewModel)
        {
            await this.itemManager.DeleteImageAsync(this.todoItem, imageViewModel.File);

            this.Images.Remove(imageViewModel);
        }

        public ICommand AddImageCommand { get; set; }

        public string Name
        {
            get { return this.todoItem.Name; }
            set
            {
                if (string.Compare(this.todoItem.Name, value) != 0)
                {
                    this.todoItem.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICollection<TodoItemImageViewModel> Images { get; set; }

        private async void AddImage(object obj)
        {
            IPlatform mediaProvider = DependencyService.Get<IPlatform>();
            var imageStream = await mediaProvider.TakePhotoAsync(App.UIContext);

            //var mediaPicker = new MediaPicker(App.UIContext);
            //var photo = await mediaPicker.TakePhotoAsync(new StoreCameraMediaOptions());

            if (imageStream != null)
            {
                MobileServiceManagedFile file = await this.itemManager.AddImageAsync(this.todoItem, Guid.NewGuid().ToString(), imageStream);

                var image = new TodoItemImageViewModel(this.itemManager, file, this.todoItem, DeleteImage);
                this.Images.Add(image);
            }
        }

        internal TodoItem GetItem()
        {
            return this.todoItem;
        }
    }
}
