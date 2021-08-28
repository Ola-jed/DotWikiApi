using Microsoft.EntityFrameworkCore.Migrations;

namespace DotWikiApi.Migrations
{
    public partial class UpdatedForeignIdSnaphot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Snapshots_AspNetUsers_ApplicationUserId1",
                table: "Snapshots");

            migrationBuilder.DropIndex(
                name: "IX_Snapshots_ApplicationUserId1",
                table: "Snapshots");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Snapshots");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Snapshots",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Snapshots_ApplicationUserId",
                table: "Snapshots",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Snapshots_AspNetUsers_ApplicationUserId",
                table: "Snapshots",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Snapshots_AspNetUsers_ApplicationUserId",
                table: "Snapshots");

            migrationBuilder.DropIndex(
                name: "IX_Snapshots_ApplicationUserId",
                table: "Snapshots");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "Snapshots",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Snapshots",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Snapshots_ApplicationUserId1",
                table: "Snapshots",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Snapshots_AspNetUsers_ApplicationUserId1",
                table: "Snapshots",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
