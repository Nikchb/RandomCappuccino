﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RandomCappuccino.Server.Data;

namespace RandomCappuccino.Server.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    partial class DataBaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("RandomCappuccino.Server.Data.Models.Participant", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("RandomCappuccino.Server.Data.Models.Tour", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Tours");
                });

            modelBuilder.Entity("RandomCappuccino.Server.Data.Models.TourPair", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Participant1Id")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Participant2Id")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TourId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Participant1Id");

                    b.HasIndex("Participant2Id");

                    b.HasIndex("TourId");

                    b.ToTable("TourPairs");
                });

            modelBuilder.Entity("RandomCappuccino.Server.Data.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RandomCappuccino.Server.Data.Models.UserRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .HasColumnType("text");

                    b.HasKey("UserId", "Role");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("RandomCappuccino.Server.Data.Models.Participant", b =>
                {
                    b.HasOne("RandomCappuccino.Server.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RandomCappuccino.Server.Data.Models.TourPair", b =>
                {
                    b.HasOne("RandomCappuccino.Server.Data.Models.Participant", "Participant1")
                        .WithMany()
                        .HasForeignKey("Participant1Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RandomCappuccino.Server.Data.Models.Participant", "Participant2")
                        .WithMany()
                        .HasForeignKey("Participant2Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RandomCappuccino.Server.Data.Models.Participant", "Tour")
                        .WithMany()
                        .HasForeignKey("TourId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Participant1");

                    b.Navigation("Participant2");

                    b.Navigation("Tour");
                });

            modelBuilder.Entity("RandomCappuccino.Server.Data.Models.UserRole", b =>
                {
                    b.HasOne("RandomCappuccino.Server.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
