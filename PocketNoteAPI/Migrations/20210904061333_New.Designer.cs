﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PocketNoteAPI.Models;

namespace PocketNoteAPI.Migrations
{
    [DbContext(typeof(PocketNoteAPIContext))]
    [Migration("20210904061333_New")]
    partial class New
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PocketNoteAPI.Models.Device", b =>
                {
                    b.Property<string>("DeviceId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("GoogleUserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DeviceId");

                    b.HasIndex("GoogleUserId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("PocketNoteAPI.Models.File", b =>
                {
                    b.Property<string>("FileId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EncryptedName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GoogleUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Signature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UploadDate")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FileId");

                    b.HasIndex("GoogleUserId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("PocketNoteAPI.Models.PocketNoteAPIItem", b =>
                {
                    b.Property<string>("GoogleUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AuthToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("Pin")
                        .HasColumnType("smallint");

                    b.Property<string>("PrivateKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PublicKey")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GoogleUserId");

                    b.ToTable("PocketNoteAPIItems");
                });

            modelBuilder.Entity("PocketNoteAPI.Models.Session", b =>
                {
                    b.Property<string>("DeviceId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("GoogleUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AuthDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ip")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DeviceId", "GoogleUserId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("PocketNoteAPI.Models.Device", b =>
                {
                    b.HasOne("PocketNoteAPI.Models.PocketNoteAPIItem", "PocketNoteAPIItem")
                        .WithMany("Devices")
                        .HasForeignKey("GoogleUserId");

                    b.Navigation("PocketNoteAPIItem");
                });

            modelBuilder.Entity("PocketNoteAPI.Models.File", b =>
                {
                    b.HasOne("PocketNoteAPI.Models.PocketNoteAPIItem", "PocketNoteAPIItem")
                        .WithMany("Files")
                        .HasForeignKey("GoogleUserId");

                    b.Navigation("PocketNoteAPIItem");
                });

            modelBuilder.Entity("PocketNoteAPI.Models.Session", b =>
                {
                    b.HasOne("PocketNoteAPI.Models.Device", "Device")
                        .WithMany("Sessions")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");
                });

            modelBuilder.Entity("PocketNoteAPI.Models.Device", b =>
                {
                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("PocketNoteAPI.Models.PocketNoteAPIItem", b =>
                {
                    b.Navigation("Devices");

                    b.Navigation("Files");
                });
#pragma warning restore 612, 618
        }
    }
}
