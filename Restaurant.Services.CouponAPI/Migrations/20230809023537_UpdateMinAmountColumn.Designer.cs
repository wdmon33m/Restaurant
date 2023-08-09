﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Restaurant.Services.CouponAPI.Data;

#nullable disable

namespace Restaurant.Services.CouponAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230809023537_UpdateMinAmountColumn")]
    partial class UpdateMinAmountColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Restaurant.Services.CouponAPI.Models.Coupon", b =>
                {
                    b.Property<int>("CouponID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CouponID"));

                    b.Property<string>("CouponCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("DiscountAmount")
                        .HasColumnType("float");

                    b.Property<double>("MinAmount")
                        .HasColumnType("float");

                    b.HasKey("CouponID");

                    b.ToTable("Coupons");

                    b.HasData(
                        new
                        {
                            CouponID = 1,
                            CouponCode = "10OFF",
                            DiscountAmount = 10.0,
                            MinAmount = 20.0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
