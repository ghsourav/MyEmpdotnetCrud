﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyEmp.Context;

#nullable disable

namespace MyEmp.Migrations
{
    [DbContext(typeof(MyCoreEmpContext))]
    [Migration("20230413082106_mastermigration")]
    partial class mastermigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MyEmp.Models.CoreEmpTech", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("Employee_Id")
                        .HasColumnType("int");

                    b.Property<bool>("Is_active")
                        .HasColumnType("bit");

                    b.Property<int>("Tech_ID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("EmpTech", (string)null);
                });

            modelBuilder.Entity("MyEmp.Models.CoreEmployee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("DOJ")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StateId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Employee", (string)null);
                });

            modelBuilder.Entity("MyEmp.Models.CoreState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("StateName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("States", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            StateName = "West Bengal"
                        },
                        new
                        {
                            Id = 2,
                            StateName = "Odisha"
                        },
                        new
                        {
                            Id = 3,
                            StateName = "Karnataka"
                        },
                        new
                        {
                            Id = 4,
                            StateName = "Tamil Nadu"
                        },
                        new
                        {
                            Id = 5,
                            StateName = "Delhi"
                        });
                });

            modelBuilder.Entity("MyEmp.Models.CoreTechNames", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Tech_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Technames", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Tech_Name = "React"
                        },
                        new
                        {
                            Id = 2,
                            Tech_Name = "NodeJs"
                        },
                        new
                        {
                            Id = 3,
                            Tech_Name = ".Net"
                        },
                        new
                        {
                            Id = 4,
                            Tech_Name = "Angular"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
