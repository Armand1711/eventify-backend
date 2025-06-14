﻿// <auto-generated />
using System;
using EventifyBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EventifyBackend.Migrations
{
    [DbContext(typeof(EventifyDbContext))]
    [Migration("20250609162945_AddUserIdToEventTasks")]
    partial class AddUserIdToEventTasks
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("ProductVersion", "8.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EventifyBackend.Models.Archive", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdAt");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("EventId")
                        .HasColumnType("integer")
                        .HasColumnName("eventId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updatedAt");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("userId");

                    b.HasKey("Id");

                    b.ToTable("Archives", "public");
                });

            modelBuilder.Entity("EventifyBackend.Models.Budget", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<float>("Amount")
                        .HasColumnType("real")
                        .HasColumnName("amount");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("category");

                    b.Property<int>("TaskId")
                        .HasColumnType("integer")
                        .HasColumnName("taskId");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("userId");

                    b.HasKey("Id");

                    b.ToTable("Budgets", "public");
                });

            modelBuilder.Entity("EventifyBackend.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Archived")
                        .HasColumnType("boolean")
                        .HasColumnName("archived");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdAt");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updatedAt");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("userId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Events", "public");
                });

            modelBuilder.Entity("EventifyBackend.Models.EventRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("RequesterEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RequesterName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("EventRequests", "public");
                });

            modelBuilder.Entity("EventifyBackend.Models.EventTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Archived")
                        .HasColumnType("boolean")
                        .HasColumnName("archived");

                    b.Property<string>("Budget")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("budget");

                    b.Property<bool>("Completed")
                        .HasColumnType("boolean")
                        .HasColumnName("completed");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdAt");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("dueDate");

                    b.Property<int>("EventId")
                        .HasColumnType("integer")
                        .HasColumnName("eventId");

                    b.Property<string>("Priority")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("priority");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updatedAt");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("userId");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("EventTasks", "public");
                });

            modelBuilder.Entity("EventifyBackend.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdAt");

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updatedAt");

                    b.HasKey("Id");

                    b.ToTable("Users", "public");
                });

            modelBuilder.Entity("EventifyBackend.Models.Event", b =>
                {
                    b.HasOne("EventifyBackend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EventifyBackend.Models.EventTask", b =>
                {
                    b.HasOne("EventifyBackend.Models.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventifyBackend.Models.User", "AssignedUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AssignedUser");

                    b.Navigation("Event");
                });
#pragma warning restore 612, 618
        }
    }
}
