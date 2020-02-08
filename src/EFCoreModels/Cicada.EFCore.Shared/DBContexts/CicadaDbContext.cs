using Cicada.EFCore.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Cicada.EFCore.Shared.DBContexts
{
    public partial class CicadaDbContext : DbContext
    {
        public CicadaDbContext(DbContextOptions<CicadaDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CheckResult> CheckResults { get; set; }
        public virtual DbSet<Config> Configs { get; set; }
        public virtual DbSet<Notice> Notices { get; set; }
        public virtual DbSet<NotifyRecode> NotifyRecodes { get; set; }
        public virtual DbSet<Party> Parties { get; set; }
        public virtual DbSet<SystemInfo> SystemInfos { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TagParty> TagParties { get; set; }
        public virtual DbSet<TaskInfo> TaskInfos { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<UserMember> UserMembers { get; set; }
        public virtual DbSet<MemberParty> MemberParties { get; set; }
        public virtual DbSet<MemberTag> MemberTags { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Calendar> Calendars { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<UserPermission> UserPermissions { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CheckResult>(entity =>
            {
                entity.ToTable("cicada_check_results");

                entity.HasKey(e => e.CheckId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.BeginTime)
                    .HasName("IX_CheckResult_BeginTime");

                entity.HasIndex(e => e.EndTime)
                    .HasName("IX_CheckResult_EndTime");

                entity.HasIndex(e => e.TaskId)
                    .HasName("IX_CheckResult_TaskId");

                entity.HasIndex(e => e.ClientId)
                    .HasName("IX_CheckResult_ClientId");

                entity.Property(e => e.CheckId).IsRequired().HasColumnType("varchar(32)");

                entity.Property(e => e.BeginTime).IsRequired().HasColumnType("timestamp");

                entity.Property(e => e.EndTime).HasColumnType("timestamp");

                entity.Property(e => e.ClientId).IsRequired().HasColumnType("varchar(32)");

                entity.Property(e => e.ProcessId).HasColumnType("int(11)");

                entity.Property(e => e.MessageInfo).HasColumnType("text");

                entity.Property(e => e.MessageLevel).HasColumnType("int(11)");

                entity.Property(e => e.Status).IsRequired().HasColumnType("int(11)");

                entity.Property(e => e.TaskId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.HasOne(d => d.TaskInfo)
                    .WithMany(p => p.CheckResults)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Config>(entity =>
            {
                entity.HasKey(e => new { e.Key, e.ClientId });

                entity.ToTable("cicada_configs");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.ClientId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.CreateTime)
                    .IsRequired()
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'2000-01-01 00:00:00'");

                entity.Property(e => e.CreateUserId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.DefaultValue)
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.Descript)
                    .HasColumnType("varchar(1024)");

                entity.Property(e => e.EffectModel)
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.EnableView)
                    .IsRequired()
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.Order)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Status)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.VaildRule)
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.Value)
                    .HasColumnType("varchar(512)");
            });

            modelBuilder.Entity<Notice>(entity =>
            {
                entity.ToTable("cicada_notices");

                entity.HasKey(e => e.NoticeId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.TaskId)
                    .HasName("IX_Notice_TaskId");

                entity.Property(e => e.NoticeId).IsRequired().HasColumnType("varchar(32)");

                entity.Property(e => e.IgnoreTime).HasColumnType("varchar(512)");

                entity.Property(e => e.EffectLevels).HasColumnType("varchar(256)");

                entity.Property(e => e.TaskId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.ToParties).HasColumnType("varchar(512)");

                entity.Property(e => e.ToTags).HasColumnType("varchar(512)");

                entity.Property(e => e.ToMembers).HasColumnType("varchar(512)");

                entity.HasOne(d => d.TaskInfo)
                    .WithMany(p => p.Notices)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<NotifyRecode>(entity =>
            {
                entity.ToTable("cicada_notice_recodes");

                entity.HasKey(e => e.NotifyRecodeId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.CheckId)
                    .HasName("IX_NotifyRecode_CheckId");

                entity.HasIndex(e => e.MemberId)
                    .HasName("IX_NotifyRecode_MemberId");

                entity.HasIndex(e => e.TaskId)
                    .HasName("IX_NotifyRecode_TaskId");

                entity.Property(e => e.NotifyRecodeId).IsRequired().HasColumnType("varchar(32)");

                entity.Property(e => e.CheckId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.CreateTime).IsRequired().HasColumnType("timestamp");

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Msg).HasColumnType("text");

                entity.Property(e => e.ReplyMsg).HasColumnType("varchar(2048)");

                entity.Property(e => e.ReplyTime).HasColumnType("timestamp");

                entity.Property(e => e.Status).IsRequired().HasColumnType("int(11)");

                entity.Property(e => e.TaskId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.HasOne(d => d.CheckResult)
                    .WithMany(p => p.NotifyRecodes)
                    .HasForeignKey(d => d.CheckId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.TaskInfo)
                    .WithMany(p => p.NotifyRecodes)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Party>(entity =>
            {
                entity.ToTable("cicada_parties");

                entity.HasKey(e => e.PartyId)
                    .HasName("PRIMARY");

                entity.Property(e => e.PartyId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Remark)
                    .HasColumnType("varchar(2048)");

                entity.Property(e => e.Order).HasColumnType("bigint(64)");

                entity.Property(e => e.ParentPartyId).HasColumnType("varchar(32)");

                entity.Property(e => e.ExtendId).HasColumnType("varchar(64)");

                entity.Property(e => e.FromSource).HasColumnType("varchar(128)");
            });

            modelBuilder.Entity<SystemInfo>(entity =>
            {
                entity.ToTable("cicada_system_infos");

                entity.HasKey(e => e.Name)
                    .HasName("PRIMARY");

                entity.Property(e => e.Name).IsRequired().HasColumnType("varchar(32)");

                entity.Property(e => e.UpdateTime).IsRequired().HasColumnType("timestamp");

                entity.Property(e => e.Version).HasColumnType("varchar(32)");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("cicada_tags");

                entity.HasKey(e => e.TagId)
                    .HasName("PRIMARY");

                entity.Property(e => e.TagId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.TagName)
                    .IsRequired()
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.Order).HasColumnType("bigint(64)");

                entity.Property(e => e.Remark)
                    .HasColumnType("varchar(2048)");

                entity.Property(e => e.ExtendId).HasColumnType("varchar(64)");

                entity.Property(e => e.FromSource).HasColumnType("varchar(128)");
            });

            modelBuilder.Entity<TagParty>(entity =>
            {
                entity.ToTable("cicada_tag_parties");

                entity.HasKey(e => new { e.TagId, e.PartyId })
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.PartyId)
                    .HasName("IX_TagParty_PartyId");

                entity.HasIndex(e => e.TagId)
                    .HasName("IX_TagParty_TagId");

                entity.Property(e => e.TagId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.PartyId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.TagParties)
                    .HasForeignKey(d => d.PartyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.TagParties)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TaskInfo>(entity =>
            {
                entity.ToTable("cicada_task_infos");

                entity.HasKey(e => e.TaskId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Status)
                    .HasName("IX_TaskInfo_Status");

                entity.HasIndex(e => e.NextRunTime)
                    .HasName("IX_TaskInfo_NextRunTime");

                entity.Property(e => e.TaskId).IsRequired().HasColumnType("varchar(32)");

                entity.Property(e => e.Status).IsRequired().HasColumnType("int(11)");

                entity.Property(e => e.EndTime).HasColumnType("timestamp");

                entity.Property(e => e.RuleType).IsRequired().HasColumnType("int(4)");

                entity.Property(e => e.Type).IsRequired().HasColumnType("int(4)");

                entity.Property(e => e.SupportPlatforms).HasColumnType("int(11)");

                entity.Property(e => e.SupportClientIds).HasColumnType("varchar(2048)");

                entity.Property(e => e.LastCheckStatus).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.NextRunTime).HasColumnType("timestamp");

                entity.Property(e => e.Owner).HasColumnType("varchar(128)");

                entity.Property(e => e.Params).HasColumnType("varchar(2048)");

                entity.Property(e => e.Command).IsRequired().HasColumnType("varchar(512)");

                entity.Property(e => e.Retries).IsRequired().HasColumnType("int(11)");

                entity.Property(e => e.Schedule).IsRequired().HasColumnType("varchar(4096)");

                entity.Property(e => e.SleepDays).HasColumnType("varchar(512)");

                entity.Property(e => e.SleepTimes).HasColumnType("varchar(512)");

                entity.Property(e => e.SleepWeeks).HasColumnType("varchar(512)");

                entity.Property(e => e.SleepCalendar).HasColumnType("varchar(512)");

                entity.Property(e => e.Timeout).HasColumnType("bigint(20)");

                entity.Property(e => e.WorkDays).HasColumnType("varchar(512)");

                entity.Property(e => e.WorkTimes).HasColumnType("varchar(512)");

                entity.Property(e => e.WorkWeeks).HasColumnType("varchar(512)");

                entity.Property(e => e.WorkCalendar).HasColumnType("varchar(512)");

                entity.Property(e => e.SleepFirst).IsRequired();

                entity.Property(e => e.MaxRuningThread).IsRequired().HasColumnType("int(11)");

                entity.Property(e => e.MaxThreadInOneClient).IsRequired().HasColumnType("int(11)");

                entity.Property(e => e.RowVersion).HasColumnType("timestamp").IsRowVersion().ValueGeneratedOnAddOrUpdate();

            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("cicada_members");

                entity.HasKey(e => e.MemberId)
                   .HasName("PRIMARY");

                entity.HasIndex(e => e.Status)
                    .HasName("IX_Member_Status");

                entity.HasIndex(e => e.ExtendId)
                    .HasName("IX_Member_ExtendId");

                entity.HasIndex(e => e.FromSource)
                    .HasName("IX_Member_FromSource");

                entity.Property(e => e.MemberId).IsRequired().HasColumnType("varchar(32)");

                entity.Property(e => e.Avatar).HasColumnType("varchar(256)");

                entity.Property(e => e.Email).HasColumnType("varchar(256)");

                entity.Property(e => e.Gender).HasColumnType("int(11)");

                entity.Property(e => e.Mobile).HasColumnType("varchar(32)");

                entity.Property(e => e.Name).IsRequired().HasColumnType("varchar(128)");

                entity.Property(e => e.Position).HasColumnType("varchar(64)");

                entity.Property(e => e.Status).IsRequired().HasColumnType("int(11)");

                entity.Property(e => e.ExtendId).HasColumnType("varchar(64)");

                entity.Property(e => e.FromSource).HasColumnType("varchar(128)");
            });

            modelBuilder.Entity<MemberParty>(entity =>
            {
                entity.ToTable("cicada_member_parties");

                entity.HasKey(e => new { e.MemberId, e.PartyId })
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.PartyId)
                    .HasName("IX_MemberParty_PartyId");

                entity.HasIndex(e => e.MemberId)
                    .HasName("IX_MemberParty_MemberId");

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.PartyId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.MemberParties)
                    .HasForeignKey(d => d.PartyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MemberParties)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<MemberTag>(entity =>
            {
                entity.ToTable("cicada_member_tags");

                entity.HasKey(e => new { e.MemberId, e.TagId })
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.TagId)
                    .HasName("IX_MemberTag_TagId");

                entity.HasIndex(e => e.MemberId)
                    .HasName("IX_MemberTag_MemberId");

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.TagId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.UserTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MemberTags)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("cicada_clients");

                entity.HasKey(e => new { e.ClientId })
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.ClientName)
                    .HasName("IX_Client_ClientName");

                entity.HasIndex(e => e.PlatformId)
                    .HasName("IX_Client_PlatformId");

                entity.Property(e => e.ClientId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.ClientName)
                    .IsRequired()
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.ClientType)
                    .IsRequired()
                    .HasColumnType("int(11)");

                entity.Property(e => e.IP)
                    .IsRequired()
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.MaxProcessNum)
                    .IsRequired()
                    .HasColumnType("int(11)");

                entity.Property(e => e.PlatformId)
                    .IsRequired()
                    .HasColumnType("int(11)");

                entity.Property(e => e.PublicKey)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Remark)
                    .HasColumnType("varchar(128)");
            });

            modelBuilder.Entity<Calendar>(entity =>
            {
                entity.ToTable("cicada_calendars");

                entity.HasKey(e => new { e.Id })
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.CalendarId)
                    .HasName("IX_Calendar_CalendarId");

                entity.HasIndex(e => e.Year)
                    .HasName("IX_Calendar_Year");

                entity.HasIndex(e => e.Month)
                    .HasName("IX_Calendar_Month");

                entity.HasIndex(e => e.Day)
                   .HasName("IX_Calendar_Day");
                    
                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.Year)
                    .IsRequired()
                    .HasColumnType("int(11)");

                entity.Property(e => e.Month)
                    .IsRequired()
                    .HasColumnType("int(11)");

                entity.Property(e => e.Day)
                    .IsRequired()
                    .HasColumnType("int(11)");

                entity.Property(e => e.CalendarId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.CalendarName)
                    .IsRequired()
                    .HasColumnType("varchar(128)");
            });

            modelBuilder.Entity<UserMember>(entity =>
            {
                entity.ToTable("cicada_user_members");

                entity.HasKey(e => new { e.UserId, e.MemberId })
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_UserMember_UserId");

                entity.HasIndex(e => e.MemberId)
                    .HasName("IX_UserMember_MemberId");

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("cicada_permissions");

                entity.HasKey(e => new { e.Id })
                   .HasName("PRIMARY");

                entity.HasIndex(e => e.ParentId)
                    .HasName("IX_Permission_ParentId");

                entity.HasIndex(e => e.Type)
                    .HasName("IX_Permission_Type");

                entity.HasIndex(e => e.PluginId)
                    .HasName("IX_Permission_PluginId");

                entity.HasIndex(e => e.Status)
                    .HasName("IX_Permission_IsDisabled");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.ParentId)
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnType("int(11)");

                entity.Property(e => e.PluginId)
                    .IsRequired()
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.Descript)
                    .HasColumnType("text");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.ToTable("cicada_user_permissions");

                entity.HasKey(e => new { e.UserId, e.PermissionId })
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_UserPermission_UserId");

                entity.HasIndex(e => e.PermissionId)
                    .HasName("IX_UserPermission_PermissionId");

                entity.HasIndex(e => e.Type)
                    .HasName("IX_UserPermission_Type");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.PermissionId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("cicada_role_permissions");

                entity.HasKey(e => new { e.RoleId, e.PermissionId })
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_RolePermission_RoleId");

                entity.HasIndex(e => e.PermissionId)
                    .HasName("IX_RolePermission_PermissionId");

                entity.HasIndex(e => e.Type)
                    .HasName("IX_RolePermission_Type");

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.PermissionId)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnType("int(11)");
            });
        }
    }
}
