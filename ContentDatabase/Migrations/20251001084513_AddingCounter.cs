using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContentDatabase.Migrations
{
    /// <inheritdoc />
    public partial class AddingCounter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Counter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Page = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CounterUpdate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CounterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogMessage = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CounterUpdate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CounterUpdate_Counter_CounterId",
                        column: x => x.CounterId,
                        principalTable: "Counter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Counter_Page",
                table: "Counter",
                column: "Page",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CounterUpdate_CounterId",
                table: "CounterUpdate",
                column: "CounterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CounterUpdate");

            migrationBuilder.DropTable(
                name: "Counter");
        }
    }
}
