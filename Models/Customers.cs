using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class Customers
    {
        public Customers()
        {
            Carts = new HashSet<Carts>();
            Delivers = new HashSet<Delivers>();
            Orders = new HashSet<Orders>();
            Returns = new HashSet<Returns>();
            Vauchers = new HashSet<Vauchers>();
        }

        [Key]
        [Column("id")]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Required]
        [Column("user_id")]
        [StringLength(450)]
        [Display(Name = "User ID")]
        public string UserId { get; set; }
        [Required]
        [Column("contact_person")]
        [StringLength(40)]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }
        [Required]
        [Column("phone_number")]
        [StringLength(10)]
        [Display(Name = "Phone")]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [Column("mail")]
        [StringLength(40)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Mail { get; set; }
        [Required]
        [Column("company")]
        [StringLength(80)]
        [Display(Name = "Company")]
        public string Company { get; set; }
        [Required]
        [Column("cui")]
        [StringLength(16)]
        [Display(Name = "Fiscal Code")]
        public string Cui { get; set; }
        [Required]
        [Column("registration_number")]
        [Display(Name = "Registration Number")]
        [StringLength(32)]
        public string RegistrationNumber { get; set; }
        [Required]
        [Column("adress")]
        [StringLength(32)]
        [Display(Name = "Adress")]
        public string Adress { get; set; }
        [Column("city_id")]
        [Display(Name = "City")]
        public int CityId { get; set; }
        [Required]
        [Column("bank")]
        [Display(Name = "Bank Name")]
        [StringLength(40)]
        public string Bank { get; set; }
        [Required]
        [Column("iban")]
        [Display(Name = "Iban")]
        [StringLength(40)]
        public string Iban { get; set; }
        [Required]
        [Column("isAccepted")]
        [Display(Name = "Accepted")]
        public bool IsAccepted { get; set; }
        [Column("credit")]
        [Display(Name = "Credit")]
        [Range(0, 100000)]
        public decimal? Credit { get; set; }
        [Column("type_id")]
        [Display(Name = "Type")]
        public int TypeId { get; set; }

        [ForeignKey(nameof(CityId))]
        [InverseProperty(nameof(Cities.Customers))]
        public virtual Cities City { get; set; }
        [ForeignKey(nameof(TypeId))]
        [InverseProperty("Customers")]
        public virtual Type Type { get; set; }
        [InverseProperty("Cutomer")]
        public virtual ICollection<Carts> Carts { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<Delivers> Delivers { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<Orders> Orders { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<Returns> Returns { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<Vauchers> Vauchers { get; set; }
    }
}
