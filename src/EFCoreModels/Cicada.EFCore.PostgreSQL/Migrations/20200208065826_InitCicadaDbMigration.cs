using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Cicada.EFCore.PostgreSQL.Migrations
{
    public partial class InitCicadaDbMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cicada_calendars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(128)", nullable: false),
                    Year = table.Column<int>(type: "int(11)", nullable: false),
                    Month = table.Column<int>(type: "int(11)", nullable: false),
                    Day = table.Column<int>(type: "int(11)", nullable: false),
                    CalendarId = table.Column<string>(type: "varchar(32)", nullable: false),
                    CalendarName = table.Column<string>(type: "varchar(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cicada_clients",
                columns: table => new
                {
                    ClientId = table.Column<string>(type: "varchar(32)", nullable: false),
                    ClientName = table.Column<string>(type: "varchar(64)", nullable: false),
                    IP = table.Column<string>(type: "varchar(64)", nullable: false),
                    Remark = table.Column<string>(type: "varchar(128)", nullable: true),
                    PlatformId = table.Column<int>(type: "int(11)", nullable: false),
                    MaxProcessNum = table.Column<int>(type: "int(11)", nullable: false),
                    ClientType = table.Column<int>(type: "int(11)", nullable: false),
                    PublicKey = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "cicada_configs",
                columns: table => new
                {
                    Key = table.Column<string>(type: "varchar(64)", nullable: false),
                    ClientId = table.Column<string>(type: "varchar(32)", nullable: false),
                    Name = table.Column<string>(type: "varchar(128)", nullable: false),
                    Value = table.Column<string>(type: "varchar(512)", nullable: true),
                    DefaultValue = table.Column<string>(type: "varchar(512)", nullable: true),
                    Status = table.Column<int>(type: "int(11)", nullable: false, defaultValueSql: "'0'"),
                    Descript = table.Column<string>(type: "varchar(1024)", nullable: true),
                    EnableView = table.Column<short>(type: "tinyint(4)", nullable: false, defaultValueSql: "'1'"),
                    EffectModel = table.Column<string>(type: "varchar(255)", nullable: true),
                    VaildRule = table.Column<string>(type: "varchar(512)", nullable: true),
                    CreateUserId = table.Column<string>(type: "varchar(32)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "'2000-01-01 00:00:00'"),
                    Order = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cicada_configs", x => new { x.Key, x.ClientId });
                });

            migrationBuilder.CreateTable(
                name: "cicada_members",
                columns: table => new
                {
                    MemberId = table.Column<string>(type: "varchar(32)", nullable: false),
                    Avatar = table.Column<string>(type: "varchar(256)", nullable: true),
                    Email = table.Column<string>(type: "varchar(256)", nullable: true),
                    Gender = table.Column<int>(type: "int(11)", nullable: false),
                    Mobile = table.Column<string>(type: "varchar(32)", nullable: true),
                    Name = table.Column<string>(type: "varchar(128)", nullable: false),
                    Position = table.Column<string>(type: "varchar(64)", nullable: true),
                    Status = table.Column<int>(type: "int(11)", nullable: false),
                    ExtendId = table.Column<string>(type: "varchar(64)", nullable: true),
                    FromSource = table.Column<string>(type: "varchar(128)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MemberId);
                });

            migrationBuilder.CreateTable(
                name: "cicada_parties",
                columns: table => new
                {
                    PartyId = table.Column<string>(type: "varchar(32)", nullable: false),
                    Name = table.Column<string>(type: "varchar(32)", nullable: false),
                    Remark = table.Column<string>(type: "varchar(2048)", nullable: true),
                    Order = table.Column<long>(type: "bigint(64)", nullable: false),
                    ParentPartyId = table.Column<string>(type: "varchar(32)", nullable: true),
                    ExtendId = table.Column<string>(type: "varchar(64)", nullable: true),
                    FromSource = table.Column<string>(type: "varchar(128)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.PartyId);
                });

            migrationBuilder.CreateTable(
                name: "cicada_permissions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(128)", nullable: false),
                    Name = table.Column<string>(type: "varchar(64)", nullable: false),
                    ParentId = table.Column<string>(type: "varchar(128)", nullable: true),
                    Type = table.Column<int>(type: "int(11)", nullable: false),
                    PluginId = table.Column<string>(type: "varchar(128)", nullable: false),
                    Descript = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cicada_role_permissions",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "varchar(32)", nullable: false),
                    PermissionId = table.Column<string>(type: "varchar(32)", nullable: false),
                    Type = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.RoleId, x.PermissionId });
                });

            migrationBuilder.CreateTable(
                name: "cicada_system_infos",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(32)", nullable: false),
                    Version = table.Column<string>(type: "varchar(32)", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "cicada_tags",
                columns: table => new
                {
                    TagId = table.Column<string>(type: "varchar(32)", nullable: false),
                    TagName = table.Column<string>(type: "varchar(64)", nullable: false),
                    Order = table.Column<long>(type: "bigint(64)", nullable: false),
                    Remark = table.Column<string>(type: "varchar(2048)", nullable: true),
                    ExtendId = table.Column<string>(type: "varchar(64)", nullable: true),
                    FromSource = table.Column<string>(type: "varchar(128)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "cicada_task_infos",
                columns: table => new
                {
                    TaskId = table.Column<string>(type: "varchar(32)", nullable: false),
                    Name = table.Column<string>(type: "varchar(128)", nullable: false),
                    Command = table.Column<string>(type: "varchar(512)", nullable: false),
                    Params = table.Column<string>(type: "varchar(2048)", nullable: true),
                    Type = table.Column<int>(type: "int(4)", nullable: false),
                    SupportPlatforms = table.Column<int>(type: "int(11)", nullable: true),
                    SupportClientIds = table.Column<string>(type: "varchar(2048)", nullable: true),
                    Owner = table.Column<string>(type: "varchar(128)", nullable: true),
                    Status = table.Column<int>(type: "int(11)", nullable: false),
                    RuleType = table.Column<int>(type: "int(4)", nullable: false),
                    Schedule = table.Column<string>(type: "varchar(4096)", nullable: false),
                    Retries = table.Column<int>(type: "int(11)", nullable: false),
                    Timeout = table.Column<long>(type: "bigint(20)", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    WorkDays = table.Column<string>(type: "varchar(512)", nullable: true),
                    SleepDays = table.Column<string>(type: "varchar(512)", nullable: true),
                    WorkTimes = table.Column<string>(type: "varchar(512)", nullable: true),
                    SleepTimes = table.Column<string>(type: "varchar(512)", nullable: true),
                    WorkWeeks = table.Column<string>(type: "varchar(512)", nullable: true),
                    SleepWeeks = table.Column<string>(type: "varchar(512)", nullable: true),
                    WorkCalendar = table.Column<string>(type: "varchar(512)", nullable: true),
                    SleepCalendar = table.Column<string>(type: "varchar(512)", nullable: true),
                    SleepFirst = table.Column<bool>(nullable: false),
                    LastCheckStatus = table.Column<int>(type: "int(11)", nullable: true),
                    NextRunTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    MaxRuningThread = table.Column<int>(type: "int(11)", nullable: false),
                    MaxThreadInOneClient = table.Column<int>(type: "int(11)", nullable: false),
                    RowVersion = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.TaskId);
                });

            migrationBuilder.CreateTable(
                name: "cicada_user_members",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(32)", nullable: false),
                    MemberId = table.Column<string>(type: "varchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.UserId, x.MemberId });
                });

            migrationBuilder.CreateTable(
                name: "cicada_user_permissions",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(32)", nullable: false),
                    PermissionId = table.Column<string>(type: "varchar(32)", nullable: false),
                    Type = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.UserId, x.PermissionId });
                });

            migrationBuilder.CreateTable(
                name: "cicada_member_parties",
                columns: table => new
                {
                    MemberId = table.Column<string>(type: "varchar(32)", nullable: false),
                    PartyId = table.Column<string>(type: "varchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.MemberId, x.PartyId });
                    table.ForeignKey(
                        name: "FK_cicada_member_parties_cicada_members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "cicada_members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cicada_member_parties_cicada_parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "cicada_parties",
                        principalColumn: "PartyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cicada_member_tags",
                columns: table => new
                {
                    MemberId = table.Column<string>(type: "varchar(32)", nullable: false),
                    TagId = table.Column<string>(type: "varchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.MemberId, x.TagId });
                    table.ForeignKey(
                        name: "FK_cicada_member_tags_cicada_members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "cicada_members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cicada_member_tags_cicada_tags_TagId",
                        column: x => x.TagId,
                        principalTable: "cicada_tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cicada_tag_parties",
                columns: table => new
                {
                    TagId = table.Column<string>(type: "varchar(32)", nullable: false),
                    PartyId = table.Column<string>(type: "varchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.TagId, x.PartyId });
                    table.ForeignKey(
                        name: "FK_cicada_tag_parties_cicada_parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "cicada_parties",
                        principalColumn: "PartyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cicada_tag_parties_cicada_tags_TagId",
                        column: x => x.TagId,
                        principalTable: "cicada_tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cicada_check_results",
                columns: table => new
                {
                    CheckId = table.Column<string>(type: "varchar(32)", nullable: false),
                    Status = table.Column<int>(type: "int(11)", nullable: false),
                    MessageInfo = table.Column<string>(type: "text", nullable: true),
                    MessageLevel = table.Column<int>(type: "int(11)", nullable: false),
                    BeginTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CostMillisecond = table.Column<long>(nullable: false),
                    ClientId = table.Column<string>(type: "varchar(32)", nullable: false),
                    ProcessId = table.Column<int>(type: "int(11)", nullable: true),
                    TaskId = table.Column<string>(type: "varchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.CheckId);
                    table.ForeignKey(
                        name: "FK_cicada_check_results_cicada_task_infos_TaskId",
                        column: x => x.TaskId,
                        principalTable: "cicada_task_infos",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cicada_notices",
                columns: table => new
                {
                    NoticeId = table.Column<string>(type: "varchar(32)", nullable: false),
                    EffectLevels = table.Column<string>(type: "varchar(256)", nullable: true),
                    IgnoreTime = table.Column<string>(type: "varchar(512)", nullable: true),
                    ToMembers = table.Column<string>(type: "varchar(512)", nullable: true),
                    ToParties = table.Column<string>(type: "varchar(512)", nullable: true),
                    ToTags = table.Column<string>(type: "varchar(512)", nullable: true),
                    TaskId = table.Column<string>(type: "varchar(32)", nullable: false),
                    CheckResultCheckId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.NoticeId);
                    table.ForeignKey(
                        name: "FK_cicada_notices_cicada_check_results_CheckResultCheckId",
                        column: x => x.CheckResultCheckId,
                        principalTable: "cicada_check_results",
                        principalColumn: "CheckId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cicada_notices_cicada_task_infos_TaskId",
                        column: x => x.TaskId,
                        principalTable: "cicada_task_infos",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cicada_notice_recodes",
                columns: table => new
                {
                    NotifyRecodeId = table.Column<string>(type: "varchar(32)", nullable: false),
                    MemberId = table.Column<string>(type: "varchar(32)", nullable: false),
                    CheckId = table.Column<string>(type: "varchar(32)", nullable: false),
                    TaskId = table.Column<string>(type: "varchar(32)", nullable: false),
                    Msg = table.Column<string>(type: "text", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Status = table.Column<int>(type: "int(11)", nullable: false),
                    ReplyMsg = table.Column<string>(type: "varchar(2048)", nullable: true),
                    ReplyTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    NoticeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.NotifyRecodeId);
                    table.ForeignKey(
                        name: "FK_cicada_notice_recodes_cicada_check_results_CheckId",
                        column: x => x.CheckId,
                        principalTable: "cicada_check_results",
                        principalColumn: "CheckId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cicada_notice_recodes_cicada_notices_NoticeId",
                        column: x => x.NoticeId,
                        principalTable: "cicada_notices",
                        principalColumn: "NoticeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cicada_notice_recodes_cicada_task_infos_TaskId",
                        column: x => x.TaskId,
                        principalTable: "cicada_task_infos",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calendar_CalendarId",
                table: "cicada_calendars",
                column: "CalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_Calendar_Day",
                table: "cicada_calendars",
                column: "Day");

            migrationBuilder.CreateIndex(
                name: "IX_Calendar_Month",
                table: "cicada_calendars",
                column: "Month");

            migrationBuilder.CreateIndex(
                name: "IX_Calendar_Year",
                table: "cicada_calendars",
                column: "Year");

            migrationBuilder.CreateIndex(
                name: "IX_CheckResult_BeginTime",
                table: "cicada_check_results",
                column: "BeginTime");

            migrationBuilder.CreateIndex(
                name: "IX_CheckResult_ClientId",
                table: "cicada_check_results",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckResult_EndTime",
                table: "cicada_check_results",
                column: "EndTime");

            migrationBuilder.CreateIndex(
                name: "IX_CheckResult_TaskId",
                table: "cicada_check_results",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_ClientName",
                table: "cicada_clients",
                column: "ClientName");

            migrationBuilder.CreateIndex(
                name: "IX_Client_PlatformId",
                table: "cicada_clients",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberParty_MemberId",
                table: "cicada_member_parties",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberParty_PartyId",
                table: "cicada_member_parties",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberTag_MemberId",
                table: "cicada_member_tags",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberTag_TagId",
                table: "cicada_member_tags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_ExtendId",
                table: "cicada_members",
                column: "ExtendId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_FromSource",
                table: "cicada_members",
                column: "FromSource");

            migrationBuilder.CreateIndex(
                name: "IX_Member_Status",
                table: "cicada_members",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_NotifyRecode_CheckId",
                table: "cicada_notice_recodes",
                column: "CheckId");

            migrationBuilder.CreateIndex(
                name: "IX_NotifyRecode_MemberId",
                table: "cicada_notice_recodes",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_cicada_notice_recodes_NoticeId",
                table: "cicada_notice_recodes",
                column: "NoticeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotifyRecode_TaskId",
                table: "cicada_notice_recodes",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_cicada_notices_CheckResultCheckId",
                table: "cicada_notices",
                column: "CheckResultCheckId");

            migrationBuilder.CreateIndex(
                name: "IX_Notice_TaskId",
                table: "cicada_notices",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_ParentId",
                table: "cicada_permissions",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_PluginId",
                table: "cicada_permissions",
                column: "PluginId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_IsDisabled",
                table: "cicada_permissions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Type",
                table: "cicada_permissions",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                table: "cicada_role_permissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                table: "cicada_role_permissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_Type",
                table: "cicada_role_permissions",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_TagParty_PartyId",
                table: "cicada_tag_parties",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_TagParty_TagId",
                table: "cicada_tag_parties",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskInfo_NextRunTime",
                table: "cicada_task_infos",
                column: "NextRunTime");

            migrationBuilder.CreateIndex(
                name: "IX_TaskInfo_Status",
                table: "cicada_task_infos",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_UserMember_MemberId",
                table: "cicada_user_members",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMember_UserId",
                table: "cicada_user_members",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_PermissionId",
                table: "cicada_user_permissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_Type",
                table: "cicada_user_permissions",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_UserId",
                table: "cicada_user_permissions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cicada_calendars");

            migrationBuilder.DropTable(
                name: "cicada_clients");

            migrationBuilder.DropTable(
                name: "cicada_configs");

            migrationBuilder.DropTable(
                name: "cicada_member_parties");

            migrationBuilder.DropTable(
                name: "cicada_member_tags");

            migrationBuilder.DropTable(
                name: "cicada_notice_recodes");

            migrationBuilder.DropTable(
                name: "cicada_permissions");

            migrationBuilder.DropTable(
                name: "cicada_role_permissions");

            migrationBuilder.DropTable(
                name: "cicada_system_infos");

            migrationBuilder.DropTable(
                name: "cicada_tag_parties");

            migrationBuilder.DropTable(
                name: "cicada_user_members");

            migrationBuilder.DropTable(
                name: "cicada_user_permissions");

            migrationBuilder.DropTable(
                name: "cicada_members");

            migrationBuilder.DropTable(
                name: "cicada_notices");

            migrationBuilder.DropTable(
                name: "cicada_parties");

            migrationBuilder.DropTable(
                name: "cicada_tags");

            migrationBuilder.DropTable(
                name: "cicada_check_results");

            migrationBuilder.DropTable(
                name: "cicada_task_infos");
        }
    }
}
