using System;
using System.Collections.Generic;
using System.Text;

namespace Xpandion.WebSite.Services.Entities
{
    public class CsvColumn
    {
        public string ColumnName { get; set; }
        public int NumberOfUnique { get; set; }
        public string MostFrequentValue { get; set; }
    }
}
