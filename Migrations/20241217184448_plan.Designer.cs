﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuadraFacil_backend.API.Data;

#nullable disable

namespace QuadraFacil_backend.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241217184448_plan")]
    partial class plan
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("QuadraFacil_backend.Models.Arena.AdressArena", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ArenaId")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Neighborhood")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("Number")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("ArenaId");

                    b.ToTable("AdressArenas");
                });

            modelBuilder.Entity("QuadraFacil_backend.Models.Arena.ArenaModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ValueHour")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Arenas");
                });

            modelBuilder.Entity("QuadraFacil_backend.Models.Arena.Space.SpaceModel", b =>
                {
                    b.Property<int>("SpaceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SpaceId"));

                    b.Property<int>("ArenaId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SpaceId");

                    b.HasIndex("ArenaId");

                    b.ToTable("Spaces");
                });

            modelBuilder.Entity("QuadraFacil_backend.Models.Reserve.ReserveModel", b =>
                {
                    b.Property<int>("Id_reserve")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_reserve"));

                    b.Property<int>("ArenaId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DataReserve")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("Observation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SpaceId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan?>("TimeFinal")
                        .IsRequired()
                        .HasColumnType("time");

                    b.Property<TimeSpan?>("TimeInitial")
                        .IsRequired()
                        .HasColumnType("time");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id_reserve");

                    b.ToTable("Reserve");
                });

            modelBuilder.Entity("QuadraFacil_backend.Models.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ArenaId")
                        .HasMaxLength(10)
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("QuadraFacil_backend.Models.Arena.AdressArena", b =>
                {
                    b.HasOne("QuadraFacil_backend.Models.Arena.ArenaModel", "Arena")
                        .WithMany("AdressArenas")
                        .HasForeignKey("ArenaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Arena");
                });

            modelBuilder.Entity("QuadraFacil_backend.Models.Arena.Space.SpaceModel", b =>
                {
                    b.HasOne("QuadraFacil_backend.Models.Arena.ArenaModel", "Arena")
                        .WithMany("Spaces")
                        .HasForeignKey("ArenaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Arena");
                });

            modelBuilder.Entity("QuadraFacil_backend.Models.Arena.ArenaModel", b =>
                {
                    b.Navigation("AdressArenas");

                    b.Navigation("Spaces");
                });
#pragma warning restore 612, 618
        }
    }
}
