using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transacciones.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPreviousAndNewBalanceColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "NewBalance",
                table: "Transactions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousBalance",
                table: "Transactions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewBalance",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "PreviousBalance",
                table: "Transactions");
        }
    }
}
