using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnManager.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToFormation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Formations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Formations");
        }
    }
}
