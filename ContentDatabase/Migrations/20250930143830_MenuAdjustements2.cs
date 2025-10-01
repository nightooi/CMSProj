using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContentDatabase.Migrations
{
    /// <inheritdoc />
    public partial class MenuAdjustements2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthoredComponents_PageComponents_PageComponentId",
                table: "AuthoredComponents");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthoredComponents_PageComponents_PageComponentId",
                table: "AuthoredComponents",
                column: "PageComponentId",
                principalTable: "PageComponents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthoredComponents_PageComponents_PageComponentId",
                table: "AuthoredComponents");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthoredComponents_PageComponents_PageComponentId",
                table: "AuthoredComponents",
                column: "PageComponentId",
                principalTable: "PageComponents",
                principalColumn: "Id");
        }
    }
}
