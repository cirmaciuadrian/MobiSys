using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class Type
    {
        public Type()
        {
            Customers = new HashSet<Customers>();
        }

        [Key]
        [Column("id")]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        [Column("name")]
        [StringLength(16)]
        public string Name { get; set; }

        [InverseProperty("Type")]
        public virtual ICollection<Customers> Customers { get; set; }
    }
}
