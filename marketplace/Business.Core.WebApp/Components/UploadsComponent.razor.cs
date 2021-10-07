using Business.WebApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Business.Core.WebApp.Components
{
    public partial class UploadsComponent : BaseComponent
    {
        [Inject] public ILogger<UploadsComponent> Logger { get; set;  }
        [Inject] public EnvironmentService Environment { get; set; }
        [Inject] public AzureBlobService AzureBlob {  get; set; }   
        
        private List<IBrowserFile> loadedFiles = new();
        private long maxFileSize = 8192 * 1000;
        private int maxAllowedFiles = 1;
        private bool isLoading;

        private async Task LoadFiles(InputFileChangeEventArgs e)
        {
            isLoading = true;
            loadedFiles.Clear();

            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                try
                {
                    loadedFiles.Add(file);

                    var trustedFileNameForFileStorage = Path.GetRandomFileName();
                    await AzureBlob.UploadBlobAsync(trustedFileNameForFileStorage, file.OpenReadStream(maxFileSize));
                    //var path = Path.Combine(Environment.ContentRootPath,
                    //        Environment.EnvironmentName, "unsafe_uploads",
                    //        trustedFileNameForFileStorage);

                    //await using FileStream fs = new(path, FileMode.Create);
                    //await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
                }
                catch (Exception ex)
                {
                    Logger.LogError("File: {Filename} Error: {Error}",
                        file.Name, ex.Message);
                }
            }

            isLoading = false;
        }
    }
}
