using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class Carts
    {
        public Carts()
        {
            CartDetails = new HashSet<CartDetails>();
        }

        [Key]
        [Column("id")]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Column("cutomer_id")]
        [Display(Name = "Customer ID")]

        public int CutomerId { get; set; }
        [Column("discount_val")]
        [Display(Name = "Discount Value")]
        public decimal? DiscountVal { get; set; }
        [Column("discount_per")]
        [Display(Name = "Discount Percentage")]
        public int? DiscountPer { get; set; }

        [ForeignKey(nameof(CutomerId))]
        [InverseProperty(nameof(Customers.Carts))]
        public virtual Customers Cutomer { get; set; }
        [InverseProperty("Cart")]
        public virtual ICollection<CartDetails> CartDetails { get; set; }
    }
}
