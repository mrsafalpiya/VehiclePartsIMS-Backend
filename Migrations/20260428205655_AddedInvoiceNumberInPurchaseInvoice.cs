using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehiclePartsIMS_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedInvoiceNumberInPurchaseInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                table: "PurchaseInvoices",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_InvoiceNumber",
                table: "PurchaseInvoices",
                column: "InvoiceNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PurchaseInvoices_InvoiceNumber",
                table: "PurchaseInvoices");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "PurchaseInvoices");
        }
    }
}
