using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContentDatabase.Migrations
{
    /// <inheritdoc />
    public partial class AddingCounterAdj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CounterUpdate_Counter_CounterId",
                table: "CounterUpdate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CounterUpdate",
                table: "CounterUpdate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Counter",
                table: "Counter");

            migrationBuilder.RenameTable(
                name: "CounterUpdate",
                newName: "CounterUpdates");

            migrationBuilder.RenameTable(
                name: "Counter",
                newName: "Counters");

            migrationBuilder.RenameIndex(
                name: "IX_CounterUpdate_CounterId",
                table: "CounterUpdates",
                newName: "IX_CounterUpdates_CounterId");

            migrationBuilder.RenameIndex(
                name: "IX_Counter_Page",
                table: "Counters",
                newName: "IX_Counters_Page");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CounterUpdates",
                table: "CounterUpdates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Counters",
                table: "Counters",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CounterUpdates_Counters_CounterId",
                table: "CounterUpdates",
                column: "CounterId",
                principalTable: "Counters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CounterUpdates_Counters_CounterId",
                table: "CounterUpdates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CounterUpdates",
                table: "CounterUpdates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Counters",
                table: "Counters");

            migrationBuilder.RenameTable(
                name: "CounterUpdates",
                newName: "CounterUpdate");

            migrationBuilder.RenameTable(
                name: "Counters",
                newName: "Counter");

            migrationBuilder.RenameIndex(
                name: "IX_CounterUpdates_CounterId",
                table: "CounterUpdate",
                newName: "IX_CounterUpdate_CounterId");

            migrationBuilder.RenameIndex(
                name: "IX_Counters_Page",
                table: "Counter",
                newName: "IX_Counter_Page");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CounterUpdate",
                table: "CounterUpdate",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Counter",
                table: "Counter",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CounterUpdate_Counter_CounterId",
                table: "CounterUpdate",
                column: "CounterId",
                principalTable: "Counter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
