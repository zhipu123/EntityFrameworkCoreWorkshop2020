using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBFirst.Models
{
    public partial class EmployeeTerritory
    {
        [Key]
        [Column(TypeName = "VARCHAR(8000)")]
        public string Id { get; set; }
        public virtual Employee Employee { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public virtual Territory Territory { get; set; }
    }
}
