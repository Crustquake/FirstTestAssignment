using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using Xpandion.WebSite.Services.Entities;

namespace Xpandion.WebSite.Services
{
    public interface ICsvProcessor : IDisposable
    {
        string ProcessFiles(IEnumerable<InputFile> files);
    }
}
