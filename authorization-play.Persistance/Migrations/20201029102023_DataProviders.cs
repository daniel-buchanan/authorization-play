using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace authorization_play.Persistance.Migrations
{
    public partial class DataProviders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataProviders",
                columns: table => new
                {
                    DataProviderId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PrincipalId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CanonicalName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProviders", x => x.DataProviderId);
                    table.ForeignKey(
                        name: "FK_DataProviders_Principals_PrincipalId",
                        column: x => x.PrincipalId,
                        principalTable: "Principals",
                        principalColumn: "PrincipalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrincipalRelations",
                columns: table => new
                {
                    PrimaryPrincipalId = table.Column<int>(nullable: false),
                    SecondaryPrincipalId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrincipalRelations", x => new { x.PrimaryPrincipalId, x.SecondaryPrincipalId });
                    table.ForeignKey(
                        name: "FK_PrincipalRelations_Principals_PrimaryPrincipalId",
                        column: x => x.PrimaryPrincipalId,
                        principalTable: "Principals",
                        principalColumn: "PrincipalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrincipalRelations_Principals_SecondaryPrincipalId",
                        column: x => x.SecondaryPrincipalId,
                        principalTable: "Principals",
                        principalColumn: "PrincipalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataProviderPolicies",
                columns: table => new
                {
                    DataProviderPolicyId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataProviderId = table.Column<int>(nullable: false),
                    SchemaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProviderPolicies", x => x.DataProviderPolicyId);
                    table.ForeignKey(
                        name: "FK_DataProviderPolicies_DataProviders_DataProviderId",
                        column: x => x.DataProviderId,
                        principalTable: "DataProviders",
                        principalColumn: "DataProviderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataProviderPolicies_Schemas_SchemaId",
                        column: x => x.SchemaId,
                        principalTable: "Schemas",
                        principalColumn: "SchemaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataSources",
                columns: table => new
                {
                    DataSourceId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataProviderId = table.Column<int>(nullable: false),
                    CanonicalName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSources", x => x.DataSourceId);
                    table.ForeignKey(
                        name: "FK_DataSources_DataProviders_DataProviderId",
                        column: x => x.DataProviderId,
                        principalTable: "DataProviders",
                        principalColumn: "DataProviderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataProviderPolicyItems",
                columns: table => new
                {
                    DataProviderPolicyId = table.Column<int>(nullable: false),
                    PrincipalId = table.Column<int>(nullable: false),
                    Allow = table.Column<bool>(nullable: false),
                    Deny = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProviderPolicyItems", x => new { x.DataProviderPolicyId, x.PrincipalId });
                    table.ForeignKey(
                        name: "FK_DataProviderPolicyItems_DataProviderPolicies_DataProviderPo~",
                        column: x => x.DataProviderPolicyId,
                        principalTable: "DataProviderPolicies",
                        principalColumn: "DataProviderPolicyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataProviderPolicyItems_Principals_PrincipalId",
                        column: x => x.PrincipalId,
                        principalTable: "Principals",
                        principalColumn: "PrincipalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataProviderPolicies_DataProviderId",
                table: "DataProviderPolicies",
                column: "DataProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_DataProviderPolicies_SchemaId",
                table: "DataProviderPolicies",
                column: "SchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_DataProviderPolicyItems_PrincipalId",
                table: "DataProviderPolicyItems",
                column: "PrincipalId");

            migrationBuilder.CreateIndex(
                name: "IX_DataProviders_PrincipalId",
                table: "DataProviders",
                column: "PrincipalId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSources_DataProviderId",
                table: "DataSources",
                column: "DataProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_PrincipalRelations_SecondaryPrincipalId",
                table: "PrincipalRelations",
                column: "SecondaryPrincipalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataProviderPolicyItems");

            migrationBuilder.DropTable(
                name: "DataSources");

            migrationBuilder.DropTable(
                name: "PrincipalRelations");

            migrationBuilder.DropTable(
                name: "DataProviderPolicies");

            migrationBuilder.DropTable(
                name: "DataProviders");
        }
    }
}
