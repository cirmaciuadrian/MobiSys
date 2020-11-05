using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class Cars
    {
        public Cars()
        {
            Delivers = new HashSet<Delivers>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("registration_plate")]
        [StringLength(10)]
        [Display(Name = "Registration Plate")]
        public string RegistrationPlate { get; set; }

        [InverseProperty("Car")]
        public virtual ICollection<Delivers> Delivers { get; set; }
    }
}
