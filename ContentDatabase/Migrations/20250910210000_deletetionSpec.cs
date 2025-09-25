using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContentDatabase.Migrations
{
    /// <inheritdoc />
    public partial class deletetionSpec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetComponentJoinTable_Assets_AssetId",
                table: "AssetComponentJoinTable");

            migrationBuilder.DropForeignKey(
                name: "FK_Pages_PageSlugs_SlugId",
                table: "Pages");

            migrationBuilder.DropForeignKey(
                name: "FK_PublishedPages_PageSlugs_SlugId",
                table: "PublishedPages");

            migrationBuilder.AlterColumn<Guid>(
                name: "SlugId",
                table: "Pages",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetComponentJoinTable_Assets_AssetId",
                table: "AssetComponentJoinTable",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_PageSlugs_SlugId",
                table: "Pages",
                column: "SlugId",
                principalTable: "PageSlugs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PublishedPages_PageSlugs_SlugId",
                table: "PublishedPages",
                column: "SlugId",
                principalTable: "PageSlugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetComponentJoinTable_Assets_AssetId",
                table: "AssetComponentJoinTable");

            migrationBuilder.DropForeignKey(
                name: "FK_Pages_PageSlugs_SlugId",
                table: "Pages");

            migrationBuilder.DropForeignKey(
                name: "FK_PublishedPages_PageSlugs_SlugId",
                table: "PublishedPages");

            migrationBuilder.AlterColumn<Guid>(
                name: "SlugId",
                table: "Pages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetComponentJoinTable_Assets_AssetId",
                table: "AssetComponentJoinTable",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_PageSlugs_SlugId",
                table: "Pages",
                column: "SlugId",
                principalTable: "PageSlugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PublishedPages_PageSlugs_SlugId",
                table: "PublishedPages",
                column: "SlugId",
                principalTable: "PageSlugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
