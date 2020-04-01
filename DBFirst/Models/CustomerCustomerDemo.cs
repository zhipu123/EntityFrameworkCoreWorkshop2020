using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBFirst.Models
{
    public partial class CustomerCustomerDemo
    {
        [Key]
        [Column(TypeName = "VARCHAR(8000)")]
        public string Id { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string CustomerTypeId { get; set; }
    }
}
