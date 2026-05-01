using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehiclePartsIMS_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AmountPaid",
                table: "SalesInvoices",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDueDate",
                table: "SalesInvoices",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                table: "PurchaseInvoices",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "PaymentDueDate",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "PurchaseInvoices");
        }
    }
}
