using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class Returns
    {
        public Returns()
        {
            ReturnDetails = new HashSet<ReturnDetails>();
        }

        [Key]
        [Column("id")]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Column("customer_id")]
        [Display(Name = "Customer ID")]
        public int CustomerId { get; set; }
        [Column("order_id")]
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }
        [Column("employee_id")]
        [Display(Name = "Employee ID")]
        public int? EmployeeId { get; set; }
        [Column("return_status_id")]
        [Display(Name = "Status")]
        public int ReturnStatusId { get; set; }
        [Column("total_price")]
        [Range(0, 100000)]
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(Customers.Returns))]
        public virtual Customers Customer { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        [InverseProperty(nameof(Employees.Returns))]
        public virtual Employees Employee { get; set; }
        [ForeignKey(nameof(OrderId))]
        [InverseProperty(nameof(Orders.Returns))]
        public virtual Orders Order { get; set; }
        [ForeignKey(nameof(ReturnStatusId))]
        [InverseProperty("Returns")]
        public virtual ReturnStatus ReturnStatus { get; set; }
        [InverseProperty("Return")]
        public virtual ICollection<ReturnDetails> ReturnDetails { get; set; }
    }
}
