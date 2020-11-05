using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class Categories
    {
        public Categories()
        {
            Products = new HashSet<Products>();
        }

        [Key]
        [Column("id")]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(64)]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [InverseProperty("Cateogry")]
        public virtual ICollection<Products> Products { get; set; }
    }
}
