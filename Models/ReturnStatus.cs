using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class ReturnStatus
    {
        public ReturnStatus()
        {
            Returns = new HashSet<Returns>();
        }

        [Key]
        [Column("id")]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Status")]
        [Column("status")]
        [StringLength(32)]
        public string Status { get; set; }

        [InverseProperty("ReturnStatus")]
        public virtual ICollection<Returns> Returns { get; set; }
    }
}
