using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagement.Migrations
{
    /// <inheritdoc />
    public partial class ModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentPath",
                table: "LeaveRequests");

            migrationBuilder.RenameColumn(
                name: "AttachmentPath",
                table: "SickLeaves",
                newName: "MedicalReportPath");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "LeaveRequests",
                newName: "Comments");

            migrationBuilder.RenameColumn(
                name: "BonusLeaveDaysUsed",
                table: "Employees",
                newName: "BonusLeaveDaysRemaining");

            migrationBuilder.RenameColumn(
                name: "AnnualLeaveDaysUsed",
                table: "Employees",
                newName: "AnnualLeaveDaysRemaining");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MedicalReportPath",
                table: "SickLeaves",
                newName: "AttachmentPath");

            migrationBuilder.RenameColumn(
                name: "Comments",
                table: "LeaveRequests",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "BonusLeaveDaysRemaining",
                table: "Employees",
                newName: "BonusLeaveDaysUsed");

            migrationBuilder.RenameColumn(
                name: "AnnualLeaveDaysRemaining",
                table: "Employees",
                newName: "AnnualLeaveDaysUsed");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentPath",
                table: "LeaveRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
