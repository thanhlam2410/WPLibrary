using System;
using System.Collections.Generic;
using System.Text;

namespace WPCore.Http.Models
{
    public class ResourceGetModel
    {
        public Dictionary<string, string> Header { get; set; }

        public string Domain { get; set; }

        public string Tag { get; set; }

        public double Timeout { get; set; }
    }
}
