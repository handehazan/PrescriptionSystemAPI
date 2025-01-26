﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using prescriptionSystemApi.context;

#nullable disable

namespace prescriptionSystemApi.Migrations
{
    [DbContext(typeof(SqlDbContext))]
    [Migration("20250123180854_UpdatePrescriptionMedicineModel")]
    partial class UpdatePrescriptionMedicineModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("prescriptionSystemApi.model.Prescription", b =>
                {
                    b.Property<int>("PrescriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PrescriptionId"));

                    b.Property<string>("DoctorId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsSubmitted")
                        .HasColumnType("bit");

                    b.Property<string>("PatientTC")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<DateTime>("VisitDate")
                        .HasColumnType("datetime2");

                    b.HasKey("PrescriptionId");

                    b.ToTable("Prescription");
                });

            modelBuilder.Entity("prescriptionSystemApi.source.db.PrescriptionMedicines", b =>
                {
                    b.Property<int>("PrescriptionMedicineID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PrescriptionMedicineID"));

                    b.Property<string>("MedicineName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("PrescriptionID")
                        .HasColumnType("int");

                    b.HasKey("PrescriptionMedicineID");

                    b.HasIndex("PrescriptionID");

                    b.ToTable("PrescriptionMedicines");
                });

            modelBuilder.Entity("prescriptionSystemApi.source.db.PrescriptionMedicines", b =>
                {
                    b.HasOne("prescriptionSystemApi.model.Prescription", "Prescription")
                        .WithMany()
                        .HasForeignKey("PrescriptionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Prescription");
                });
#pragma warning restore 612, 618
        }
    }
}
