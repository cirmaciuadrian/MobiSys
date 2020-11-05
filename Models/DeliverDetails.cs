using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class DeliverDetails
    {
        [Key]
        [Column("id")]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Column("deliver_id")]
        [Display(Name = "Deliver ID")]
        public int DeliverId { get; set; }
        [Column("product_id")]
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }
        [Column("quantity")]
        [Display(Name = "Quantity")]
        [Range(0, 100000)]
        public int Quantity { get; set; }

        [ForeignKey(nameof(DeliverId))]
        [InverseProperty(nameof(Delivers.DeliverDetails))]
        public virtual Delivers Deliver { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(Products.DeliverDetails))]
        public virtual Products Product { get; set; }
    }
}
