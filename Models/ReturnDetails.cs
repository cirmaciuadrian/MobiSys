using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class ReturnDetails
    {
        [Key]
        [Column("id")]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Column("return_id")]
        [Display(Name = "Return ID")]
        public int ReturnId { get; set; }
        [Column("product_id")]
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }
        [Column("quantity")]
        [Display(Name = "Quantity")]
        [Range(0, 100000)]
        public int Quantity { get; set; }
        [Required]
        [Column("reason")]
        [Display(Name = "Reason")]
        [StringLength(400)]
        public string Reason { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(Products.ReturnDetails))]
        public virtual Products Product { get; set; }
        [ForeignKey(nameof(ReturnId))]
        [InverseProperty(nameof(Returns.ReturnDetails))]
        public virtual Returns Return { get; set; }
    }
}
