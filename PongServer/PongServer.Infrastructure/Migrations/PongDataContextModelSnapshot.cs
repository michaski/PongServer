﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PongServer.Infrastructure.Data;

#nullable disable

namespace PongServer.Infrastructure.Migrations
{
    [DbContext(typeof(PongDataContext))]
    partial class PongDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("IdentityUser");
                });

            modelBuilder.Entity("PongServer.Domain.Entities.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("GameStartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("GuestPlayerId")
                        .HasColumnType("text");

                    b.Property<int>("GuestPlayerScore")
                        .HasColumnType("integer");

                    b.Property<Guid>("HostId")
                        .HasColumnType("uuid");

                    b.Property<string>("HostPlayerId")
                        .HasColumnType("text");

                    b.Property<int>("HostPlayerScore")
                        .HasColumnType("integer");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("GuestPlayerId");

                    b.HasIndex("HostId")
                        .IsUnique();

                    b.HasIndex("HostPlayerId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("PongServer.Domain.Entities.Host", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OwnerId")
                        .HasColumnType("text");

                    b.Property<int>("Port")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Hosts");
                });

            modelBuilder.Entity("PongServer.Domain.Entities.PlayerScore", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("GamesPlayed")
                        .HasColumnType("integer");

                    b.Property<int>("GamesWon")
                        .HasColumnType("integer");

                    b.Property<string>("PlayerId")
                        .HasColumnType("text");

                    b.Property<int>("RankingPosition")
                        .HasColumnType("integer");

                    b.Property<int>("RankingScore")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Scores");
                });

            modelBuilder.Entity("PongServer.Domain.Entities.Game", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "GuestPlayer")
                        .WithMany()
                        .HasForeignKey("GuestPlayerId");

                    b.HasOne("PongServer.Domain.Entities.Host", "Host")
                        .WithOne("HostedGame")
                        .HasForeignKey("PongServer.Domain.Entities.Game", "HostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "HostPlayer")
                        .WithMany()
                        .HasForeignKey("HostPlayerId");

                    b.Navigation("GuestPlayer");

                    b.Navigation("Host");

                    b.Navigation("HostPlayer");
                });

            modelBuilder.Entity("PongServer.Domain.Entities.Host", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("PongServer.Domain.Entities.PlayerScore", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("PongServer.Domain.Entities.Host", b =>
                {
                    b.Navigation("HostedGame");
                });
#pragma warning restore 612, 618
        }
    }
}
