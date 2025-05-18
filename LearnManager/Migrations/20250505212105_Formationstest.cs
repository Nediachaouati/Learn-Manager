using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnManager.Migrations
{
    /// <inheritdoc />
    public partial class Formationstest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Formations_Users_FormateurId",
                table: "Formations");

            migrationBuilder.AlterColumn<int>(
                name: "FormateurId",
                table: "Formations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Formations_Users_FormateurId",
                table: "Formations",
                column: "FormateurId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Formations_Users_FormateurId",
                table: "Formations");

            migrationBuilder.AlterColumn<int>(
                name: "FormateurId",
                table: "Formations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Formations_Users_FormateurId",
                table: "Formations",
                column: "FormateurId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
