using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Shared
{
    public class FileModel : BaseModel
    {
        public string Extension { get; set; }
        public string Type { get; set; }
    }
}
