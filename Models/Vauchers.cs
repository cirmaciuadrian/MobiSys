using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class Vauchers
    {
        [Key]
        [Column("id")]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Code")]
        [Column("code")]
        [StringLength(10)]
        public string Code { get; set; }
        [Column("customer_id")]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        [Column("percentage")]
        [Display(Name = "Percentage")]
        [Range(0, 50)]
        public int? Percentage { get; set; }
        [Column("value")]
        [Display(Name = "Value")]
        [Range(0, 100000)]
        public decimal? Value { get; set; }
        [Column("createdBy")]
        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }
        [Column("order_id")]
        [Display(Name = "Order ID")]
        public int? OrderId { get; set; }
        [Column("isUsed")]
        [Display(Name = "Used")]
        public bool IsUsed { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(Employees.Vauchers))]
        public virtual Employees CreatedByNavigation { get; set; }
        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(Customers.Vauchers))]
        public virtual Customers Customer { get; set; }
        [ForeignKey(nameof(OrderId))]
        [InverseProperty(nameof(Orders.Vauchers))]
        public virtual Orders Order { get; set; }
    }
}
