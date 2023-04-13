using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeSheet.Migrations
{
    /// <inheritdoc />
    public partial class JTS4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role_Id",
                table: "viewPreviousChanges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Role_Id",
                table: "employees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role_Id",
                table: "viewPreviousChanges");

            migrationBuilder.DropColumn(
                name: "Role_Id",
                table: "employees");
        }
    }
}
