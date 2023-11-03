﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Proyecto1_MZ_MJ.Data;

#nullable disable

namespace Proyecto1_MZ_MJ.Migrations
{
    [DbContext(typeof(Proyecto1_MZ_MJContext))]
    partial class Proyecto1_MZ_MJContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Proyecto1_MZ_MJ.Models.Comentario", b =>
                {
                    b.Property<int>("ComentarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ComentarioId"), 1L, 1);

                    b.Property<int>("HabitacionId")
                        .HasColumnType("int");

                    b.Property<string>("Texto")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ComentarioId");

                    b.HasIndex("HabitacionId");

                    b.ToTable("Comentario");
                });

            modelBuilder.Entity("Proyecto1_MZ_MJ.Models.Habitacion", b =>
                {
                    b.Property<int>("HabitacionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HabitacionId"), 1L, 1);

                    b.Property<int>("Capacidad")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Disponible")
                        .HasColumnType("bit");

                    b.Property<double>("NumHabitacion")
                        .HasColumnType("float");

                    b.Property<double>("PrecioPorNoche")
                        .HasColumnType("float");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Vistas")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("HabitacionId");

                    b.ToTable("Habitacion");
                });

            modelBuilder.Entity("Proyecto1_MZ_MJ.Models.Pago", b =>
                {
                    b.Property<int>("PagoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PagoId"), 1L, 1);

                    b.Property<DateTime>("FechaPago")
                        .HasColumnType("datetime2");

                    b.Property<int>("HabitacionId")
                        .HasColumnType("int");

                    b.Property<string>("MetodoPago")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("MontoPagado")
                        .HasColumnType("float");

                    b.Property<bool>("Pagado")
                        .HasColumnType("bit");

                    b.HasKey("PagoId");

                    b.HasIndex("HabitacionId");

                    b.ToTable("Pago");
                });

            modelBuilder.Entity("Proyecto1_MZ_MJ.Models.Comentario", b =>
                {
                    b.HasOne("Proyecto1_MZ_MJ.Models.Habitacion", "Habitacion")
                        .WithMany("Comentarios")
                        .HasForeignKey("HabitacionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Habitacion");
                });

            modelBuilder.Entity("Proyecto1_MZ_MJ.Models.Pago", b =>
                {
                    b.HasOne("Proyecto1_MZ_MJ.Models.Habitacion", "Habitacion")
                        .WithMany()
                        .HasForeignKey("HabitacionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Habitacion");
                });

            modelBuilder.Entity("Proyecto1_MZ_MJ.Models.Habitacion", b =>
                {
                    b.Navigation("Comentarios");
                });
#pragma warning restore 612, 618
        }
    }
}
