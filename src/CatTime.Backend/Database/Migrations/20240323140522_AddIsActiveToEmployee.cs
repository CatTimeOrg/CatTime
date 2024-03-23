using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatTime.Backend.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Employees",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmailAddress",
                table: "Employees",
                column: "EmailAddress",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_EmailAddress",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Employees");
        }
    }
}
