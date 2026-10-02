using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMSystem.Migrations
{
    /// <inheritdoc />
    public partial class addleadconversiontable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_leads_customers_ConvertedCustomerId",
                table: "leads");

            migrationBuilder.DropColumn(
                name: "ConvertedAt",
                table: "leads");

            migrationBuilder.CreateTable(
                name: "lead_conversions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    LeadId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConvertedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConvertedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lead_conversions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lead_conversions_AspNetUsers_ConvertedByUserId",
                        column: x => x.ConvertedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_lead_conversions_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_lead_conversions_leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lead_conversions_organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_lead_conversions_ConvertedAt",
                table: "lead_conversions",
                column: "ConvertedAt");

            migrationBuilder.CreateIndex(
                name: "IX_lead_conversions_ConvertedByUserId",
                table: "lead_conversions",
                column: "ConvertedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_lead_conversions_CustomerId",
                table: "lead_conversions",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lead_conversions_LeadId",
                table: "lead_conversions",
                column: "LeadId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lead_conversions_OrganizationId",
                table: "lead_conversions",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_leads_customers_ConvertedCustomerId",
                table: "leads",
                column: "ConvertedCustomerId",
                principalTable: "customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_leads_customers_ConvertedCustomerId",
                table: "leads");

            migrationBuilder.DropTable(
                name: "lead_conversions");

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
    }
}
