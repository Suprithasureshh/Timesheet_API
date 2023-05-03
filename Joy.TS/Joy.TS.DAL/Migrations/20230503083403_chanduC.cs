using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Joy.TS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class chanduC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reportinng_Manager2",
                table: "viewPreviousChanges");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reportinng_Manager2",
                table: "viewPreviousChanges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
