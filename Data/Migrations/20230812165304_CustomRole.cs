using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class CustomRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descriptions",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descriptions",
                table: "AspNetRoles");
        }
    }
}
