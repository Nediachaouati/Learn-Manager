using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnManager.Migrations
{
    /// <inheritdoc />
    public partial class AddDepenseMode2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Depenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Montant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormateurId = table.Column<int>(type: "int", nullable: true),
                    FormationId = table.Column<int>(type: "int", nullable: true),
                    DateDepense = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Depenses_Formations_FormationId",
                        column: x => x.FormationId,
                        principalTable: "Formations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Depenses_Users_FormateurId",
                        column: x => x.FormateurId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Depenses_FormateurId",
                table: "Depenses",
                column: "FormateurId");

            migrationBuilder.CreateIndex(
                name: "IX_Depenses_FormationId",
                table: "Depenses",
                column: "FormationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Depenses");
        }
    }
}
