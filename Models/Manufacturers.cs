using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class Manufacturers
    {
        public Manufacturers()
        {
            Products = new HashSet<Products>();
        }

        [Key]
        [Column("id")]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(16)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [InverseProperty("Manufacturer")]
        public virtual ICollection<Products> Products { get; set; }
    }
}
