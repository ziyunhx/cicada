using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicada.EFCore.Sqlite.Migrations
{
    public partial class InitCicadaIdentityDbMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cicada_roles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cicada_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cicada_users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cicada_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cicada_role_claims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cicada_role_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cicada_role_claims_cicada_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "cicada_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cicada_user_claims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cicada_user_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cicada_user_claims_cicada_users_UserId",
                        column: x => x.UserId,
                        principalTable: "cicada_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cicada_user_logins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cicada_user_logins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_cicada_user_logins_cicada_users_UserId",
                        column: x => x.UserId,
                        principalTable: "cicada_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cicada_user_roles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cicada_user_roles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_cicada_user_roles_cicada_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "cicada_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cicada_user_roles_cicada_users_UserId",
                        column: x => x.UserId,
                        principalTable: "cicada_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cicada_user_tokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cicada_user_tokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_cicada_user_tokens_cicada_users_UserId",
                        column: x => x.UserId,
                        principalTable: "cicada_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cicada_role_claims_RoleId",
                table: "cicada_role_claims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "cicada_roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cicada_user_claims_UserId",
                table: "cicada_user_claims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_cicada_user_logins_UserId",
                table: "cicada_user_logins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_cicada_user_roles_RoleId",
                table: "cicada_user_roles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "cicada_users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "cicada_users",
                column: "NormalizedUserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cicada_role_claims");

            migrationBuilder.DropTable(
                name: "cicada_user_claims");

            migrationBuilder.DropTable(
                name: "cicada_user_logins");

            migrationBuilder.DropTable(
                name: "cicada_user_roles");

            migrationBuilder.DropTable(
                name: "cicada_user_tokens");

            migrationBuilder.DropTable(
                name: "cicada_roles");

            migrationBuilder.DropTable(
                name: "cicada_users");
        }
    }
}
