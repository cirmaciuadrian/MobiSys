using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class Cities
    {
        public Cities()
        {
            Customers = new HashSet<Customers>();
            Employees = new HashSet<Employees>();
        }

        [Key]
        [Column("id")]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(16)]
        [Display(Name = "City")]
        public string Name { get; set; }

        [InverseProperty("City")]
        public virtual ICollection<Customers> Customers { get; set; }
        [InverseProperty("City")]
        public virtual ICollection<Employees> Employees { get; set; }
    }
}
