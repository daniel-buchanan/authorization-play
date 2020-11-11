using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace authorization_play.Persistance.Migrations
{
    public partial class AddDataSourceConnections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataSourceConnections",
                columns: table => new
                {
                    DataSourceConnectionId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataSourceId = table.Column<int>(nullable: false),
                    DataProviderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSourceConnections", x => x.DataSourceConnectionId);
                    table.ForeignKey(
                        name: "FK_DataSourceConnections_DataProviders_DataProviderId",
                        column: x => x.DataProviderId,
                        principalTable: "DataProviders",
                        principalColumn: "DataProviderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataSourceConnections_DataSources_DataSourceId",
                        column: x => x.DataSourceId,
                        principalTable: "DataSources",
                        principalColumn: "DataSourceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataSourceResourceConnections",
                columns: table => new
                {
                    DataSourceConnectionId = table.Column<int>(nullable: false),
                    ResourceId = table.Column<int>(nullable: false),
                    SchemaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSourceResourceConnections", x => new { x.DataSourceConnectionId, x.ResourceId, x.SchemaId });
                    table.ForeignKey(
                        name: "FK_DataSourceResourceConnections_DataSourceConnections_DataSou~",
                        column: x => x.DataSourceConnectionId,
                        principalTable: "DataSourceConnections",
                        principalColumn: "DataSourceConnectionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataSourceResourceConnections_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "ResourceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataSourceResourceConnections_Schemas_SchemaId",
                        column: x => x.SchemaId,
                        principalTable: "Schemas",
                        principalColumn: "SchemaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataSourceConnections_DataProviderId",
                table: "DataSourceConnections",
                column: "DataProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSourceConnections_DataSourceId",
                table: "DataSourceConnections",
                column: "DataSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSourceResourceConnections_ResourceId",
                table: "DataSourceResourceConnections",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSourceResourceConnections_SchemaId",
                table: "DataSourceResourceConnections",
                column: "SchemaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataSourceResourceConnections");

            migrationBuilder.DropTable(
                name: "DataSourceConnections");
        }
    }
}
