using Infrastructure.Models;
using System;

namespace Infrastructure.Extensions
{
    public static class FileModelExtensions
    {
        public static FileEntity CreateFromFileModel(this FileEntity model, string filename, string extension)
        {
            model.Id = Guid.NewGuid().ToString();
            model.CreatedAtDateTimeUtc = DateTime.UtcNow;
            model.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            model.Title = filename; 
            model.Extension = extension;
            return model;
        }

        public static FileEntity UpdateFromFileModel(this FileEntity model)
        {
            model.CreatedAtDateTimeUtc = DateTime.UtcNow;
            model.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            return model;
        }
    }
}
