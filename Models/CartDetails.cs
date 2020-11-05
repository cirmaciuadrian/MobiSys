using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class CartDetails
    {
        [Key]
        [Column("id")]
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Column("cart_id")]
        [Display(Name = "Cart ID")]
        public int CartId { get; set; }
        [Column("product_id")]
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }
        [Column("quantity")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [ForeignKey(nameof(CartId))]
        [InverseProperty(nameof(Carts.CartDetails))]
        public virtual Carts Cart { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(Products.CartDetails))]
        public virtual Products Product { get; set; }
    }
}
