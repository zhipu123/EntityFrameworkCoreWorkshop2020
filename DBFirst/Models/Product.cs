using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBFirst.Models
{
    public partial class Product
    {
        [Key]
        public long Id { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string ProductName { get; set; }
        public long SupplierId { get; set; }
        public long CategoryId { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string QuantityPerUnit { get; set; }
        [Required]
        [Column(TypeName = "DECIMAL")]
        public byte[] UnitPrice { get; set; }
        public long UnitsInStock { get; set; }
        public long UnitsOnOrder { get; set; }
        public long ReorderLevel { get; set; }
        public long Discontinued { get; set; }
        
        [ForeignKey(nameof(SupplierId))]
        public virtual Supplier Supplier { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }
    }
}
