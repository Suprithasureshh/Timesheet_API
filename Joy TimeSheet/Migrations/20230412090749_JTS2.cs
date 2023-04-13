using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeSheet.Migrations
{
    /// <inheritdoc />
    public partial class JTS2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "viewPreviousChanges",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Employee_Id = table.Column<int>(type: "int", nullable: false),
                    First_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Last_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Employee_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reporting_Manager1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reportinng_Manager2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Employee_Type_Id = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alternate_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Designation_Id = table.Column<int>(type: "int", nullable: false),
                    Contact_No = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is_Active = table.Column<bool>(type: "bit", nullable: false),
                    Joining_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified_Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_viewPreviousChanges", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "viewPreviousChanges");
        }
    }
}
