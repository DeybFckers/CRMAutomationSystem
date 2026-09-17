using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMSystem.Migrations
{
    /// <inheritdoc />
    public partial class betterstructureforleadandcustomerrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_leads_customers_CustomerId",
                table: "leads");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "leads",
                newName: "ConvertedCustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_leads_CustomerId",
                table: "leads",
                newName: "IX_leads_ConvertedCustomerId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ConvertedAt",
                table: "leads",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_leads_customers_ConvertedCustomerId",
                table: "leads",
                column: "ConvertedCustomerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_leads_customers_ConvertedCustomerId",
                table: "leads");

            migrationBuilder.DropColumn(
                name: "ConvertedAt",
                table: "leads");

            migrationBuilder.RenameColumn(
                name: "ConvertedCustomerId",
                table: "leads",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_leads_ConvertedCustomerId",
                table: "leads",
                newName: "IX_leads_CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_leads_customers_CustomerId",
                table: "leads",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "Id");
        }
    }
}
