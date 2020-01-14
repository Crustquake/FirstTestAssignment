using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Xpandion.WebSite.Services.Entities
{
    public class InputFile
    {
        public string FileName { get; set; }
        public Func<Stream> OpenStream { get; set; }
    }
}
