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
    public class EditProductViewModel
    {
        public EditProductViewModel()
        {
            CartDetails = new HashSet<CartDetails>();
            DeliverDetails = new HashSet<DeliverDetails>();
            OrderDetails = new HashSet<OrderDetails>();
            ProductRepricing = new HashSet<ProductRepricing>();
            ReturnDetails = new HashSet<ReturnDetails>();
        }

        [Key]
        [Column("id")]
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Product Code")]
        [Column("product_code")]
        [StringLength(16)]

        public string ProductCode { get; set; }
        [Required]
        [Column("name")]
        [StringLength(16)]
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Column("weight")]
        [Display(Name = "Weight")]
        [Range(0, 100000)]
        public int Weight { get; set; }
        [Column("um_id", TypeName = "float")]
        [Display(Name = "UM")]

        public int UmId { get; set; }
        [Column("initial_price")]
        [Display(Name = "Initial Price")]
        [Range(0, 100000)]
        public decimal InitialPrice { get; set; }
        [Column("discount")]
        [Display(Name = "Discount")]
        [Range(0, 100000)]
        public decimal Discount { get; set; }
        [Column("final_price")]
        [Display(Name = "Final Price")]
        [Range(0, 100000)]
        public decimal FinalPrice { get; set; }
        [Column("quantity")]
        [Display(Name = "Quantity")]
        [Range(0, 100000)]
        public int Quantity { get; set; }
        [Required]
        [Display(Name = "Details")]

        [Column("details")]
        [StringLength(400)]
        public string Details { get; set; }
        [Column("units_per_box")]
        [Display(Name = "Units Per Box")]
        [Range(0, 100000)]
        public int UnitsPerBox { get; set; }
        [Column("cateogry_id")]
        [Display(Name = "Category")]
        public int CateogryId { get; set; }
        [Column("manufacturer_id")]
        [Display(Name = "Manufacturer")]
        public int ManufacturerId { get; set; }

        [Display(Name = "Image")]
        public string Image { get; set; }
        [Display(Name = "Image")]
        public IFormFile photo { get; set; }

        [ForeignKey(nameof(CateogryId))]
        [InverseProperty(nameof(Categories.Products))]
        [Display(Name = "Category")]
        public virtual Categories Cateogry { get; set; }
        [ForeignKey(nameof(ManufacturerId))]
        [InverseProperty(nameof(Manufacturers.Products))]
        [Display(Name = "Manugacturer")]
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