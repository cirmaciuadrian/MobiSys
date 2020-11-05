using MobiSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MobiSys.ViewModels
{
    public partial class ProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    
}



