using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeCard.Infrastructure
{
    public class ZipDownload
    {
        public string FileName { get; set; }
        public IEnumerable<string> FileList { get; set; }
    }
}