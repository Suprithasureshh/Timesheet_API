using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Joy.TS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class chanduD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hashpassword",
                table: "employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hashpassword",
                table: "employees");
        }
    }
}
