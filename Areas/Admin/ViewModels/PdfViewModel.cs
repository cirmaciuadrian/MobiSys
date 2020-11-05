using MobiSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MobiSys.Areas.Admin.ViewModels
{
    public class PdfViewModel
    {
        [Display(Name = "Deliver ID")]
        public int DeliverId { get; set; }
        [Display(Name = "Order ID")]
        public int OrderId { get; set; }

        [Display(Name = "Customer")]
        public string Customer { get; set; }
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        [Display(Name = "Adress")]
        public string Adress { get; set; }
        [Display(Name = "Car")]
        public string Car { get; set; }
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Display(Name = "Weight")]
        public int Weight { get; set; }
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        [Display(Name = "Units Per Box")]
        public int UnitsPerBox { get; set; }

      


    }
}
