using Business.Shared;
using Business.Shared.Connections;
using Business.Shared.Extensions;
using Business.Shared.Models;
using Business.WebApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Business.Core.WebApp.Components
{
    public partial class UploadsComponent : BaseComponent
    {
        [Inject] public ILogger<UploadsComponent> Logger { get; set;  }
        [Inject] public EnvironmentService Environment { get; set; }
        [Inject] public FilesHttpClientWithAuth<FileModel> FilesClient {  get; set; }   
        
        private List<IBrowserFile> loadedFiles = new();
        private List<string> blobNames = new();
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

                    var fileModel = new FileModel();
                    fileModel.CreateFromFileModel(trustedFileNameForFileStorage, "pdf");

                    using (var content = new MultipartFormDataContent())
                    {
                        content.Add(new StreamContent(file.OpenReadStream(maxFileSize))
                        {
                            Headers = { ContentLength = file.Size, ContentType = new MediaTypeHeaderValue(file.ContentType) }
                        }, "File", trustedFileNameForFileStorage);
                        var response = await FilesClient.UploadPdfFile(fileModel, content);
                    }
                    blobNames.Add(trustedFileNameForFileStorage);
                }
                catch (Exception ex)
                {
                    Logger.LogError("File: {Filename} Error: {Error}",
                        file.Name, ex.Message);
                }
            }

            isLoading = false;
        }

        //private async Task DownloadFile(MouseEventArgs e, string item)
        //{
        //    await AzureBlob.DownloadBlobAsync(item, $"c:/Users/gilroy/Desktop/{item}.pdf");
        //}
    }
}
