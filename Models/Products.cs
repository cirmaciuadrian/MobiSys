using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class Products
    {
        public Products()
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


        [Display(Name = "Product Code")]

        [Required]
        [Column("product_code")]
        [StringLength(16)]
        public string ProductCode { get; set; }
        [Required]
        [Column("name")]
        [StringLength(16)]
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Column("weight", TypeName = "float")]
        [Range(0, 100000)]
        [Display(Name = "Weight")]
        public int Weight { get; set; }
        [Column("um_id")]
        [Display(Name = "UM")]
        public int UmId { get; set; }
        [Display(Name = "Intial Price")]
        [Column("initial_price")]
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
        [Column("details")]
        [Display(Name = "Details")]
        [StringLength(400)]
        public string Details { get; set; }
        [Column("units_per_box")]
        [Display(Name = "Units Per Box")]
        public int UnitsPerBox { get; set; }
        [Column("cateogry_id")]
        [Display(Name = "Category")]
        [Range(0, 100000)]
        public int CateogryId { get; set; }
        [Column("manufacturer_id")]
        [Display(Name = "Manufacturer")]
        public int ManufacturerId { get; set; }
        [Required]
        [Display(Name = "Image")]
        [Column("image")]
        [StringLength(80)]
        public string Image { get; set; }

     
      

        [ForeignKey(nameof(CateogryId))]
        [InverseProperty(nameof(Categories.Products))]
        public virtual Categories Cateogry { get; set; }
        [ForeignKey(nameof(ManufacturerId))]
        [InverseProperty(nameof(Manufacturers.Products))]
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
