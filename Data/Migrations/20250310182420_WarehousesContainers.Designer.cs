﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Warehouse.Data;

#nullable disable

namespace Warehouse.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250310182420_WarehousesContainers")]
    partial class WarehousesContainers
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WarehouseApi.Entities.Container", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Category")
                        .HasColumnType("integer");

                    b.Property<double>("MaxWeight")
                        .HasColumnType("double precision");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<int>("WarehouseId")
                        .HasColumnType("integer");

                    b.Property<int>("WarehousesId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("WarehousesId");

                    b.ToTable("Containers");
                });

            modelBuilder.Entity("WarehouseApi.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("ContainerId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Weight")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("ContainerId");

                    b.ToTable("Products", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("WarehouseApi.Entities.Warehouses", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("WarehouseApi.Entities.Animal", b =>
                {
                    b.HasBaseType("WarehouseApi.Entities.Product");

                    b.Property<string>("PassId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.ToTable("Animals", (string)null);
                });

            modelBuilder.Entity("WarehouseApi.Entities.Clothes", b =>
                {
                    b.HasBaseType("WarehouseApi.Entities.Product");

                    b.Property<string>("Size")
                        .IsRequired()
                        .HasColumnType("text");

                    b.ToTable("Clothes", (string)null);
                });

            modelBuilder.Entity("WarehouseApi.Entities.Food", b =>
                {
                    b.HasBaseType("WarehouseApi.Entities.Product");

                    b.Property<DateTime>("ExpiredData")
                        .HasColumnType("timestamp with time zone");

                    b.ToTable("Food", (string)null);
                });

            modelBuilder.Entity("WarehouseApi.Entities.Container", b =>
                {
                    b.HasOne("WarehouseApi.Entities.Warehouses", "Warehouses")
                        .WithMany("Containers")
                        .HasForeignKey("WarehousesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Warehouses");
                });

            modelBuilder.Entity("WarehouseApi.Entities.Product", b =>
                {
                    b.HasOne("WarehouseApi.Entities.Container", "Container")
                        .WithMany("Products")
                        .HasForeignKey("ContainerId");

                    b.Navigation("Container");
                });

            modelBuilder.Entity("WarehouseApi.Entities.Animal", b =>
                {
                    b.HasOne("WarehouseApi.Entities.Product", null)
                        .WithOne()
                        .HasForeignKey("WarehouseApi.Entities.Animal", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WarehouseApi.Entities.Clothes", b =>
                {
                    b.HasOne("WarehouseApi.Entities.Product", null)
                        .WithOne()
                        .HasForeignKey("WarehouseApi.Entities.Clothes", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WarehouseApi.Entities.Food", b =>
                {
                    b.HasOne("WarehouseApi.Entities.Product", null)
                        .WithOne()
                        .HasForeignKey("WarehouseApi.Entities.Food", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WarehouseApi.Entities.Container", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("WarehouseApi.Entities.Warehouses", b =>
                {
                    b.Navigation("Containers");
                });
#pragma warning restore 612, 618
        }
    }
}
