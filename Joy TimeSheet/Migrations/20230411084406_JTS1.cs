using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeSheet.Migrations
{
    /// <inheritdoc />
    public partial class JTS1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    Client_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Client_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is_Active = table.Column<bool>(type: "bit", nullable: false),
                    Create_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified_Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.Client_Id);
                });

            migrationBuilder.CreateTable(
                name: "designations",
                columns: table => new
                {
                    Designation_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is_Active = table.Column<bool>(type: "bit", nullable: false),
                    Create_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified_Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_designations", x => x.Designation_Id);
                });

            migrationBuilder.CreateTable(
                name: "employeeProject",
                columns: table => new
                {
                    Employee_Project_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Employee_Id = table.Column<int>(type: "int", nullable: false),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    Start_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Create_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified_Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employeeProject", x => x.Employee_Project_Id);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    Employee_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    First_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Last_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Employee_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reporting_Manager1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reportinng_Manager2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Employee_Type_Id = table.Column<int>(type: "int", nullable: false),
                    Official_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alternate_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role_Id = table.Column<int>(type: "int", nullable: false),
                    Designation_Id = table.Column<int>(type: "int", nullable: false),
                    Contact_No = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is_Active = table.Column<bool>(type: "bit", nullable: false),
                    Joining_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Create_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified_Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.Employee_Id);
                });

            migrationBuilder.CreateTable(
                name: "employeeTypes",
                columns: table => new
                {
                    Employee_Type_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Employee_Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is_Active = table.Column<bool>(type: "bit", nullable: false),
                    Create_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified_Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employeeTypes", x => x.Employee_Type_Id);
                });

            migrationBuilder.CreateTable(
                name: "hrContactInformations",
                columns: table => new
                {
                    Hr_Contact_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    First_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Last_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hr_Email_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hr_Contact_No = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is_Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hrContactInformations", x => x.Hr_Contact_Id);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    Project_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Project_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Project_Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Client_Id = table.Column<int>(type: "int", nullable: false),
                    Is_Active = table.Column<bool>(type: "bit", nullable: false),
                    Project_Start_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Project_End_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Create_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified_Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.Project_Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Role_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Create_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified_Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Role_Id);
                });

            migrationBuilder.CreateTable(
                name: "timeSheets",
                columns: table => new
                {
                    TimeSheet_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Employee_Id = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Leave = table.Column<bool>(type: "bit", nullable: false),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    TimesheetSummary_Id = table.Column<int>(type: "int", nullable: false),
                    Duration_in_Hours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_timeSheets", x => x.TimeSheet_Id);
                });

            migrationBuilder.CreateTable(
                name: "timeSheetSummarys",
                columns: table => new
                {
                    TimesheetSummary_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Employee_Id = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    No_Of_days_Worked = table.Column<double>(type: "float", nullable: false),
                    No_Of_Leave_Taken = table.Column<double>(type: "float", nullable: false),
                    Total_Working_Hours = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified_Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_timeSheetSummarys", x => x.TimesheetSummary_Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "designations");

            migrationBuilder.DropTable(
                name: "employeeProject");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "employeeTypes");

            migrationBuilder.DropTable(
                name: "hrContactInformations");

            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "timeSheets");

            migrationBuilder.DropTable(
                name: "timeSheetSummarys");
        }
    }
}
