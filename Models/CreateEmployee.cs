using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MobiSys.Models
{
    public class CreateEmployee
    {
        

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

        [Required]
        [Column("email")]
        [EmailAddress]
        [StringLength(64)]
        [Display(Name = "Email")]
        
        public string email { get; set; }
        [Required]
        [Column("adress")]
        [StringLength(64)]
        [Display(Name = "Adress")]
        public string Adress { get; set; }
        [Column("city_id")]
        public int CityId { get; set; }
        [Required]
        [Column("job_id")]
        [StringLength(450)]
        [Display(Name = "Job")]
        public string JobId { get; set; }
        [Column("salary")]
        [Display(Name = "Salary")]
        [Range(0, 100000)]
        public int Salary { get; set; }

        [ForeignKey(nameof(CityId))]
        [InverseProperty(nameof(Cities.Employees))]
        public virtual Cities City { get; set; }
    
}
}
