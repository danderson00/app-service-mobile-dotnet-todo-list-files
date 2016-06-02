﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.Files;
using Microsoft.WindowsAzure.MobileServices.Files.Metadata;
using Microsoft.WindowsAzure.MobileServices.Files.Sync;
using Microsoft.WindowsAzure.MobileServices.Sync;
using MobileAppsFilesSample;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Storage;

[assembly: Xamarin.Forms.Dependency(typeof(WinApp.WindowsStorePlatform))]
namespace WinApp
{
    public class WindowsStorePlatform : IPlatform
    {
        public async Task DownloadFileAsync<T>(IMobileServiceSyncTable<T> table, MobileServiceFile file)
        {
            var path = await FileHelper.GetLocalFilePathAsync(file.ParentId, file.Name);
            var folder = Path.GetDirectoryName(path);

            var storageFolder = await StorageFolder.GetFolderFromPathAsync(folder);

            if (await storageFolder.TryGetItemAsync(file.Name) == null) {
                await storageFolder.CreateFileAsync(file.Name);
            }

            await table.DownloadFileAsync(file, path);
        }

        public async Task<IMobileServiceFileDataSource> GetFileDataSource(MobileServiceFileMetadata metadata)
        {
            var filePath = await FileHelper.GetLocalFilePathAsync(metadata.ParentDataItemId, metadata.FileName);
            return new PathMobileServiceFileDataSource(filePath);
        }

        public async Task<string> GetTodoFilesPathAsync()
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var filePath = "TodoItemFiles";

            var result = await storageFolder.TryGetItemAsync(filePath);

            if (result == null) {
                result = await storageFolder.CreateFolderAsync(filePath);
            }

            // return result.Name; // later operations will use relative paths
            return result.Path;
        }

        public async Task<Stream> TakePhotoAsync(object context)
        {
            try {
                CameraCaptureUI dialog = new CameraCaptureUI();
                Size aspectRatio = new Size(16, 9);
                dialog.PhotoSettings.CroppedAspectRatio = aspectRatio;

                StorageFile file = await dialog.CaptureFileAsync(CameraCaptureUIMode.Photo);
                return await file.OpenStreamForReadAsync();
            }
            catch (TaskCanceledException) {
                return null;
            }
        }
    }
}
