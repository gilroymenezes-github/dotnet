using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsRekog
{
    internal class DetectLabelsResult
    {
        public string Name { get; set; }
        public float Confidence { get; set; }
    }
}
