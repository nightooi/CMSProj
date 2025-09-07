using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContentDatabase.Migrations
{
    /// <inheritdoc />
    public partial class AuthoredFix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetFileTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    AssetDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssetDomainId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Constructed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Generated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationAuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                        name: "FK_Assets_AssetFileTypes_AssetFileTypeId",
                        column: x => x.AssetFileTypeId,
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
                        name: "FK_Assets_Authors_CreationAuthorId",
                        column: x => x.CreationAuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComponentMarkups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Markup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Constructed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Generated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationAuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Published = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CopyRight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CopyRightDisclaimer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastRevisionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevisionDiff = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentMarkups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComponentMarkups_Authors_CreationAuthorId",
                        column: x => x.CreationAuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Constructed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Generated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationAuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                        name: "FK_PageTemplates_Authors_CreationAuthorId",
                        column: x => x.CreationAuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageComponents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentHtml = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    ChildOffset = table.Column<int>(type: "int", nullable: false),
                    SelfPageOrder = table.Column<int>(type: "int", nullable: false),
                    PageTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CssHeaderTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsHeaderTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsBodyTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherHeaders = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Constructed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Generated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationAuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Published = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CopyRight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CopyRightDisclaimer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastRevisionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevisionDiff = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageComponents_Authors_CreationAuthorId",
                        column: x => x.CreationAuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PageComponents_PageTemplates_PageTemplateId",
                        column: x => x.PageTemplateId,
                        principalTable: "PageTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetComponentJoinTable",
                columns: table => new
                {
                    AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetComponentJoinTable", x => new { x.AssetId, x.ComponentId });
                    table.ForeignKey(
                        name: "FK_AssetComponentJoinTable_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetComponentJoinTable_PageComponents_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "PageComponents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AssetAuthoredJoinTable",
                columns: table => new
                {
                    AssetsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Asset = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthoredComp = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompAssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetAuthoredJoinTable", x => new { x.AssetsId, x.PageId });
                    table.UniqueConstraint("AK_AssetAuthoredJoinTable_Asset", x => x.Asset);
                    table.ForeignKey(
                        name: "FK_AssetAuthoredJoinTable_Assets_AssetsId",
                        column: x => x.AssetsId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetAuthoredJoinTable_Assets_CompAssetId",
                        column: x => x.CompAssetId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AuthoredComponents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayLoadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CssHeaderTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsHeaderTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsBodyTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    OtherHeaders = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Constructed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Generated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationAuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Published = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CopyRight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CopyRightDisclaimer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastRevisionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevisionDiff = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthoredComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthoredComponents_Authors_CreationAuthorId",
                        column: x => x.CreationAuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthoredComponents_ComponentMarkups_PayLoadId",
                        column: x => x.PayLoadId,
                        principalTable: "ComponentMarkups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AuthoredComponents_PageComponents_PageComponentId",
                        column: x => x.PageComponentId,
                        principalTable: "PageComponents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SlugId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Constructed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Generated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationAuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                        name: "FK_Pages_Authors_CreationAuthorId",
                        column: x => x.CreationAuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Constructed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Generated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationAuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                        name: "FK_PageVersions_Authors_CreationAuthorId",
                        column: x => x.CreationAuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PageVersions_PageTemplates_PageTemplateId",
                        column: x => x.PageTemplateId,
                        principalTable: "PageTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PageVersions_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PageSlugs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PageslugSlugId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageSlugs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PublishedPages",
                columns: table => new
                {
                    SlugId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishedPages", x => x.SlugId);
                    table.ForeignKey(
                        name: "FK_PublishedPages_PageSlugs_SlugId",
                        column: x => x.SlugId,
                        principalTable: "PageSlugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PublishedPages_PageVersions_PageVersionId",
                        column: x => x.PageVersionId,
                        principalTable: "PageVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PublishedPages_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetAuthoredJoinTable_CompAssetId",
                table: "AssetAuthoredJoinTable",
                column: "CompAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAuthoredJoinTable_CompId",
                table: "AssetAuthoredJoinTable",
                column: "CompId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAuthoredJoinTable_PageId",
                table: "AssetAuthoredJoinTable",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetComponentJoinTable_ComponentId",
                table: "AssetComponentJoinTable",
                column: "ComponentId");

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
                name: "IX_Assets_AssetFileTypeId",
                table: "Assets",
                column: "AssetFileTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_CreationAuthorId",
                table: "Assets",
                column: "CreationAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_Url",
                table: "Assets",
                column: "Url",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthoredComponents_CreationAuthorId",
                table: "AuthoredComponents",
                column: "CreationAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthoredComponents_PageComponentId",
                table: "AuthoredComponents",
                column: "PageComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthoredComponents_PageVersionId",
                table: "AuthoredComponents",
                column: "PageVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthoredComponents_PayLoadId",
                table: "AuthoredComponents",
                column: "PayLoadId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentMarkups_CreationAuthorId",
                table: "ComponentMarkups",
                column: "CreationAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_PageComponents_CreationAuthorId",
                table: "PageComponents",
                column: "CreationAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_PageComponents_PageTemplateId",
                table: "PageComponents",
                column: "PageTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_CreationAuthorId",
                table: "Pages",
                column: "CreationAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_SlugId",
                table: "Pages",
                column: "SlugId");

            migrationBuilder.CreateIndex(
                name: "IX_PageSlugs_PageslugSlugId",
                table: "PageSlugs",
                column: "PageslugSlugId");

            migrationBuilder.CreateIndex(
                name: "IX_PageTemplates_CreationAuthorId",
                table: "PageTemplates",
                column: "CreationAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_PageVersions_CreationAuthorId",
                table: "PageVersions",
                column: "CreationAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_PageVersions_PageId",
                table: "PageVersions",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_PageVersions_PageTemplateId",
                table: "PageVersions",
                column: "PageTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_PublishedPages_PageId",
                table: "PublishedPages",
                column: "PageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PublishedPages_PageVersionId",
                table: "PublishedPages",
                column: "PageVersionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetAuthoredJoinTable_AuthoredComponents_CompId",
                table: "AssetAuthoredJoinTable",
                column: "CompId",
                principalTable: "AuthoredComponents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetAuthoredJoinTable_AuthoredComponents_PageId",
                table: "AssetAuthoredJoinTable",
                column: "PageId",
                principalTable: "AuthoredComponents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthoredComponents_PageVersions_PageVersionId",
                table: "AuthoredComponents",
                column: "PageVersionId",
                principalTable: "PageVersions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_PageSlugs_SlugId",
                table: "Pages",
                column: "SlugId",
                principalTable: "PageSlugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PageSlugs_PublishedPages_PageslugSlugId",
                table: "PageSlugs",
                column: "PageslugSlugId",
                principalTable: "PublishedPages",
                principalColumn: "SlugId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pages_Authors_CreationAuthorId",
                table: "Pages");

            migrationBuilder.DropForeignKey(
                name: "FK_PageTemplates_Authors_CreationAuthorId",
                table: "PageTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_PageVersions_Authors_CreationAuthorId",
                table: "PageVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_PublishedPages_PageVersions_PageVersionId",
                table: "PublishedPages");

            migrationBuilder.DropForeignKey(
                name: "FK_Pages_PageSlugs_SlugId",
                table: "Pages");

            migrationBuilder.DropForeignKey(
                name: "FK_PublishedPages_PageSlugs_SlugId",
                table: "PublishedPages");

            migrationBuilder.DropTable(
                name: "AssetAuthoredJoinTable");

            migrationBuilder.DropTable(
                name: "AssetComponentJoinTable");

            migrationBuilder.DropTable(
                name: "AuthoredComponents");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "ComponentMarkups");

            migrationBuilder.DropTable(
                name: "PageComponents");

            migrationBuilder.DropTable(
                name: "AssetFileTypes");

            migrationBuilder.DropTable(
                name: "AssetHostDomains");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "PageVersions");

            migrationBuilder.DropTable(
                name: "PageTemplates");

            migrationBuilder.DropTable(
                name: "PageSlugs");

            migrationBuilder.DropTable(
                name: "PublishedPages");

            migrationBuilder.DropTable(
                name: "Pages");
        }
    }
}
