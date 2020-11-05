using MobiSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MobiSys.ViewModels
{
    
    public partial class ProductsCustomersViewModel
    {
        public ProductsCustomersViewModel()
        {
            
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
       
        [Required]
        [Column("name")]
        [StringLength(16)]
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Column("weight", TypeName = "float")]
        [Display(Name = "Weight")]
        [Range(0, 100000)]
        public int Weight { get; set; }
        [Column("um_id")]
        [Range(0, 100000)]
        [Display(Name = "UM")]
        public int UmId { get; set; }
        [Display(Name = "Initial Price")]
        [Range(0, 100000)]
        [Column("initial_price")]
        public decimal InitialPrice { get; set; }

        [Display(Name = "Final Price")]
        [Range(0, 100000)]
        [Column("final_price")]
        public decimal FinalPrice { get; set; }
        [Display(Name = "Category")]
        [Column("cateogry_id")]
        public int CateogryId { get; set; }
        
        [Required]
        [Column("image")]
        [StringLength(80)]
        [Display(Name = "Image")]
        public string Image { get; set; }

        
     
    }
}

