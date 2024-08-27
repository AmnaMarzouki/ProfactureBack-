﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pro_FactureAPI.Data;

#nullable disable

namespace Pro_FactureAPI.Migrations
{
    [DbContext(typeof(ProfactureDb))]
    [Migration("20240825213250_amna")]
    partial class amna
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Pro_FactureAPI.Models.Abonnement", b =>
                {
                    b.Property<Guid>("IdAbonnement")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Actif")
                        .HasColumnType("bit");

                    b.Property<float>("Coût")
                        .HasColumnType("real");

                    b.Property<DateTime>("DateCreation")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateFin")
                        .HasColumnType("datetime2");

                    b.Property<string>("Durée")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NbFichiers")
                        .HasColumnType("int");

                    b.Property<bool>("Publish")
                        .HasColumnType("bit");

                    b.Property<int>("Titre")
                        .HasColumnType("int");

                    b.HasKey("IdAbonnement");

                    b.ToTable("Abonnements");
                });

            modelBuilder.Entity("Pro_FactureAPI.Models.Contact", b =>
                {
                    b.Property<Guid>("IdContact")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sujet")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdContact");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("Pro_FactureAPI.Models.Fichier", b =>
                {
                    b.Property<Guid>("IdFichier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateImportation")
                        .HasColumnType("datetime2");

                    b.Property<string>("NomFichier")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RepertoireFk")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdFichier");

                    b.HasIndex("RepertoireFk");

                    b.ToTable("Fichiers");
                });

            modelBuilder.Entity("Pro_FactureAPI.Models.Repertoire", b =>
                {
                    b.Property<Guid>("IdRepertoire")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreation")
                        .HasColumnType("datetime2");

                    b.Property<string>("NomRepertoire")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UtilisateurId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("IdRepertoire");

                    b.ToTable("Repertoires");
                });

            modelBuilder.Entity("Pro_FactureAPI.Models.Fichier", b =>
                {
                    b.HasOne("Pro_FactureAPI.Models.Repertoire", "Repertoire")
                        .WithMany("Fichiers")
                        .HasForeignKey("RepertoireFk")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Repertoire");
                });

            modelBuilder.Entity("Pro_FactureAPI.Models.Repertoire", b =>
                {
                    b.Navigation("Fichiers");
                });
#pragma warning restore 612, 618
        }
    }
}
