using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Xpandion.WebSite.Services.Entities
{
    public class CsvFile
    {
        public string FileName { get; set; }
        public string CustomerName { get; set; }
        public string DataStructure { get; set; }
        public DateTime Date { get; set; }

        public int NumberOfColumns { get; set; }
        public int NumberOfRows { get; set; }
        public CsvColumn[] Columns { get; set; }
        public string[][] Records { get; set; }

        public bool TryParseFileName(string fileName)
        {
            try
            {
                FileName = Path.GetFileName(fileName);

                CultureInfo enUS = new CultureInfo("en-US");
                string format = "yyyy-MM-dd";

                string[] parts = Path.GetFileNameWithoutExtension(fileName).Split('_');
                if (parts.Length != 3 || !DateTime.TryParseExact(parts[2], format, enUS, DateTimeStyles.None, out DateTime date))
                    return false;


                CustomerName = parts[0];
                DataStructure = parts[1];
                Date = date;
                return true;
            }
            catch (Exception exception)
            {
                throw new CsvProcessorException("Exception during name parsing", exception);
            }
        }
        public bool TryParseData(Stream stream)
        {
            try
            {
                List<string> lines = new List<string>();
                using (var reader = new StreamReader(stream))
                {
                    string firstLine = reader.ReadLine();
                    string[] columns = firstLine.Split(';');
                    while (!reader.EndOfStream)
                    {
                        lines.Add(reader.ReadLine());
                    }

                    NumberOfRows = lines.Count;
                    NumberOfColumns = columns.Length;
                    Records = lines.Select(line => line.Split(';')).ToArray();
                    Columns = columns.Select(column => new CsvColumn { ColumnName = column }).ToArray();

                    for(int column = 0; column < NumberOfColumns; column++)
                    {
                        Dictionary<string, int> values = new Dictionary<string, int>();
                        for (int row = 0; row < NumberOfRows; row++)
                        {
                            string record = Records[row][column];
                            if (values.ContainsKey(record))
                            {
                                values[record] += 1;
                            }
                            else
                            {
                                values.Add(record, 1);
                            }
                        }
                        Columns[column].MostFrequentValue = values.OrderByDescending(element => element.Value).First().Key;
                        Columns[column].NumberOfUnique = values.Keys.Count;
                    }
                }

                return true;
            }
            catch (Exception exception)
            {
                throw new CsvProcessorException("Exception during data parsing", exception);
            }
        }
    }
}
