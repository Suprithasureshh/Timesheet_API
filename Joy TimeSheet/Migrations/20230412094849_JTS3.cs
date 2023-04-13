using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeSheet.Migrations
{
    /// <inheritdoc />
    public partial class JTS3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role_Id",
                table: "employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role_Id",
                table: "employees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
