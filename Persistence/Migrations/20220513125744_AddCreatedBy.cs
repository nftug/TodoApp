using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class AddCreatedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "TodoItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_CreatedById",
                table: "TodoItems",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_AspNetUsers_CreatedById",
                table: "TodoItems",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_AspNetUsers_CreatedById",
                table: "TodoItems");

            migrationBuilder.DropIndex(
                name: "IX_TodoItems_CreatedById",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "TodoItems");
        }
    }
}
