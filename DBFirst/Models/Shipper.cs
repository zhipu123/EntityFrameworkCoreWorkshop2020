using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBFirst.Models
{
    public partial class Shipper
    {
        [Key]
        public long Id { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string CompanyName { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string Phone { get; set; }
    }
}
