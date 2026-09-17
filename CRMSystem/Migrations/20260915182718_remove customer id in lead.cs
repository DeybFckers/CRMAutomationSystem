using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMSystem.Migrations
{
    /// <inheritdoc />
    public partial class removecustomeridinlead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_leads_customers_CustomerId",
                table: "leads");

            migrationBuilder.AddForeignKey(
                name: "FK_leads_customers_CustomerId",
                table: "leads",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_leads_customers_CustomerId",
                table: "leads");

            migrationBuilder.AddForeignKey(
                name: "FK_leads_customers_CustomerId",
                table: "leads",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
