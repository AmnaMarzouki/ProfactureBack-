using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pro_FactureAPI.Migrations
{
    /// <inheritdoc />
    public partial class Profac : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abonnements",
                columns: table => new
                {
                    IdAbonnement = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Publish = table.Column<bool>(type: "bit", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Titre = table.Column<int>(type: "int", nullable: false),
                    Coût = table.Column<float>(type: "real", nullable: false),
                    Durée = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Actif = table.Column<bool>(type: "bit", nullable: false),
                    NbFichiers = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abonnements", x => x.IdAbonnement);
                });

            migrationBuilder.CreateTable(
                name: "Fichiers",
                columns: table => new
                {
                    IdFichier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomFichier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateImportation = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fichiers", x => x.IdFichier);
                });

            migrationBuilder.CreateTable(
                name: "Repertoires",
                columns: table => new
                {
                    IdRepertoire = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomRepertoire = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UtilisateurId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repertoires", x => x.IdRepertoire);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abonnements");

            migrationBuilder.DropTable(
                name: "Fichiers");

            migrationBuilder.DropTable(
                name: "Repertoires");
        }
    }
}
