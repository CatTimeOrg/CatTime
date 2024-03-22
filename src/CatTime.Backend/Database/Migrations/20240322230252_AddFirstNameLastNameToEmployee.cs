using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatTime.Backend.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFirstNameLastNameToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Employees",
                type: "text",
                nullable: false,
                defaultValue: "John");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Employees",
                type: "text",
                nullable: false,
                defaultValue: "Smith");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Employees");
        }
    }
}
