using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinatTestTechnique2.Migrations
{
    public partial class AddCandidatModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Candidats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NiveauEtude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnneesExperience = table.Column<int>(type: "int", nullable: false),
                    DernierEmployeur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CV = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidats", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candidats");
        }
    }
}
