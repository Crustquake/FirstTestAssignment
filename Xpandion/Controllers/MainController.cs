using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Xpandion.Data;
using Xpandion.WebSite.Models;
using Xpandion.WebSite.Services;
using Xpandion.WebSite.Services.Entities;

namespace Xpandion.WebSite.Controllers
{
    public class MainController : Controller
    {
        private readonly ILogger<MainController> _logger;
        private readonly XpandionContext _context;

        public MainController(ILogger<MainController> logger, XpandionContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(IFormFile[] files)
        {
            IEnumerable<IFormFile> csvFiles = files.Where(file => Path.GetExtension(file.FileName) == ".csv");
            var model = new FilesProcessingViewModel();
            if (csvFiles.Any())
            {
                using (var processor = new CsvProcessor(_context, _logger))
                {
                    var filesForProcessing = csvFiles.Select(formFile => new InputFile
                    {
                        FileName = formFile.FileName,
                        OpenStream = formFile.OpenReadStream
                    });
                    model.Result = processor.ProcessFiles(filesForProcessing);
                }
            }
            else
            {
                model.Result = "There isn't csv-files";
            }
            return View("FilesProcessing", model);
        }
    }
}
