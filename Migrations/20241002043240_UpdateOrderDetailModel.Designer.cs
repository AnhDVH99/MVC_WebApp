﻿// <auto-generated />
using System;
using ASP.NET_Core_MVC_Piacom.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ASP.NET_Core_MVC_Piacom.Migrations
{
    [DbContext(typeof(PiacomDbContext))]
    [Migration("20241002043240_UpdateOrderDetailModel")]
    partial class UpdateOrderDetailModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.CreditLimit", b =>
                {
                    b.Property<Guid>("CreditLimitID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreditType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CustomerID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FromDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OverDue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Total")
                        .IsRequired()
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("CreditLimitID");

                    b.HasIndex("CustomerID");

                    b.ToTable("CreditLimits");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.Customer", b =>
                {
                    b.Property<Guid>("CustomerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AddressLine1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddressLine2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("EmployeeID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SaleRepEmployeeID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerID");

                    b.HasIndex("EmployeeID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.Employee", b =>
                {
                    b.Property<Guid>("EmployeeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EmployeeID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.Order", b =>
                {
                    b.Property<Guid>("OrderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CustomerID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EmployeeID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OrderDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrderStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequiredDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShippedDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SysD")
                        .HasColumnType("datetime2");

                    b.Property<string>("SysU")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrderID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.OrderDetail", b =>
                {
                    b.Property<Guid>("OrderDetailID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Discount")
                        .HasColumnType("int");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("EnvironmentTax")
                        .HasColumnType("real");

                    b.Property<Guid>("OrderID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<Guid>("PriceDetailID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("UnitID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("VAT")
                        .HasColumnType("real");

                    b.Property<float>("priceAfterTax")
                        .HasColumnType("real");

                    b.Property<float>("priceBeforeTax")
                        .HasColumnType("real");

                    b.HasKey("OrderDetailID");

                    b.HasIndex("OrderID");

                    b.HasIndex("UnitID");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.Payment", b =>
                {
                    b.Property<Guid>("PaymentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("Amount")
                        .HasColumnType("real");

                    b.Property<Guid>("CustomerID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Discount")
                        .HasColumnType("int");

                    b.Property<string>("PaymentCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.Price", b =>
                {
                    b.Property<Guid>("PriceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PriceCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SysD")
                        .HasColumnType("datetime2");

                    b.Property<string>("SysU")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ToDate")
                        .HasColumnType("datetime2");

                    b.HasKey("PriceID");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.PriceDetail", b =>
                {
                    b.Property<Guid>("PriceDetailID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("EnvirontmentTax")
                        .HasColumnType("real");

                    b.Property<Guid>("PriceID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UnitID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("VAT")
                        .HasColumnType("int");

                    b.HasKey("PriceDetailID");

                    b.HasIndex("PriceID");

                    b.ToTable("PriceDetails");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.Product", b =>
                {
                    b.Property<Guid>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OrderDetailID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SysD")
                        .HasColumnType("datetime2");

                    b.Property<string>("SysU")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductID");

                    b.HasIndex("OrderDetailID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.Unit", b =>
                {
                    b.Property<Guid>("UnitID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UnitCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UnitName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UnitID");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.User", b =>
                {
                    b.Property<Guid>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EmployeeID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.HasIndex("EmployeeID")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.CreditLimit", b =>
                {
                    b.HasOne("ASP.NET_Core_MVC_Piacom.Models.Domain.Customer", "Customer")
                        .WithMany("CreditLimits")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.Customer", b =>
                {
                    b.HasOne("ASP.NET_Core_MVC_Piacom.Models.Domain.Employee", null)
                        .WithMany("Customers")
                        .HasForeignKey("EmployeeID");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.Order", b =>
                {
                    b.HasOne("ASP.NET_Core_MVC_Piacom.Models.Domain.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.OrderDetail", b =>
                {
                    b.HasOne("ASP.NET_Core_MVC_Piacom.Models.Domain.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ASP.NET_Core_MVC_Piacom.Models.Domain.Unit", "Unit")
                        .WithMany()
                        .HasForeignKey("UnitID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.Payment", b =>
                {
                    b.HasOne("ASP.NET_Core_MVC_Piacom.Models.Domain.Customer", "Customer")
                        .WithMany("Payments")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.PriceDetail", b =>
                {
                    b.HasOne("ASP.NET_Core_MVC_Piacom.Models.Domain.Price", "Price")
                        .WithMany("PriceDetails")
                        .HasForeignKey("PriceID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Price");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.Product", b =>
                {
                    b.HasOne("ASP.NET_Core_MVC_Piacom.Models.Domain.OrderDetail", null)
                        .WithMany("Products")
                        .HasForeignKey("OrderDetailID");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.User", b =>
                {
                    b.HasOne("ASP.NET_Core_MVC_Piacom.Models.Domain.Employee", "Employee")
                        .WithOne("User")
                        .HasForeignKey("ASP.NET_Core_MVC_Piacom.Models.Domain.User", "EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.Customer", b =>
                {
                    b.Navigation("CreditLimits");

                    b.Navigation("Orders");

                    b.Navigation("Payments");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.Employee", b =>
                {
                    b.Navigation("Customers");

                    b.Navigation("User")
                        .IsRequired();
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.OrderDetail", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("ASP.NET_Core_MVC_Piacom.Models.Domain.Price", b =>
                {
                    b.Navigation("PriceDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
