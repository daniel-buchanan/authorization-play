using Microsoft.EntityFrameworkCore.Migrations;

namespace authorization_play.Persistance.Migrations
{
    public partial class UpdatedPrincipalRelationNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrincipalRelations_Principals_PrimaryPrincipalId",
                table: "PrincipalRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_PrincipalRelations_Principals_SecondaryPrincipalId",
                table: "PrincipalRelations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrincipalRelations",
                table: "PrincipalRelations");

            migrationBuilder.DropIndex(
                name: "IX_PrincipalRelations_SecondaryPrincipalId",
                table: "PrincipalRelations");

            migrationBuilder.DropColumn(
                name: "PrimaryPrincipalId",
                table: "PrincipalRelations");

            migrationBuilder.DropColumn(
                name: "SecondaryPrincipalId",
                table: "PrincipalRelations");

            migrationBuilder.AddColumn<int>(
                name: "ParentPrincipalId",
                table: "PrincipalRelations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChildPrincipalId",
                table: "PrincipalRelations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrincipalRelations",
                table: "PrincipalRelations",
                columns: new[] { "ParentPrincipalId", "ChildPrincipalId" });

            migrationBuilder.CreateIndex(
                name: "IX_PrincipalRelations_ChildPrincipalId",
                table: "PrincipalRelations",
                column: "ChildPrincipalId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrincipalRelations_Principals_ChildPrincipalId",
                table: "PrincipalRelations",
                column: "ChildPrincipalId",
                principalTable: "Principals",
                principalColumn: "PrincipalId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrincipalRelations_Principals_ParentPrincipalId",
                table: "PrincipalRelations",
                column: "ParentPrincipalId",
                principalTable: "Principals",
                principalColumn: "PrincipalId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrincipalRelations_Principals_ChildPrincipalId",
                table: "PrincipalRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_PrincipalRelations_Principals_ParentPrincipalId",
                table: "PrincipalRelations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrincipalRelations",
                table: "PrincipalRelations");

            migrationBuilder.DropIndex(
                name: "IX_PrincipalRelations_ChildPrincipalId",
                table: "PrincipalRelations");

            migrationBuilder.DropColumn(
                name: "ParentPrincipalId",
                table: "PrincipalRelations");

            migrationBuilder.DropColumn(
                name: "ChildPrincipalId",
                table: "PrincipalRelations");

            migrationBuilder.AddColumn<int>(
                name: "PrimaryPrincipalId",
                table: "PrincipalRelations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondaryPrincipalId",
                table: "PrincipalRelations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrincipalRelations",
                table: "PrincipalRelations",
                columns: new[] { "PrimaryPrincipalId", "SecondaryPrincipalId" });

            migrationBuilder.CreateIndex(
                name: "IX_PrincipalRelations_SecondaryPrincipalId",
                table: "PrincipalRelations",
                column: "SecondaryPrincipalId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrincipalRelations_Principals_PrimaryPrincipalId",
                table: "PrincipalRelations",
                column: "PrimaryPrincipalId",
                principalTable: "Principals",
                principalColumn: "PrincipalId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrincipalRelations_Principals_SecondaryPrincipalId",
                table: "PrincipalRelations",
                column: "SecondaryPrincipalId",
                principalTable: "Principals",
                principalColumn: "PrincipalId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
