using Microsoft.AspNetCore.Http;
using MobiSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MobiSys.Areas.Admin.ViewModels
{
    public class ProductsViewModel
    {
        public ProductsViewModel()
        {
            CartDetails = new HashSet<CartDetails>();
            DeliverDetails = new HashSet<DeliverDetails>();
            OrderDetails = new HashSet<OrderDetails>();
            ProductRepricing = new HashSet<ProductRepricing>();
            ReturnDetails = new HashSet<ReturnDetails>();
        }

        [Key]
        [Column("id")]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Required]
        [Column("product_code")]
        [StringLength(16)]
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }
        [Required]
        [Column("name")]
        [StringLength(16)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Column("weight")]
        [Display(Name = "Weight")]
        [Range(0, 100000)]
        public int Weight { get; set; }
        [Column("um_id", TypeName = "float") ]
        [Display(Name = "Um")]
        public int UmId { get; set; }
        [Column("initial_price")]
        [Display(Name = "Intial Price")]
        [Range(0, 100000)]
        public decimal InitialPrice { get; set; }
        [Column("discount")]
        [Range(0, 100000)]
        [Display(Name = "Discount")]
        public decimal Discount { get; set; }
        [Column("final_price")]
        [Range(0, 100000)]
        [Display(Name = "Final Price")]
        public decimal FinalPrice { get; set; }
        [Column("quantity")]
        [Range(0, 100000)]
        [Display(Name = "Quanitty")]
        public int Quantity { get; set; }
        [Required]
        [Display(Name = "Details")]
        [Column("details")]
        [StringLength(400)]
        public string Details { get; set; }
        [Column("units_per_box")]
        [Range(0, 100000)]
        [Display(Name = "Units Per Box")]
        public int UnitsPerBox { get; set; }
        [Column("cateogry_id")]
        [Display(Name = "Category")]
        public int CateogryId { get; set; }
        [Column("manufacturer_id")]
        [Display(Name = "Manugacturere")]
        public int ManufacturerId { get; set; }
        
        [Required]
        [Display(Name = "Image")]


        public IFormFile photo { get; set; }

        [Display(Name = "Category")]
        [ForeignKey(nameof(CateogryId))]
        [InverseProperty(nameof(Categories.Products))]

        public virtual Categories Cateogry { get; set; }
        [ForeignKey(nameof(ManufacturerId))]
        [InverseProperty(nameof(Manufacturers.Products))]
        [Display(Name = "Manufacturer")]
        public virtual Manufacturers Manufacturer { get; set; }
        [ForeignKey(nameof(UmId))]
        [InverseProperty("Products")]
        public virtual Um Um { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<CartDetails> CartDetails { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<DeliverDetails> DeliverDetails { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductRepricing> ProductRepricing { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ReturnDetails> ReturnDetails { get; set; }
    }
}
