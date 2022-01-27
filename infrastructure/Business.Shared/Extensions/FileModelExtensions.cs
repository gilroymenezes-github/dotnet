using Infrastructure.Models;
using System;

namespace Infrastructure.Extensions
{
    public static class FileModelExtensions
    {
        public static FileModel CreateFromFileModel(this FileModel model, string filename, string extension)
        {
            model.Id = Guid.NewGuid().ToString();
            model.CreatedAtDateTimeUtc = DateTime.UtcNow;
            model.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            model.Name = filename; 
            model.Extension = extension;
            return model;
        }

        public static FileModel UpdateFromFileModel(this FileModel model)
        {
            model.CreatedAtDateTimeUtc = DateTime.UtcNow;
            model.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            return model;
        }
    }
}
