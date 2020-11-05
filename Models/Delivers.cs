using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class Delivers
    {
        public Delivers()
        {
            DeliverDetails = new HashSet<DeliverDetails>();
        }

        [Key]
        [Column("id")]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Column("order_id")]
        [Display(Name = "Order ID")]
        public int OrderId { get; set; }
        [Column("employee_id")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }
        [Column("customer_id")]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        [Column("car_id")]
        [Display(Name = "Car")]
        public int CarId { get; set; }
        [Column("status_deliver_id")]
        [Display(Name = "Status")]
        public int StatusDeliverId { get; set; }

        [ForeignKey(nameof(CarId))]
        [InverseProperty(nameof(Cars.Delivers))]
        public virtual Cars Car { get; set; }
        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(Customers.Delivers))]
        public virtual Customers Customer { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        [InverseProperty(nameof(Employees.Delivers))]
        public virtual Employees Employee { get; set; }
        [ForeignKey(nameof(OrderId))]
        [InverseProperty(nameof(Orders.Delivers))]
        public virtual Orders Order { get; set; }
        [ForeignKey(nameof(StatusDeliverId))]
        [InverseProperty(nameof(DeliverStatus.Delivers))]
        public virtual DeliverStatus StatusDeliver { get; set; }
        [InverseProperty("Deliver")]
        public virtual ICollection<DeliverDetails> DeliverDetails { get; set; }
    }
}
