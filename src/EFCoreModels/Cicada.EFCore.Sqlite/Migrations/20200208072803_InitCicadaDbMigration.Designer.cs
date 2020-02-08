﻿// <auto-generated />
using System;
using Cicada.EFCore.Shared.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cicada.EFCore.Sqlite.Migrations
{
    [DbContext(typeof(CicadaDbContext))]
    [Migration("20200208072803_InitCicadaDbMigration")]
    partial class InitCicadaDbMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1");

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.Calendar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<string>("CalendarId")
                        .IsRequired()
                        .HasColumnType("varchar(32)");

                    b.Property<string>("CalendarName")
                        .IsRequired()
                        .HasColumnType("varchar(128)");

                    b.Property<int>("Day")
                        .HasColumnType("int(11)");

                    b.Property<int>("Month")
                        .HasColumnType("int(11)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(128)");

                    b.Property<int>("Year")
                        .HasColumnType("int(11)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex("CalendarId")
                        .HasName("IX_Calendar_CalendarId");

                    b.HasIndex("Day")
                        .HasName("IX_Calendar_Day");

                    b.HasIndex("Month")
                        .HasName("IX_Calendar_Month");

                    b.HasIndex("Year")
                        .HasName("IX_Calendar_Year");

                    b.ToTable("cicada_calendars");
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.CheckResult", b =>
                {
                    b.Property<string>("CheckId")
                        .HasColumnType("varchar(32)");

                    b.Property<DateTime?>("BeginTime")
                        .IsRequired()
                        .HasColumnType("timestamp");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("varchar(32)");

                    b.Property<long>("CostMillisecond")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("timestamp");

                    b.Property<string>("MessageInfo")
                        .HasColumnType("text");

                    b.Property<int>("MessageLevel")
                        .HasColumnType("int(11)");

                    b.Property<int?>("ProcessId")
                        .HasColumnType("int(11)");

                    b.Property<int>("Status")
                        .HasColumnType("int(11)");

                    b.Property<string>("TaskId")
                        .IsRequired()
                        .HasColumnType("varchar(32)");

                    b.HasKey("CheckId")
                        .HasName("PRIMARY");

                    b.HasIndex("BeginTime")
                        .HasName("IX_CheckResult_BeginTime");

                    b.HasIndex("ClientId")
                        .HasName("IX_CheckResult_ClientId");

                    b.HasIndex("EndTime")
                        .HasName("IX_CheckResult_EndTime");

                    b.HasIndex("TaskId")
                        .HasName("IX_CheckResult_TaskId");

                    b.ToTable("cicada_check_results");
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.Client", b =>
                {
                    b.Property<string>("ClientId")
                        .HasColumnType("varchar(32)");

                    b.Property<string>("ClientName")
                        .IsRequired()
                        .HasColumnType("varchar(64)");

                    b.Property<int>("ClientType")
                        .HasColumnType("int(11)");

                    b.Property<string>("IP")
                        .IsRequired()
                        .HasColumnType("varchar(64)");

                    b.Property<int>("MaxProcessNum")
                        .HasColumnType("int(11)");

                    b.Property<int>("PlatformId")
                        .HasColumnType("int(11)");

                    b.Property<string>("PublicKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Remark")
                        .HasColumnType("varchar(128)");

                    b.HasKey("ClientId")
                        .HasName("PRIMARY");

                    b.HasIndex("ClientName")
                        .HasName("IX_Client_ClientName");

                    b.HasIndex("PlatformId")
                        .HasName("IX_Client_PlatformId");

                    b.ToTable("cicada_clients");
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.Config", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("varchar(64)");

                    b.Property<string>("ClientId")
                        .HasColumnType("varchar(32)");

                    b.Property<DateTime>("CreateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("'2000-01-01 00:00:00'");

                    b.Property<string>("CreateUserId")
                        .IsRequired()
                        .HasColumnType("varchar(32)");

                    b.Property<string>("DefaultValue")
                        .HasColumnType("varchar(512)");

                    b.Property<string>("Descript")
                        .HasColumnType("varchar(1024)");

                    b.Property<string>("EffectModel")
                        .HasColumnType("varchar(255)");

                    b.Property<sbyte>("EnableView")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(4)")
                        .HasDefaultValueSql("'1'");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(128)");

                    b.Property<int?>("Order")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasDefaultValueSql("'0'");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasDefaultValueSql("'0'");

                    b.Property<string>("VaildRule")
                        .HasColumnType("varchar(512)");

                    b.Property<string>("Value")
                        .HasColumnType("varchar(512)");

                    b.HasKey("Key", "ClientId");

                    b.ToTable("cicada_configs");
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.Member", b =>
                {
                    b.Property<string>("MemberId")
                        .HasColumnType("varchar(32)");

                    b.Property<string>("Avatar")
                        .HasColumnType("varchar(256)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(256)");

                    b.Property<string>("ExtendId")
                        .HasColumnType("varchar(64)");

                    b.Property<string>("FromSource")
                        .HasColumnType("varchar(128)");

                    b.Property<int>("Gender")
                        .HasColumnType("int(11)");

                    b.Property<string>("Mobile")
                        .HasColumnType("varchar(32)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Position")
                        .HasColumnType("varchar(64)");

                    b.Property<int>("Status")
                        .HasColumnType("int(11)");

                    b.HasKey("MemberId")
                        .HasName("PRIMARY");

                    b.HasIndex("ExtendId")
                        .HasName("IX_Member_ExtendId");

                    b.HasIndex("FromSource")
                        .HasName("IX_Member_FromSource");

                    b.HasIndex("Status")
                        .HasName("IX_Member_Status");

                    b.ToTable("cicada_members");
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.MemberParty", b =>
                {
                    b.Property<string>("MemberId")
                        .HasColumnType("varchar(32)");

                    b.Property<string>("PartyId")
                        .HasColumnType("varchar(32)");

                    b.HasKey("MemberId", "PartyId")
                        .HasName("PRIMARY");

                    b.HasIndex("MemberId")
                        .HasName("IX_MemberParty_MemberId");

                    b.HasIndex("PartyId")
                        .HasName("IX_MemberParty_PartyId");

                    b.ToTable("cicada_member_parties");
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.MemberTag", b =>
                {
                    b.Property<string>("MemberId")
                        .HasColumnType("varchar(32)");

                    b.Property<string>("TagId")
                        .HasColumnType("varchar(32)");

                    b.HasKey("MemberId", "TagId")
                        .HasName("PRIMARY");

                    b.HasIndex("MemberId")
                        .HasName("IX_MemberTag_MemberId");

                    b.HasIndex("TagId")
                        .HasName("IX_MemberTag_TagId");

                    b.ToTable("cicada_member_tags");
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.Notice", b =>
                {
                    b.Property<string>("NoticeId")
                        .HasColumnType("varchar(32)");

                    b.Property<string>("CheckResultCheckId")
                        .HasColumnType("TEXT");

                    b.Property<string>("EffectLevels")
                        .HasColumnType("varchar(256)");

                    b.Property<string>("IgnoreTime")
                        .HasColumnType("varchar(512)");

                    b.Property<string>("TaskId")
                        .IsRequired()
                        .HasColumnType("varchar(32)");

                    b.Property<string>("ToMembers")
                        .HasColumnType("varchar(512)");

                    b.Property<string>("ToParties")
                        .HasColumnType("varchar(512)");

                    b.Property<string>("ToTags")
                        .HasColumnType("varchar(512)");

                    b.HasKey("NoticeId")
                        .HasName("PRIMARY");

                    b.HasIndex("CheckResultCheckId");

                    b.HasIndex("TaskId")
                        .HasName("IX_Notice_TaskId");

                    b.ToTable("cicada_notices");
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.NotifyRecode", b =>
                {
                    b.Property<string>("NotifyRecodeId")
                        .HasColumnType("varchar(32)");

                    b.Property<string>("CheckId")
                        .IsRequired()
                        .HasColumnType("varchar(32)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp");

                    b.Property<string>("MemberId")
                        .IsRequired()
                        .HasColumnType("varchar(32)");

                    b.Property<string>("Msg")
                        .HasColumnType("text");

                    b.Property<string>("NoticeId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReplyMsg")
                        .HasColumnType("varchar(2048)");

                    b.Property<DateTime>("ReplyTime")
                        .HasColumnType("timestamp");

                    b.Property<int>("Status")
                        .HasColumnType("int(11)");

                    b.Property<string>("TaskId")
                        .IsRequired()
                        .HasColumnType("varchar(32)");

                    b.HasKey("NotifyRecodeId")
                        .HasName("PRIMARY");

                    b.HasIndex("CheckId")
                        .HasName("IX_NotifyRecode_CheckId");

                    b.HasIndex("MemberId")
                        .HasName("IX_NotifyRecode_MemberId");

                    b.HasIndex("NoticeId");

                    b.HasIndex("TaskId")
                        .HasName("IX_NotifyRecode_TaskId");

                    b.ToTable("cicada_notice_recodes");
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.Party", b =>
                {
                    b.Property<string>("PartyId")
                        .HasColumnType("varchar(32)");

                    b.Property<string>("ExtendId")
                        .HasColumnType("varchar(64)");

                    b.Property<string>("FromSource")
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(32)");

                    b.Property<long>("Order")
                        .HasColumnType("bigint(64)");

                    b.Property<string>("ParentPartyId")
                        .HasColumnType("varchar(32)");

                    b.Property<string>("Remark")
                        .HasColumnType("varchar(2048)");

                    b.HasKey("PartyId")
                        .HasName("PRIMARY");

                    b.ToTable("cicada_parties");
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.Permission", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Descript")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(64)");

                    b.Property<string>("ParentId")
                        .HasColumnType("varchar(128)");

                    b.Property<string>("PluginId")
                        .IsRequired()
                        .HasColumnType("varchar(128)");

                    b.Property<int>("Status")
                        .HasColumnType("int(11)");

                    b.Property<int>("Type")
                        .HasColumnType("int(11)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex("ParentId")
                        .HasName("IX_Permission_ParentId");

                    b.HasIndex("PluginId")
                        .HasName("IX_Permission_PluginId");

                    b.HasIndex("Status")
                        .HasName("IX_Permission_IsDisabled");

                    b.HasIndex("Type")
                        .HasName("IX_Permission_Type");

                    b.ToTable("cicada_permissions");
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.RolePermission", b =>
                {
                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(32)");

                    b.Property<string>("PermissionId")
                        .HasColumnType("varchar(32)");

                    b.Property<int>("Type")
                        .HasColumnType("int(11)");

                    b.HasKey("RoleId", "PermissionId")
                        .HasName("PRIMARY");

                    b.HasIndex("PermissionId")
                        .HasName("IX_RolePermission_PermissionId");

                    b.HasIndex("RoleId")
                        .HasName("IX_RolePermission_RoleId");

                    b.HasIndex("Type")
                        .HasName("IX_RolePermission_Type");

                    b.ToTable("cicada_role_permissions");
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.SystemInfo", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("varchar(32)");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("timestamp");

                    b.Property<string>("Version")
                        .HasColumnType("varchar(32)");

                    b.HasKey("Name")
                        .HasName("PRIMARY");

                    b.ToTable("cicada_system_infos");
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.Tag", b =>
                {
                    b.Property<string>("TagId")
                        .HasColumnType("varchar(32)");

                    b.Property<string>("ExtendId")
                        .HasColumnType("varchar(64)");

                    b.Property<string>("FromSource")
                        .HasColumnType("varchar(128)");

                    b.Property<long>("Order")
                        .HasColumnType("bigint(64)");

                    b.Property<string>("Remark")
                        .HasColumnType("varchar(2048)");

                    b.Property<string>("TagName")
                        .IsRequired()
                        .HasColumnType("varchar(64)");

                    b.HasKey("TagId")
                        .HasName("PRIMARY");

                    b.ToTable("cicada_tags");
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.TagParty", b =>
                {
                    b.Property<string>("TagId")
                        .HasColumnType("varchar(32)");

                    b.Property<string>("PartyId")
                        .HasColumnType("varchar(32)");

                    b.HasKey("TagId", "PartyId")
                        .HasName("PRIMARY");

                    b.HasIndex("PartyId")
                        .HasName("IX_TagParty_PartyId");

                    b.HasIndex("TagId")
                        .HasName("IX_TagParty_TagId");

                    b.ToTable("cicada_tag_parties");
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.TaskInfo", b =>
                {
                    b.Property<string>("TaskId")
                        .HasColumnType("varchar(32)");

                    b.Property<string>("Command")
                        .IsRequired()
                        .HasColumnType("varchar(512)");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("timestamp");

                    b.Property<int?>("LastCheckStatus")
                        .HasColumnType("int(11)");

                    b.Property<int>("MaxRuningThread")
                        .HasColumnType("int(11)");

                    b.Property<int>("MaxThreadInOneClient")
                        .HasColumnType("int(11)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(128)");

                    b.Property<DateTime?>("NextRunTime")
                        .HasColumnType("timestamp");

                    b.Property<string>("Owner")
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Params")
                        .HasColumnType("varchar(2048)");

                    b.Property<int>("Retries")
                        .HasColumnType("int(11)");

                    b.Property<DateTime>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp");

                    b.Property<int>("RuleType")
                        .HasColumnType("int(4)");

                    b.Property<string>("Schedule")
                        .IsRequired()
                        .HasColumnType("varchar(4096)");

                    b.Property<string>("SleepCalendar")
                        .HasColumnType("varchar(512)");

                    b.Property<string>("SleepDays")
                        .HasColumnType("varchar(512)");

                    b.Property<bool>("SleepFirst")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SleepTimes")
                        .HasColumnType("varchar(512)");

                    b.Property<string>("SleepWeeks")
                        .HasColumnType("varchar(512)");

                    b.Property<int>("Status")
                        .HasColumnType("int(11)");

                    b.Property<string>("SupportClientIds")
                        .HasColumnType("varchar(2048)");

                    b.Property<int?>("SupportPlatforms")
                        .HasColumnType("int(11)");

                    b.Property<long>("Timeout")
                        .HasColumnType("bigint(20)");

                    b.Property<int>("Type")
                        .HasColumnType("int(4)");

                    b.Property<string>("WorkCalendar")
                        .HasColumnType("varchar(512)");

                    b.Property<string>("WorkDays")
                        .HasColumnType("varchar(512)");

                    b.Property<string>("WorkTimes")
                        .HasColumnType("varchar(512)");

                    b.Property<string>("WorkWeeks")
                        .HasColumnType("varchar(512)");

                    b.HasKey("TaskId")
                        .HasName("PRIMARY");

                    b.HasIndex("NextRunTime")
                        .HasName("IX_TaskInfo_NextRunTime");

                    b.HasIndex("Status")
                        .HasName("IX_TaskInfo_Status");

                    b.ToTable("cicada_task_infos");
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.UserMember", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(32)");

                    b.Property<string>("MemberId")
                        .HasColumnType("varchar(32)");

                    b.HasKey("UserId", "MemberId")
                        .HasName("PRIMARY");

                    b.HasIndex("MemberId")
                        .HasName("IX_UserMember_MemberId");

                    b.HasIndex("UserId")
                        .HasName("IX_UserMember_UserId");

                    b.ToTable("cicada_user_members");
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.UserPermission", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(32)");

                    b.Property<string>("PermissionId")
                        .HasColumnType("varchar(32)");

                    b.Property<int>("Type")
                        .HasColumnType("int(11)");

                    b.HasKey("UserId", "PermissionId")
                        .HasName("PRIMARY");

                    b.HasIndex("PermissionId")
                        .HasName("IX_UserPermission_PermissionId");

                    b.HasIndex("Type")
                        .HasName("IX_UserPermission_Type");

                    b.HasIndex("UserId")
                        .HasName("IX_UserPermission_UserId");

                    b.ToTable("cicada_user_permissions");
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.CheckResult", b =>
                {
                    b.HasOne("Cicada.EFCore.Shared.Models.TaskInfo", "TaskInfo")
                        .WithMany("CheckResults")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.MemberParty", b =>
                {
                    b.HasOne("Cicada.EFCore.Shared.Models.Member", "Member")
                        .WithMany("MemberParties")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Cicada.EFCore.Shared.Models.Party", "Party")
                        .WithMany("MemberParties")
                        .HasForeignKey("PartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.MemberTag", b =>
                {
                    b.HasOne("Cicada.EFCore.Shared.Models.Member", "Member")
                        .WithMany("MemberTags")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Cicada.EFCore.Shared.Models.Tag", "Tag")
                        .WithMany("UserTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.Notice", b =>
                {
                    b.HasOne("Cicada.EFCore.Shared.Models.CheckResult", null)
                        .WithMany("Notices")
                        .HasForeignKey("CheckResultCheckId");

                    b.HasOne("Cicada.EFCore.Shared.Models.TaskInfo", "TaskInfo")
                        .WithMany("Notices")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.NotifyRecode", b =>
                {
                    b.HasOne("Cicada.EFCore.Shared.Models.CheckResult", "CheckResult")
                        .WithMany("NotifyRecodes")
                        .HasForeignKey("CheckId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Cicada.EFCore.Shared.Models.Notice", null)
                        .WithMany("NotifyRecodes")
                        .HasForeignKey("NoticeId");

                    b.HasOne("Cicada.EFCore.Shared.Models.TaskInfo", "TaskInfo")
                        .WithMany("NotifyRecodes")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Cicada.EFCore.Shared.Models.TagParty", b =>
                {
                    b.HasOne("Cicada.EFCore.Shared.Models.Party", "Party")
                        .WithMany("TagParties")
                        .HasForeignKey("PartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Cicada.EFCore.Shared.Models.Tag", "Tag")
                        .WithMany("TagParties")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
