﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication2;

#nullable disable

namespace WebApplication2.Migrations
{
    [DbContext(typeof(MineDbContext))]
    partial class MineDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebApplication2.ApplicationUser", b =>
                {
                    b.Property<int>("ApplicationUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ApplicationUserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ApplicationUserId");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("WebApplication2.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PostId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CommentId");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("WebApplication2.Device", b =>
                {
                    b.Property<int>("DeviceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DeviceId"));

                    b.Property<string>("Bssid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("CardMountFailed")
                        .HasColumnType("bit");

                    b.Property<long>("CardSize")
                        .HasColumnType("bigint");

                    b.Property<long>("CardType")
                        .HasColumnType("bigint");

                    b.Property<int>("Channel")
                        .HasColumnType("int");

                    b.Property<int>("DeviceMac")
                        .HasColumnType("int");

                    b.Property<long>("FreeSize")
                        .HasColumnType("bigint");

                    b.Property<string>("Ip")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mac")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("OpenError")
                        .HasColumnType("bit");

                    b.Property<string>("PubSubClientBufferStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rssi")
                        .HasColumnType("int");

                    b.Property<int>("SignalQuality")
                        .HasColumnType("int");

                    b.Property<string>("Ssid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("WriteError")
                        .HasColumnType("bit");

                    b.HasKey("DeviceId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("WebApplication2.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("WebApplication2.VibrationData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("AxAvg")
                        .HasColumnType("real");

                    b.Property<float>("AxDiff")
                        .HasColumnType("real");

                    b.Property<float>("AxMax")
                        .HasColumnType("real");

                    b.Property<float>("AxMin")
                        .HasColumnType("real");

                    b.Property<float>("AyAvg")
                        .HasColumnType("real");

                    b.Property<float>("AyDiff")
                        .HasColumnType("real");

                    b.Property<float>("AyMax")
                        .HasColumnType("real");

                    b.Property<float>("AyMin")
                        .HasColumnType("real");

                    b.Property<float>("AzAvg")
                        .HasColumnType("real");

                    b.Property<float>("AzDiff")
                        .HasColumnType("real");

                    b.Property<float>("AzMax")
                        .HasColumnType("real");

                    b.Property<float>("AzMin")
                        .HasColumnType("real");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeviceId")
                        .HasColumnType("int");

                    b.Property<float>("GxAvg")
                        .HasColumnType("real");

                    b.Property<float>("GxDiff")
                        .HasColumnType("real");

                    b.Property<float>("GxMax")
                        .HasColumnType("real");

                    b.Property<float>("GxMin")
                        .HasColumnType("real");

                    b.Property<float>("GyAvg")
                        .HasColumnType("real");

                    b.Property<float>("GyDiff")
                        .HasColumnType("real");

                    b.Property<float>("GyMax")
                        .HasColumnType("real");

                    b.Property<float>("GyMin")
                        .HasColumnType("real");

                    b.Property<float>("GzAvg")
                        .HasColumnType("real");

                    b.Property<float>("GzDiff")
                        .HasColumnType("real");

                    b.Property<float>("GzMax")
                        .HasColumnType("real");

                    b.Property<float>("GzMin")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.ToTable("VibrationDatas");
                });

            modelBuilder.Entity("WebApplication2.Comment", b =>
                {
                    b.HasOne("WebApplication2.Post", null)
                        .WithMany("Comments")
                        .HasForeignKey("PostId");
                });

            modelBuilder.Entity("WebApplication2.VibrationData", b =>
                {
                    b.HasOne("WebApplication2.Device", null)
                        .WithMany("ScannersData")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication2.Device", b =>
                {
                    b.Navigation("ScannersData");
                });

            modelBuilder.Entity("WebApplication2.Post", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
