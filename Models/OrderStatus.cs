using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            Orders = new HashSet<Orders>();
        }

        [Key]
        [Column("id")]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Required]
        [Column("status")]
        [StringLength(16)]
        [Display(Name = "Status")]
        public string Status { get; set; }

        [InverseProperty("OrderStatus")]
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
