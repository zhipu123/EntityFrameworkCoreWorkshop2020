using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBFirst.Models
{
    public partial class CustomerDemographic
    {
        [Key]
        [Column(TypeName = "VARCHAR(8000)")]
        public string Id { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string CustomerDesc { get; set; }
    }
}
