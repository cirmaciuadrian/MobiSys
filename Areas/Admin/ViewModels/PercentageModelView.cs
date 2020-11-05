using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MobiSys.Areas.Admin.ViewModels
    
{
    public partial class PercentageModelView
    {
        
        [Required]
        [Display(Name = "Customer ID")]
        public int CustomerId { get; set; }
        
        [Required]
        [Range(1, 50)]
        [Display(Name = "Percentage")]
        public int Percentage { get; set; }
    }
}
