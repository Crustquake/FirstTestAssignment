using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Xpandion.Data.DatabaseEntities
{
    public class CsvFileColumnEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public virtual Customer Customer { get; set; }
        [Required]
        public virtual DataStructure Structure { get; set; }
    }
}
