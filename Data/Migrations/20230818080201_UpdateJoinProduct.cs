using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateJoinProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TestJoinId",
                table: "Product",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_TestJoinId",
                table: "Product",
                column: "TestJoinId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_AspNetUsers_TestJoinId",
                table: "Product",
                column: "TestJoinId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_AspNetUsers_TestJoinId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_TestJoinId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "TestJoinId",
                table: "Product");
        }
    }
}
