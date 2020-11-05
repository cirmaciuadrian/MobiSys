using MobiSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MobiSys.Areas.Admin.ViewModels
{
    public class CreateDeliverViewModel
    {
        
        
        [DataType(DataType.Date)]
        [Display(Name = "Shipping Date")]

        public DateTime? ShippingDate { get; set; }

        [Display(Name = "Order ID")]
        public int OrderId { get; set; }


        [Display(Name = "Product Name")]
        public string productName { get; set; }
        [Display(Name = "Product ID")]
        public int productId { get; set; }

        [Display(Name = "Exiration Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime? ExpDate { get; set; }
        [Display(Name = "Car")]

        [Column("car_id")]
        public int? CarId { get; set; }

        [Display(Name = "Car")]

        [ForeignKey(nameof(CarId))]
        [InverseProperty(nameof(Cars.Delivers))]
        public virtual Cars Car { get; set; }


    }
}





