using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Xpandion.Data.DatabaseEntities
{
    public class CsvFileEntity
    {
        public int Id { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public Customer Customer { get; set; }
        [Required]
        public DataStructure Structure { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime ProcessingDate { get; set; }
        [Required]
        public int NumberOfColumns { get; set; }
        [Required]
        public int NumberOfRows { get; set; }
        public virtual ICollection<CsvFileColumnEntity> Columns { get; set; }
    }
}
