using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.WebAdmin.Models
{
    public class ApplicationErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
