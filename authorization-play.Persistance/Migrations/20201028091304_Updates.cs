using Microsoft.EntityFrameworkCore.Migrations;

namespace authorization_play.Persistance.Migrations
{
    public partial class Updates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceActions_Actions_ActionId",
                table: "ResourceActions");

            migrationBuilder.DropForeignKey(
                name: "FK_Resources_ResourceKinds_ResourceKindId",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Resources_ResourceKindId",
                table: "Resources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResourceActions",
                table: "ResourceActions");

            migrationBuilder.DropIndex(
                name: "IX_ResourceActions_ActionId",
                table: "ResourceActions");

            migrationBuilder.DropColumn(
                name: "ResourceKindId",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "ResourceActionId",
                table: "ResourceActions");

            migrationBuilder.AlterColumn<int>(
                name: "ActionId",
                table: "ResourceActions",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResourceActions",
                table: "ResourceActions",
                columns: new[] { "ActionId", "ResourceId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceActions_Actions_ActionId",
                table: "ResourceActions",
                column: "ActionId",
                principalTable: "Actions",
                principalColumn: "ActionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceActions_Actions_ActionId",
                table: "ResourceActions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResourceActions",
                table: "ResourceActions");

            migrationBuilder.AddColumn<int>(
                name: "ResourceKindId",
                table: "Resources",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ActionId",
                table: "ResourceActions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ResourceActionId",
                table: "ResourceActions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResourceActions",
                table: "ResourceActions",
                columns: new[] { "ResourceActionId", "ResourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ResourceKindId",
                table: "Resources",
                column: "ResourceKindId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceActions_ActionId",
                table: "ResourceActions",
                column: "ActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceActions_Actions_ActionId",
                table: "ResourceActions",
                column: "ActionId",
                principalTable: "Actions",
                principalColumn: "ActionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_ResourceKinds_ResourceKindId",
                table: "Resources",
                column: "ResourceKindId",
                principalTable: "ResourceKinds",
                principalColumn: "ResourceKindId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
