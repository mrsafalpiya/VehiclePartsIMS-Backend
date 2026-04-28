using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehiclePartsIMS_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedDueDateInSalesInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDueDate",
                table: "SalesInvoices",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDueDate",
                table: "SalesInvoices");
        }
    }
}
