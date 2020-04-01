using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBFirst.Models
{
    public partial class OrderDetail
    {
        [Key]
        [Column(TypeName = "VARCHAR(8000)")]
        public string Id { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        [Required]
        [Column(TypeName = "DECIMAL")]
        public byte[] UnitPrice { get; set; }
        public long Quantity { get; set; }
        [Column(TypeName = "DOUBLE")]
        public double Discount { get; set; }
    }
}
