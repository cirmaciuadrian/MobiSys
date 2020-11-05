using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MobiSys.Areas.Admin.ViewModels
{
    public class EditRoleViewModel
    {

        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        
        
        public string Id { get; set; }
        [Display(Name = "Role")]

        [Required(ErrorMessage ="Role name is required")]
        public String RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
