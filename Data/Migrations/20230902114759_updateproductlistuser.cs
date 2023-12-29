using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateproductlistuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ListUserJoin",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListUserJoin",
                table: "Product");
        }
    }
}
