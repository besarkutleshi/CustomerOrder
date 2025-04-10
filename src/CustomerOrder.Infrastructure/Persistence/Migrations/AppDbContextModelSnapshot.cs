﻿// <auto-generated />
using System;
using CustomerOrder.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CustomerOrder.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CustomerOrder.Domain.Aggregates.Customer", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Customers", (string)null);
                });

            modelBuilder.Entity("CustomerOrder.Domain.Aggregates.Product", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Products", (string)null);
                });

            modelBuilder.Entity("CustomerOrder.Domain.Aggregates.Customer", b =>
                {
                    b.OwnsMany("CustomerOrder.Domain.Entities.Order", "Orders", b1 =>
                        {
                            b1.Property<int>("Id")
                                .HasColumnType("int");

                            b1.Property<int>("CustomerId")
                                .HasColumnType("int");

                            b1.Property<DateTime>("OrderDate")
                                .HasColumnType("datetime2");

                            b1.Property<string>("Status")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("nvarchar(20)");

                            b1.HasKey("Id", "CustomerId");

                            b1.HasIndex("CustomerId");

                            b1.ToTable("Orders", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");

                            b1.OwnsOne("CustomerOrder.Domain.ValueObjects.Money", "TotalPrice", b2 =>
                                {
                                    b2.Property<int>("OrderId")
                                        .HasColumnType("int");

                                    b2.Property<int>("OrderCustomerId")
                                        .HasColumnType("int");

                                    b2.Property<string>("Currency")
                                        .IsRequired()
                                        .HasMaxLength(20)
                                        .HasColumnType("nvarchar(20)")
                                        .HasColumnName("Currency");

                                    b2.Property<decimal>("Value")
                                        .HasColumnType("decimal(18,2)")
                                        .HasColumnName("TotalPrice");

                                    b2.HasKey("OrderId", "OrderCustomerId");

                                    b2.ToTable("Orders");

                                    b2.WithOwner()
                                        .HasForeignKey("OrderId", "OrderCustomerId");
                                });

                            b1.OwnsMany("CustomerOrder.Domain.ValueObjects.OrderItem", "OrderItems", b2 =>
                                {
                                    b2.Property<int>("OrderId")
                                        .HasColumnType("int");

                                    b2.Property<int>("OrderCustomerId")
                                        .HasColumnType("int");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("int");

                                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b2.Property<int>("Id"));

                                    b2.Property<int>("ProductId")
                                        .HasColumnType("int");

                                    b2.Property<int>("Quantity")
                                        .HasColumnType("int");

                                    b2.HasKey("OrderId", "OrderCustomerId", "Id");

                                    b2.ToTable("OrderItems", (string)null);

                                    b2.WithOwner()
                                        .HasForeignKey("OrderId", "OrderCustomerId");
                                });

                            b1.Navigation("OrderItems");

                            b1.Navigation("TotalPrice")
                                .IsRequired();
                        });

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("CustomerOrder.Domain.Aggregates.Product", b =>
                {
                    b.OwnsOne("CustomerOrder.Domain.ValueObjects.Money", "Price", b1 =>
                        {
                            b1.Property<int>("ProductId")
                                .HasColumnType("int");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("nvarchar(20)")
                                .HasColumnName("Currency");

                            b1.Property<decimal>("Value")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("Price");

                            b1.HasKey("ProductId");

                            b1.ToTable("Products");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.Navigation("Price")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
