using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MobiSys.Models
{
    public partial class MobiSysContext : IdentityDbContext
    {
        public MobiSysContext()
        {
        }

        public MobiSysContext(DbContextOptions<MobiSysContext> options)
            : base(options)
        {
        }

        
        public virtual DbSet<Cars> Cars { get; set; }
        public virtual DbSet<CartDetails> CartDetails { get; set; }
        public virtual DbSet<Carts> Carts { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<DeliverDetails> DeliverDetails { get; set; }
        public virtual DbSet<DeliverStatus> DeliverStatus { get; set; }
        public virtual DbSet<Delivers> Delivers { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Manufacturers> Manufacturers { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<ProductRepricing> ProductRepricing { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Promotions> Promotions { get; set; }
        public virtual DbSet<ReturnDetails> ReturnDetails { get; set; }
        public virtual DbSet<ReturnStatus> ReturnStatus { get; set; }
        public virtual DbSet<Returns> Returns { get; set; }
        public virtual DbSet<Type> Type { get; set; }
        public virtual DbSet<Um> Um { get; set; }
        public virtual DbSet<Vauchers> Vauchers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
          
            modelBuilder.Entity<Cars>(entity =>
            {
                entity.Property(e => e.RegistrationPlate).IsFixedLength();
            });

            modelBuilder.Entity<CartDetails>(entity =>
            {
                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.CartDetails)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartDetails_Carts");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CartDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartDetails_Products");
            });

            modelBuilder.Entity<Carts>(entity =>
            {
                entity.HasOne(d => d.Cutomer)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.CutomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Carts_Customers");
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.Property(e => e.Name).IsFixedLength();
            });

            modelBuilder.Entity<Cities>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_cities")
                    .IsUnique();

                entity.Property(e => e.Name).IsFixedLength();
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasIndex(e => e.Cui)
                    .HasName("IX_Customers")
                    .IsUnique();

                entity.Property(e => e.Adress).IsFixedLength();

                entity.Property(e => e.Bank).IsFixedLength();

                entity.Property(e => e.Company).IsFixedLength();

                entity.Property(e => e.ContactPerson).IsFixedLength();

                entity.Property(e => e.Cui).IsFixedLength();

                entity.Property(e => e.Iban).IsFixedLength();

                entity.Property(e => e.Mail).IsFixedLength();

                entity.Property(e => e.PhoneNumber).IsFixedLength();

                entity.Property(e => e.RegistrationNumber).IsFixedLength();

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customers_Cities");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customers_Type");
            });

            modelBuilder.Entity<DeliverDetails>(entity =>
            {
                entity.HasOne(d => d.Deliver)
                    .WithMany(p => p.DeliverDetails)
                    .HasForeignKey(d => d.DeliverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeliverDetails_Delivers");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.DeliverDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeliverDetails_Products");
            });

            modelBuilder.Entity<DeliverStatus>(entity =>
            {
                entity.Property(e => e.Status).IsFixedLength();
            });

            modelBuilder.Entity<Delivers>(entity =>
            {
                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Delivers)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Delivers_Cars");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Delivers)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Delivers_Customers");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Delivers)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Delivers_Employees");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Delivers)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Delivers_Orders");

                entity.HasOne(d => d.StatusDeliver)
                    .WithMany(p => p.Delivers)
                    .HasForeignKey(d => d.StatusDeliverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Delivers_DeliverStatus");
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.Property(e => e.Adress).IsFixedLength();

                entity.Property(e => e.FullName).IsFixedLength();

                //entity.Property(e => e.email).IsFixedLength();

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employees_Cities");
            });

            modelBuilder.Entity<Manufacturers>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_Manufacturers")
                    .IsUnique();

                entity.Property(e => e.Name).IsFixedLength();
            });

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Products");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.Property(e => e.Status).IsFixedLength();
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Customers");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Orders_Employees");

                entity.HasOne(d => d.OrderStatus)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OrderStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_OrderStatus");
            });

            modelBuilder.Entity<ProductRepricing>(entity =>
            {
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductRepricing)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductRepricing_Products");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasIndex(e => e.ProductCode)
                    .HasName("IX_Products")
                    .IsUnique();

                entity.Property(e => e.Details).IsFixedLength();

                entity.Property(e => e.Image).IsFixedLength();

                entity.Property(e => e.Name).IsFixedLength();

                entity.Property(e => e.ProductCode).IsFixedLength();

                entity.HasOne(d => d.Cateogry)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CateogryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products_category");

                entity.HasOne(d => d.Manufacturer)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ManufacturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products_manufacturer");

                entity.HasOne(d => d.Um)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.UmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_products_um");
            });

            modelBuilder.Entity<Promotions>(entity =>
            {
                entity.Property(e => e.Details).IsFixedLength();
            });

            modelBuilder.Entity<ReturnDetails>(entity =>
            {
                entity.Property(e => e.Reason).IsFixedLength();

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ReturnDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReturnDetails_Products");

                entity.HasOne(d => d.Return)
                    .WithMany(p => p.ReturnDetails)
                    .HasForeignKey(d => d.ReturnId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReturnDetails_Returns");
            });

            modelBuilder.Entity<ReturnStatus>(entity =>
            {
                entity.Property(e => e.Status).IsFixedLength();
            });

            modelBuilder.Entity<Returns>(entity =>
            {
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Returns)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Returns_Customers");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Returns)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Returns_Employees");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Returns)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Returns_Orders");

                entity.HasOne(d => d.ReturnStatus)
                    .WithMany(p => p.Returns)
                    .HasForeignKey(d => d.ReturnStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Returns_ReturnStatus");
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_Type")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).IsFixedLength();
            });

            modelBuilder.Entity<Um>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_um")
                    .IsUnique();

                entity.Property(e => e.Name).IsFixedLength();
            });

            modelBuilder.Entity<Vauchers>(entity =>
            {
                entity.HasIndex(e => e.Code)
                    .HasName("IX_Vauchers")
                    .IsUnique();

                entity.Property(e => e.Code).IsFixedLength();

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Vauchers)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vauchers_Employees");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Vauchers)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vauchers_Customers");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Vauchers)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Vauchers_Orders");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
