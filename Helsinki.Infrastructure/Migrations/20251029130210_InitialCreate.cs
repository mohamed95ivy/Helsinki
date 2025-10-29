using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Helsinki.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Conversions",
                columns: table => new
                {
                    ConversionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FromCurrency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    ToCurrency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    FromAmount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 6, nullable: false),
                    ToAmount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 6, nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "TEXT", precision: 18, scale: 9, nullable: false),
                    ConversionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversions", x => x.ConversionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conversions_ConversionDate",
                table: "Conversions",
                column: "ConversionDate");

            migrationBuilder.CreateIndex(
                name: "IX_Conversions_UserId",
                table: "Conversions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conversions");
        }
    }
}
