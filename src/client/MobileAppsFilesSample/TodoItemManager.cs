using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Files.Managed;
using System.IO;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace MobileAppsFilesSample
{
    public partial class TodoItemManager
    {
        MobileServiceClient client;

        IMobileServiceSyncTable<TodoItem> todoTable;

        private TodoItemManager() { }

        public static async Task<TodoItemManager> CreateAsync()
        {
            var result = new TodoItemManager();
            result.client = new MobileServiceClient(Constants.ApplicationURL);

            var store = new MobileServiceSQLiteStore("localstore.db");

            store.DefineTable<TodoItem>();

            // Initialize file sync
            result.client.InitializeManagedFileSyncContext(store);

            // Initialize the SyncContext using the default IMobileServiceSyncHandler
            await result.client.SyncContext.InitializeAsync(store, StoreTrackingOptions.NotifyLocalAndServerOperations);

            result.todoTable = result.client.GetSyncTable<TodoItem>();


            return result;
        }

        public IMobileServiceClient MobileServiceClient
        {
            get
            {
                return this.client;
            }
        }

        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try {
                // FILES: Push file changes
                await this.todoTable.PushFileChangesAsync();

                // FILES: Automatic pull
                // A normal pull will automatically process new/modified/deleted files, engaging the file sync handler
                await this.todoTable.PullAsync("todoItems", this.todoTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc) {
                if (exc.PushResult != null) {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null) {
                foreach (var error in syncErrors) {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null) {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }
                }
            }
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync()
        {
            try {
                return await todoTable.OrderBy(item => item.Name).ToListAsync();
            }
            catch (MobileServiceInvalidOperationException msioe) {
                Debug.WriteLine(@"INVALID {0}", msioe.Message);
            }
            catch (Exception e) {
                Debug.WriteLine(@"ERROR {0}", e.Message);
            }
            return null;
        }

        public async Task SaveTaskAsync(TodoItem item)
        {
            if (item.Id == null) {
                await todoTable.InsertAsync(item);
            }
            else {
                await todoTable.UpdateAsync(item);
            }
        }

        public async Task DeleteTaskAsync(TodoItem item)
        {
            try {
                await todoTable.DeleteAsync(item);
            }
            catch (MobileServiceInvalidOperationException msioe) {
                Debug.WriteLine(@"INVALID {0}", msioe.Message);
            }
            catch (Exception e) {
                Debug.WriteLine(@"ERROR {0}", e.Message);
            }
        }

        internal async Task DownloadFileAsync(MobileServiceManagedFile file)
        {
            var todoItem = await todoTable.LookupAsync(file.ParentId);
            Debug.WriteLine ("++ Downloading file: " + todoItem.Name);

            IPlatform platform = DependencyService.Get<IPlatform>();
            await platform.DownloadFileAsync(this.todoTable, file);
        }

        internal async Task<MobileServiceManagedFile> AddImage(TodoItem todoItem, string name, Stream content)
        {
            // FILES: Creating/Adding file
            return await this.todoTable.AddFileAsync(todoItem, name, content);
        }

        internal async Task DeleteImage(TodoItem todoItem, MobileServiceManagedFile file)
        {
            // FILES: Deleting file
            await this.todoTable.DeleteFileAsync(todoItem, file.Name);

            // "Touch" the record to mark it as updated
            await this.todoTable.UpdateAsync(todoItem);
        }

        internal async Task<IEnumerable<MobileServiceManagedFile>> GetImageFilesAsync(TodoItem todoItem)
        {
            // FILES: Get files (local)
            return await this.todoTable.GetFilesAsync(todoItem);
        }
    }
}
