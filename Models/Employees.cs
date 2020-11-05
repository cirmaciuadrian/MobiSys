using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class Employees
    {
        public Employees()
        {
            Delivers = new HashSet<Delivers>();
            Orders = new HashSet<Orders>();
            Returns = new HashSet<Returns>();
            Vauchers = new HashSet<Vauchers>();
        }

        [Key]
        [Column("id")]
        [Display(Name = "ID")]
        public int Id { get; set; }
       
        [Column("user_id")]
        [StringLength(450)]
        [Display(Name = "User ID")]
        public string UserId { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(64)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Column("email")]
        [Display(Name = "Email")]
        [EmailAddress]
        [StringLength(64)]
        public string email { get; set; }
        [Required]
        [Column("adress")]
        [StringLength(64)]
        [Display(Name = "Adress")]
        public string Adress { get; set; }
        [Column("city_id")]
        [Display(Name = "City")]
        public int CityId { get; set; }
        [Required]
        [Column("job_id")]
        [Display(Name = "Job")]
        [StringLength(450)]
        public string JobId { get; set; }
        [Column("salary")]
        [Display(Name = "Salary")]
        public int Salary { get; set; }

        [ForeignKey(nameof(CityId))]
        [InverseProperty(nameof(Cities.Employees))]
        public virtual Cities City { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<Delivers> Delivers { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<Orders> Orders { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<Returns> Returns { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<Vauchers> Vauchers { get; set; }
    }
}
