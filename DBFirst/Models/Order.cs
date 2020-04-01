using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace DBFirst.Models
{
    public partial class Order : IValidatableObject
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("CustomerId")]
       
        public virtual Customer Customer { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        [Required]
        [NotNull]
        public  string CustomerId { get; set; }
        public virtual Employee Employee { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string OrderDate { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string RequiredDate { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string ShippedDate { get; set; }
        public long? ShipVia { get; set; }
        [Required]
        [Column(TypeName = "DECIMAL")]
        public byte[] Freight { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string ShipName { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string ShipAddress { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string ShipCity { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string ShipRegion { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string ShipPostalCode { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string ShipCountry { get; set; }
        
        public virtual List<EmployeeTerritory> EmployeeTerritories { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrEmpty(ShipAddress))
            {
                yield return new ValidationResult("ShipAddress should not be empty", new [] { nameof(ShipAddress)});
            }
        }
    }
}
