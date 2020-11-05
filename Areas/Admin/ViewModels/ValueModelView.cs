using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MobiSys.Areas.Admin.ViewModels
{
    public partial class ValueModelView
    {
        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Required]
        [Range (1, 99999)]
        [Display(Name = "Value")]
        public int Value { get; set; }
    }
}
