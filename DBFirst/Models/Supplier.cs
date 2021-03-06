﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBFirst.Models
{
    public partial class Supplier
    {
        [Key]
        public long Id { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string CompanyName { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string ContactName { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string ContactTitle { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string Address { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string City { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string Region { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string PostalCode { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string Country { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string Phone { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string Fax { get; set; }
        [Column(TypeName = "VARCHAR(8000)")]
        public string HomePage { get; set; }
    }
}
