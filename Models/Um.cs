using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class Um
    {
        public Um()
        {
            Products = new HashSet<Products>();
        }

        [Key]
        [Column("id")]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        [Column("name")]
        [StringLength(10)]
        public string Name { get; set; }

        [InverseProperty("Um")]
        public virtual ICollection<Products> Products { get; set; }
    }
}
