using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExcelUploadApi.Migrations
{
    /// <inheritdoc />
    public partial class remove2Col : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Column3",
                table: "Data");

            migrationBuilder.DropColumn(
                name: "Column4",
                table: "Data");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Column3",
                table: "Data",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Column4",
                table: "Data",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
