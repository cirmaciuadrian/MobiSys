using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class DeliverStatus
    {
        public DeliverStatus()
        {
            Delivers = new HashSet<Delivers>();
        }

        [Key]
        [Column("id")]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Required]
        [Column("status")]
        [StringLength(24)]
        [Display(Name = "Status")]
        public string Status { get; set; }

        [InverseProperty("StatusDeliver")]
        public virtual ICollection<Delivers> Delivers { get; set; }
    }
}
