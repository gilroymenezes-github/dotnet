using Business.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Shared.Extensions
{
    public static class FileModelExtensions
    {
        public static FileModel CreateFromFileModel(this FileModel model, string filename, string extension)
        {
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
