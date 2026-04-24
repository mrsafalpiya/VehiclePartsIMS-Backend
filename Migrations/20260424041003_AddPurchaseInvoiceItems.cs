using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehiclePartsIMS_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseInvoiceItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "SalesInvoices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AmountPaid",
                table: "SalesInvoices",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
