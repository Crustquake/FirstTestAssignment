using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Xpandion.Data.DatabaseEntities
{
    public class DataStructure
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
