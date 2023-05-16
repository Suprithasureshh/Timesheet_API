﻿// <auto-generated />
using System;
using Joy.TS.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Joy.TS.DAL.Migrations
{
    [DbContext(typeof(TimeSheetContext))]
    [Migration("20230516042122_hh")]
    partial class hh
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Joy.TS.DAL.Model.Client", b =>
                {
                    b.Property<int>("Client_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Client_Id"));

                    b.Property<string>("Client_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Create_Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Is_Active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("Modified_Date")
                        .HasColumnType("datetime2");

                    b.HasKey("Client_Id");

                    b.ToTable("clients");
                });

            modelBuilder.Entity("Joy.TS.DAL.Model.Designations", b =>
                {
                    b.Property<int>("Designation_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Designation_Id"));

                    b.Property<DateTime>("Create_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Designation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Is_Active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("Modified_Date")
                        .HasColumnType("datetime2");

                    b.HasKey("Designation_Id");

                    b.ToTable("designations");
                });

            modelBuilder.Entity("Joy.TS.DAL.Model.Employee", b =>
                {
                    b.Property<int>("Employee_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Employee_Id"));

                    b.Property<string>("Alternate_Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Client_Id")
                        .HasColumnType("int");

                    b.Property<string>("Contact_No")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Create_Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Designation_Id")
                        .HasColumnType("int");

                    b.Property<int>("Employee_Type_Id")
                        .HasColumnType("int");

                    b.Property<string>("Employee_code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("End_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("First_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hashpassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Is_Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Joining_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Last_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Modified_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Official_Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Otp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Project_Id")
                        .HasColumnType("int");

                    b.Property<string>("Reporting_Manager1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role_Id")
                        .HasColumnType("int");

                    b.HasKey("Employee_Id");

                    b.ToTable("employees");
                });

            modelBuilder.Entity("Joy.TS.DAL.Model.EmployeeProject", b =>
                {
                    b.Property<int>("Employee_Project_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Employee_Project_Id"));

                    b.Property<DateTime>("Create_Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Employee_Id")
                        .HasColumnType("int");

                    b.Property<DateTime?>("End_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Modified_Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Project_Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Start_Date")
                        .HasColumnType("datetime2");

                    b.HasKey("Employee_Project_Id");

                    b.ToTable("employeeProject");
                });

            modelBuilder.Entity("Joy.TS.DAL.Model.EmployeeType", b =>
                {
                    b.Property<int>("Employee_Type_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Employee_Type_Id"));

                    b.Property<DateTime>("Create_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Employee_Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Is_Active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("Modified_Date")
                        .HasColumnType("datetime2");

                    b.HasKey("Employee_Type_Id");

                    b.ToTable("employeeTypes");
                });

            modelBuilder.Entity("Joy.TS.DAL.Model.Fiscal_Year", b =>
                {
                    b.Property<int>("Fiscal_Year_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Fiscal_Year_ID"));

                    b.Property<string>("Month")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Fiscal_Year_ID");

                    b.ToTable("Fiscal_Years");
                });

            modelBuilder.Entity("Joy.TS.DAL.Model.HrContactInformation", b =>
                {
                    b.Property<int>("Hr_Contact_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Hr_Contact_Id"));

                    b.Property<string>("First_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hr_Contact_No")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hr_Email_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Is_Active")
                        .HasColumnType("bit");

                    b.Property<string>("Last_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Hr_Contact_Id");

                    b.ToTable("hrContactInformations");
                });

            modelBuilder.Entity("Joy.TS.DAL.Model.Projects", b =>
                {
                    b.Property<int>("Project_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Project_Id"));

                    b.Property<int>("Client_Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Create_Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Is_Active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("Modified_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Project_Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Project_End_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Project_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Project_Start_Date")
                        .HasColumnType("datetime2");

                    b.HasKey("Project_Id");

                    b.ToTable("projects");
                });

            modelBuilder.Entity("Joy.TS.DAL.Model.Roles", b =>
                {
                    b.Property<int>("Role_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Role_Id"));

                    b.Property<DateTime>("Create_Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Modified_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Role_Id");

                    b.ToTable("roles");
                });

            modelBuilder.Entity("Joy.TS.DAL.Model.TimeSheetSummary", b =>
                {
                    b.Property<int>("TimesheetSummary_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TimesheetSummary_Id"));

                    b.Property<DateTime>("Created_Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Employee_Id")
                        .HasColumnType("int");

                    b.Property<int>("Fiscal_Year_ID")
                        .HasColumnType("int");

                    b.Property<string>("ImagePathTimesheet")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePathUpload")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("Modified_Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("No_Of_Leave_Taken")
                        .HasColumnType("float");

                    b.Property<double>("No_Of_days_Worked")
                        .HasColumnType("float");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Total_Working_Hours")
                        .HasColumnType("float");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("TimesheetSummary_Id");

                    b.ToTable("timeSheetSummarys");
                });

            modelBuilder.Entity("Joy.TS.DAL.Model.TimeSheets", b =>
                {
                    b.Property<int>("TimeSheet_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TimeSheet_Id"));

                    b.Property<int?>("Date")
                        .HasColumnType("int");

                    b.Property<string>("Day")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Duration_in_Hours")
                        .HasColumnType("int");

                    b.Property<int?>("Employee_Id")
                        .HasColumnType("int");

                    b.Property<bool>("Leave")
                        .HasColumnType("bit");

                    b.Property<int?>("Project_Id")
                        .HasColumnType("int");

                    b.Property<int>("TimesheetSummary_Id")
                        .HasColumnType("int");

                    b.HasKey("TimeSheet_Id");

                    b.ToTable("timeSheets");
                });

            modelBuilder.Entity("Joy.TS.DAL.Model.ViewPreviousChanges", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Alternate_Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Client_Id")
                        .HasColumnType("int");

                    b.Property<string>("Contact_No")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Designation_Id")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Employee_Id")
                        .HasColumnType("int");

                    b.Property<int>("Employee_Type_Id")
                        .HasColumnType("int");

                    b.Property<string>("Employee_code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("End_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("First_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Is_Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Joining_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Last_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Modified_Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Project_Id")
                        .HasColumnType("int");

                    b.Property<string>("Reporting_Manager1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role_Id")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("viewPreviousChanges");
                });
#pragma warning restore 612, 618
        }
    }
}
