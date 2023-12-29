using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class Product3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "PriceJump",
                table: "Product",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceJump",
                table: "Product");
        }
    }
}
