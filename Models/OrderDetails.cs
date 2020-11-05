using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class OrderDetails
    {
        [Key]
        [Column("id")]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Column("order_id")]
        [Display(Name = "Order ID")]
        public int OrderId { get; set; }
        [Column("product_id")]
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }
        [Column("quantity")]
        [Display(Name = "Quantity")]
        [Range(0, 100000)]
        public int Quantity { get; set; }
        [Column("price")]
        [Display(Name = "Price")]
        [Range(0, 100000)]
        public decimal Price { get; set; }
        [Column("discount_per")]
        [Display(Name = "Discount Percentuage")]
        [Range(0, 50)]
        public int? DiscountPer { get; set; }
        [Column("exp_date", TypeName = "date")]
        [DataType(DataType.Date)]
        [Display(Name = "Expiration Date")]
        public DateTime? ExpDate { get; set; }

        [ForeignKey(nameof(OrderId))]
        [InverseProperty(nameof(Orders.OrderDetails))]
        public virtual Orders Order { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(Products.OrderDetails))]
        public virtual Products Product { get; set; }
    }
}
