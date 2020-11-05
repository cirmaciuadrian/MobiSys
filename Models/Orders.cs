using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobiSys.Models
{
    public partial class Orders
    {
        public Orders()
        {
            Delivers = new HashSet<Delivers>();
            OrderDetails = new HashSet<OrderDetails>();
            Returns = new HashSet<Returns>();
            Vauchers = new HashSet<Vauchers>();
        }

        [Key]
        [Column("id")]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Column("customer_id")]
        [Display(Name = "Customer ID")]
        public int? CustomerId { get; set; }
        [Column("employee_id")]
        [Display(Name = "Employee ID")]
        public int? EmployeeId { get; set; }
        [Column("date", TypeName = "datetime")]
        [Display(Name = "Date & Hour")]
        public DateTime Date { get; set; }
        [Column("discount_val")]
        [Display(Name = "Discount value")]
        [Range(0, 100000)]
        public decimal? DiscountVal { get; set; }
        [Column("total_price")]
        [Display(Name = "Total Price")]
        [Range(0, 100000)]
        public decimal TotalPrice { get; set; }
        [Column("order_status_id")]
        [Display(Name = "Status")]
        public int OrderStatusId { get; set; }
        [Column("shipping_date", TypeName = "date")]
        [DataType(DataType.Date)]
        [Display(Name = "Shipping Date")]
        public DateTime? ShippingDate { get; set; }
        [Column("isPaid")]
        [Display(Name = "Paid")]
        public bool isPaid { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(Customers.Orders))]
        public virtual Customers Customer { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        [InverseProperty(nameof(Employees.Orders))]
        public virtual Employees Employee { get; set; }
        [ForeignKey(nameof(OrderStatusId))]
        [InverseProperty("Orders")]
        public virtual OrderStatus OrderStatus { get; set; }
        [InverseProperty("Order")]
        public virtual ICollection<Delivers> Delivers { get; set; }
        [InverseProperty("Order")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        [InverseProperty("Order")]
        public virtual ICollection<Returns> Returns { get; set; }
        [InverseProperty("Order")]
        public virtual ICollection<Vauchers> Vauchers { get; set; }
    }
}
