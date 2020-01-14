using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;

using Xpandion.Data;
using Xpandion.Data.DatabaseEntities;
using Xpandion.WebSite.Services.Entities;

namespace Xpandion.WebSite.Services
{
    public sealed class CsvProcessor : ICsvProcessor
    {
        private readonly XpandionContext _context;
        private readonly ILogger _logger;

        public CsvProcessor(XpandionContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }
        public string ProcessFiles(IEnumerable<InputFile> files)
        {
            try
            {
                var result = new StringBuilder();

                _logger.LogInformation($"Start processing {files.Count()} files");
                result.AppendLine($"{files.Count()} files was uploaded:");

                //Grouping by Customer Name and Data Structure
                var parsedNames = files.Select(file =>
                    {
                        var csvFile = new CsvFile();
                        var parseResult = csvFile.TryParseFileName(file.FileName);
                        return new { ParseResult = parseResult, CsvFile = csvFile, file.OpenStream };
                    });

                foreach (var wrongName in parsedNames.Where(file => !file.ParseResult))
                {
                    result.AppendLine($"• File \"{wrongName.CsvFile.FileName}\" has wrong name format");
                }

                var groups = parsedNames
                    .Where(file => file.ParseResult)
                    .GroupBy(file => new { file.CsvFile.CustomerName, file.CsvFile.DataStructure });

                foreach(var group in groups)
                {
                    var orderedFiles = group.OrderBy(file => file.CsvFile.Date);

                    CsvFile previousFile = null;
                    foreach(var file in orderedFiles)
                    {
                        using (var stream = file.OpenStream())
                        {
                            var parseResult = file.CsvFile.TryParseData(stream);
                            if (!parseResult)
                            {
                                result.AppendLine($"• File \"{file.CsvFile.FileName}\" has wrong data format");
                            }
                            else
                            {
                                result.AppendLine($"• Analyse file \"{file.CsvFile.FileName}\":");
                                result.AppendLine($"\tNumber of columns: {file.CsvFile.NumberOfColumns}; Number of rows: {file.CsvFile.NumberOfRows}");
                                foreach (var column in file.CsvFile.Columns)
                                {
                                    result.AppendLine($"\tColumn name: \"{column.ColumnName}\"; Number of unique values: {column.NumberOfUnique}; Most frequent value: \"{column.MostFrequentValue}\"");
                                }

                                if (previousFile != null)
                                {
                                    var addedColumns = file.CsvFile.Columns.Select(c => c.ColumnName).Except(previousFile.Columns.Select(c => c.ColumnName));
                                    if (addedColumns.Any())
                                        result.AppendLine($"\t\tAdded columns: {string.Join(", ", addedColumns)}");

                                    var removedColumns = previousFile.Columns.Select(c => c.ColumnName).Except(file.CsvFile.Columns.Select(c => c.ColumnName));
                                    if (removedColumns.Any())
                                        result.AppendLine($"\t\tRemoved columns: {string.Join(", ", removedColumns)}");
                                }

                                previousFile = file.CsvFile;

                                var customer = _context.Customers.FirstOrDefault(customer => customer.Name == file.CsvFile.CustomerName);
                                if (customer == null)
                                {
                                    customer = new Customer { Name = file.CsvFile.CustomerName };
                                    _context.Customers.Add(customer);
                                }

                                var structure = _context.DataStructures.FirstOrDefault(structure => structure.Name == file.CsvFile.DataStructure);
                                if (structure == null)
                                {
                                    structure = new DataStructure { Name = file.CsvFile.DataStructure };
                                    _context.DataStructures.Add(structure);
                                }

                                var fileEntity = new CsvFileEntity
                                {
                                    FileName = file.CsvFile.FileName,
                                    Customer = customer,
                                    Structure = structure,
                                    Date = file.CsvFile.Date,
                                    ProcessingDate = DateTime.Now,
                                    NumberOfColumns = file.CsvFile.NumberOfColumns,
                                    NumberOfRows = file.CsvFile.NumberOfRows,
                                    Columns = new List<CsvFileColumnEntity>()
                                };

                                foreach (var fileColumn in file.CsvFile.Columns)
                                {
                                    var columnEntity = _context.Columns
                                        .FirstOrDefault(column => column.Name == fileColumn.ColumnName 
                                            && column.Customer == customer 
                                            && column.Structure == structure);
                                    if (columnEntity == null)
                                    {
                                        columnEntity = new CsvFileColumnEntity { Name = fileColumn.ColumnName, Customer = customer, Structure = structure };
                                        _context.Columns.Add(columnEntity);
                                    }
                                    fileEntity.Columns.Add(columnEntity);
                                }

                                _context.Files.Add(fileEntity);
                                _context.SaveChanges();
                            }
                        }
                    }
                }

                return result.ToString();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Exeception during processing files");
                return "Processing failed, please contact administrator";
            }
        }

        #region IDisposable Support
        private bool isDisposed = false;
        private void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                isDisposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
