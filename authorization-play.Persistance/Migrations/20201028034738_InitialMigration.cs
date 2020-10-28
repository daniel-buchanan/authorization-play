using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace authorization_play.Persistance.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    ActionId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CanonicalName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.ActionId);
                });

            migrationBuilder.CreateTable(
                name: "Principals",
                columns: table => new
                {
                    PrincipalId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CanonicalName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Principals", x => x.PrincipalId);
                });

            migrationBuilder.CreateTable(
                name: "ResourceKinds",
                columns: table => new
                {
                    ResourceKindId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceKinds", x => x.ResourceKindId);
                });

            migrationBuilder.CreateTable(
                name: "Schemas",
                columns: table => new
                {
                    SchemaId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CanonicalName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schemas", x => x.SchemaId);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CanonicalName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    ResourceId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CanonicalName = table.Column<string>(nullable: true),
                    ResourceKindId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.ResourceId);
                    table.ForeignKey(
                        name: "FK_Resources_ResourceKinds_ResourceKindId",
                        column: x => x.ResourceKindId,
                        principalTable: "ResourceKinds",
                        principalColumn: "ResourceKindId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DelegationGrants",
                columns: table => new
                {
                    DelegationGrantId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PrincipalId = table.Column<int>(nullable: false),
                    SchemaId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelegationGrants", x => x.DelegationGrantId);
                    table.ForeignKey(
                        name: "FK_DelegationGrants_Principals_PrincipalId",
                        column: x => x.PrincipalId,
                        principalTable: "Principals",
                        principalColumn: "PrincipalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DelegationGrants_Schemas_SchemaId",
                        column: x => x.SchemaId,
                        principalTable: "Schemas",
                        principalColumn: "SchemaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DelegationGrants_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermissionGrants",
                columns: table => new
                {
                    PermissionGrantId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PrincipalId = table.Column<int>(nullable: false),
                    SchemaId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGrants", x => x.PermissionGrantId);
                    table.ForeignKey(
                        name: "FK_PermissionGrants_Principals_PrincipalId",
                        column: x => x.PrincipalId,
                        principalTable: "Principals",
                        principalColumn: "PrincipalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionGrants_Schemas_SchemaId",
                        column: x => x.SchemaId,
                        principalTable: "Schemas",
                        principalColumn: "SchemaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionGrants_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResourceActions",
                columns: table => new
                {
                    ResourceId = table.Column<int>(nullable: false),
                    ResourceActionId = table.Column<int>(nullable: false),
                    ActionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceActions", x => new { x.ResourceActionId, x.ResourceId });
                    table.ForeignKey(
                        name: "FK_ResourceActions_Actions_ActionId",
                        column: x => x.ActionId,
                        principalTable: "Actions",
                        principalColumn: "ActionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResourceActions_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "ResourceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DelegationGrantResourceActions",
                columns: table => new
                {
                    DelegationGrantId = table.Column<int>(nullable: false),
                    ActionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelegationGrantResourceActions", x => new { x.DelegationGrantId, x.ActionId });
                    table.ForeignKey(
                        name: "FK_DelegationGrantResourceActions_Actions_ActionId",
                        column: x => x.ActionId,
                        principalTable: "Actions",
                        principalColumn: "ActionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DelegationGrantResourceActions_DelegationGrants_DelegationG~",
                        column: x => x.DelegationGrantId,
                        principalTable: "DelegationGrants",
                        principalColumn: "DelegationGrantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DelegationGrantResources",
                columns: table => new
                {
                    DelegationGrantId = table.Column<int>(nullable: false),
                    ResourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelegationGrantResources", x => new { x.DelegationGrantId, x.ResourceId });
                    table.ForeignKey(
                        name: "FK_DelegationGrantResources_DelegationGrants_DelegationGrantId",
                        column: x => x.DelegationGrantId,
                        principalTable: "DelegationGrants",
                        principalColumn: "DelegationGrantId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DelegationGrantResources_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "ResourceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DelegationGrantTags",
                columns: table => new
                {
                    DelegationGrantId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelegationGrantTags", x => new { x.DelegationGrantId, x.TagId });
                    table.ForeignKey(
                        name: "FK_DelegationGrantTags_DelegationGrants_DelegationGrantId",
                        column: x => x.DelegationGrantId,
                        principalTable: "DelegationGrants",
                        principalColumn: "DelegationGrantId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DelegationGrantTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionGrantResourceActions",
                columns: table => new
                {
                    PermissionGrantId = table.Column<int>(nullable: false),
                    ActionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGrantResourceActions", x => new { x.PermissionGrantId, x.ActionId });
                    table.ForeignKey(
                        name: "FK_PermissionGrantResourceActions_Actions_ActionId",
                        column: x => x.ActionId,
                        principalTable: "Actions",
                        principalColumn: "ActionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionGrantResourceActions_PermissionGrants_PermissionG~",
                        column: x => x.PermissionGrantId,
                        principalTable: "PermissionGrants",
                        principalColumn: "PermissionGrantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionGrantResources",
                columns: table => new
                {
                    PermissionGrantId = table.Column<int>(nullable: false),
                    ResourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGrantResources", x => new { x.PermissionGrantId, x.ResourceId });
                    table.ForeignKey(
                        name: "FK_PermissionGrantResources_PermissionGrants_PermissionGrantId",
                        column: x => x.PermissionGrantId,
                        principalTable: "PermissionGrants",
                        principalColumn: "PermissionGrantId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionGrantResources_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "ResourceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionGrantTags",
                columns: table => new
                {
                    PermissionGrantId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGrantTags", x => new { x.PermissionGrantId, x.TagId });
                    table.ForeignKey(
                        name: "FK_PermissionGrantTags_PermissionGrants_PermissionGrantId",
                        column: x => x.PermissionGrantId,
                        principalTable: "PermissionGrants",
                        principalColumn: "PermissionGrantId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionGrantTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DelegationGrantResourceActions_ActionId",
                table: "DelegationGrantResourceActions",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_DelegationGrantResources_ResourceId",
                table: "DelegationGrantResources",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_DelegationGrants_PrincipalId",
                table: "DelegationGrants",
                column: "PrincipalId");

            migrationBuilder.CreateIndex(
                name: "IX_DelegationGrants_SchemaId",
                table: "DelegationGrants",
                column: "SchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_DelegationGrants_TagId",
                table: "DelegationGrants",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_DelegationGrantTags_TagId",
                table: "DelegationGrantTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGrantResourceActions_ActionId",
                table: "PermissionGrantResourceActions",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGrantResources_ResourceId",
                table: "PermissionGrantResources",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGrants_PrincipalId",
                table: "PermissionGrants",
                column: "PrincipalId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGrants_SchemaId",
                table: "PermissionGrants",
                column: "SchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGrants_TagId",
                table: "PermissionGrants",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGrantTags_TagId",
                table: "PermissionGrantTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceActions_ActionId",
                table: "ResourceActions",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceActions_ResourceId",
                table: "ResourceActions",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ResourceKindId",
                table: "Resources",
                column: "ResourceKindId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DelegationGrantResourceActions");

            migrationBuilder.DropTable(
                name: "DelegationGrantResources");

            migrationBuilder.DropTable(
                name: "DelegationGrantTags");

            migrationBuilder.DropTable(
                name: "PermissionGrantResourceActions");

            migrationBuilder.DropTable(
                name: "PermissionGrantResources");

            migrationBuilder.DropTable(
                name: "PermissionGrantTags");

            migrationBuilder.DropTable(
                name: "ResourceActions");

            migrationBuilder.DropTable(
                name: "DelegationGrants");

            migrationBuilder.DropTable(
                name: "PermissionGrants");

            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "Principals");

            migrationBuilder.DropTable(
                name: "Schemas");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "ResourceKinds");
        }
    }
}
