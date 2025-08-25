using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContentDatabase.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetFileTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetFileTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetHostDomains",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DomainName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DomainUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetHostDomains", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetFileTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    AssetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssetDomainId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetFileTypeId1 = table.Column<int>(type: "int", nullable: false),
                    Constructed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Generated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevisionAuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Published = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CopyRight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CopyRightDisclaimer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastRevisionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevisionDiff = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assets_AssetFileTypes_AssetFileTypeId1",
                        column: x => x.AssetFileTypeId1,
                        principalTable: "AssetFileTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assets_AssetHostDomains_AssetDomainId",
                        column: x => x.AssetDomainId,
                        principalTable: "AssetHostDomains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assets_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PageComponent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentPosition = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Constructed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Generated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevisionAuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Published = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CopyRight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CopyRightDisclaimer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastRevisionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevisionDiff = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageComponent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageComponent_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PageTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Constructed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Generated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevisionAuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Published = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CopyRight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CopyRightDisclaimer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastRevisionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevisionDiff = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageTemplates_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PageComponentPageTemplate",
                columns: table => new
                {
                    PageComponentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageComponentPageTemplate", x => new { x.PageComponentsId, x.PageTemplateId });
                    table.ForeignKey(
                        name: "FK_PageComponentPageTemplate_PageComponent_PageComponentsId",
                        column: x => x.PageComponentsId,
                        principalTable: "PageComponent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PageComponentPageTemplate_PageTemplates_PageTemplateId",
                        column: x => x.PageTemplateId,
                        principalTable: "PageTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    PageName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PageTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Constructed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Generated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevisionAuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Published = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CopyRight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CopyRightDisclaimer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastRevisionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevisionDiff = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pages_PageTemplates_PageTemplateId",
                        column: x => x.PageTemplateId,
                        principalTable: "PageTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false),
                    Constructed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Generated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevisionAuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Published = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CopyRight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CopyRightDisclaimer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastRevisionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevisionDiff = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageVersions_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PageVersions_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuothoredComponents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayLoad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComponentName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CssUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeaderJsUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    PageVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Constructed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Generated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevisionAuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Published = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CopyRight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CopyRightDisclaimer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastRevisionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevisionDiff = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuothoredComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuothoredComponents_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AuothoredComponents_PageComponent_PageComponentId",
                        column: x => x.PageComponentId,
                        principalTable: "PageComponent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuothoredComponents_PageVersions_PageVersionId",
                        column: x => x.PageVersionId,
                        principalTable: "PageVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetFileTypes_FileType",
                table: "AssetFileTypes",
                column: "FileType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetDomainId",
                table: "Assets",
                column: "AssetDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetFileTypeId1",
                table: "Assets",
                column: "AssetFileTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AuthorId",
                table: "Assets",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_Url",
                table: "Assets",
                column: "Url",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuothoredComponents_AuthorId",
                table: "AuothoredComponents",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuothoredComponents_PageComponentId",
                table: "AuothoredComponents",
                column: "PageComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_AuothoredComponents_PageVersionId",
                table: "AuothoredComponents",
                column: "PageVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_PageComponent_AuthorId",
                table: "PageComponent",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_PageComponentPageTemplate_PageTemplateId",
                table: "PageComponentPageTemplate",
                column: "PageTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_AuthorId",
                table: "Pages",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_PageTemplateId",
                table: "Pages",
                column: "PageTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_PageTemplates_AuthorId",
                table: "PageTemplates",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_PageVersions_AuthorId",
                table: "PageVersions",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_PageVersions_PageId",
                table: "PageVersions",
                column: "PageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "AuothoredComponents");

            migrationBuilder.DropTable(
                name: "PageComponentPageTemplate");

            migrationBuilder.DropTable(
                name: "AssetFileTypes");

            migrationBuilder.DropTable(
                name: "AssetHostDomains");

            migrationBuilder.DropTable(
                name: "PageVersions");

            migrationBuilder.DropTable(
                name: "PageComponent");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "PageTemplates");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
