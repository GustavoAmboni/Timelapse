﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Timelapse.CLI.Infraestructure.Data.Context;

#nullable disable

namespace Timelapse.CLI.Infraestructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("Timelapse.CLI.Entities.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Anchor")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ItemId");

                    b.ToTable("Items", (string)null);
                });

            modelBuilder.Entity("Timelapse.CLI.Entities.Period", b =>
                {
                    b.Property<int>("PeriodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Commentary")
                        .HasColumnType("TEXT");

                    b.Property<int>("ItemId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("StoppedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("PeriodId");

                    b.HasIndex("ItemId");

                    b.ToTable("Periods");
                });

            modelBuilder.Entity("Timelapse.CLI.Entities.Period", b =>
                {
                    b.HasOne("Timelapse.CLI.Entities.Item", "Item")
                        .WithMany("Periods")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Timelapse.CLI.Entities.Item", b =>
                {
                    b.Navigation("Periods");
                });
#pragma warning restore 612, 618
        }
    }
}
