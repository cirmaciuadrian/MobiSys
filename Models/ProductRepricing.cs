using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class ProductRepricing
    {
        [Key]
        [Column("id")]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Column("product_id")]
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }
        [Column("price")]
        [Display(Name = "Price")]
        [Range(0, 100000)]
        public decimal Price { get; set; }
        [Column("date", TypeName = "date")]
        [Display(Name = "Date")]

        public DateTime Date { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(Products.ProductRepricing))]
        public virtual Products Product { get; set; }
    }
}
