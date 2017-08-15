using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace _900.Models
{
    public partial class InnoSheetContext : DbContext
    {
        public virtual DbSet<Professor> Professor { get; set; }
        public virtual DbSet<Schedule> Schedule { get; set; }
        public virtual DbSet<StudyGroup> StudyGroup { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<SubjectProfessor> SubjectProfessor { get; set; }
        public virtual DbSet<Time> Time { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserSubject> UserSubject { get; set; }

        public InnoSheetContext(DbContextOptions<InnoSheetContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasKey(e => e.IdProfessor)
                    .HasName("PK_Professor");

                entity.Property(e => e.IdProfessor).HasColumnName("Id_professor");

                entity.Property(e => e.Professor1)
                    .IsRequired()
                    .HasColumnName("Professor")
                    .HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.Property(e => e.IdSubject).HasColumnName("Id_subject");

                entity.Property(e => e.IdTime).HasColumnName("Id_time");

                entity.Property(e => e.RoomNumber).HasColumnName("Room_number");

                entity.HasOne(d => d.IdSubjectNavigation)
                    .WithMany(p => p.Schedule)
                    .HasForeignKey(d => d.IdSubject)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Schedule_Subject");

                entity.HasOne(d => d.IdTimeNavigation)
                    .WithMany(p => p.Schedule)
                    .HasForeignKey(d => d.IdTime)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Schedule_Time");
            });

            modelBuilder.Entity<StudyGroup>(entity =>
            {
                entity.HasKey(e => e.IdGroup)
                    .HasName("PK_Group");

                entity.ToTable("Study_Group");

                entity.Property(e => e.IdGroup).HasColumnName("Id_Group");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasColumnName("Group_Name")
                    .HasColumnType("nchar(3)");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.IdSubject)
                    .HasName("PK_Subject");

                entity.Property(e => e.IdSubject).HasColumnName("Id_subject");

                entity.Property(e => e.IdGroup).HasColumnName("Id_Group");

                entity.Property(e => e.SubjectFull)
                    .IsRequired()
                    .HasColumnName("Subject_full")
                    .HasMaxLength(200);

                entity.Property(e => e.SubjectShort)
                    .IsRequired()
                    .HasColumnName("Subject_short")
                    .HasColumnType("nchar(15)");

                entity.HasOne(d => d.IdGroupNavigation)
                    .WithMany(p => p.Subject)
                    .HasForeignKey(d => d.IdGroup)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Subject_Group");
            });

            modelBuilder.Entity<SubjectProfessor>(entity =>
            {
                entity.HasKey(e => e.IdSubProf)
                    .HasName("PK_Subject_Professor");

                entity.ToTable("Subject_Professor");

                entity.Property(e => e.IdSubProf).HasColumnName("Id_sub_prof");

                entity.Property(e => e.IdProfessor).HasColumnName("Id_professor");

                entity.Property(e => e.IdSubject).HasColumnName("Id_subject");

                entity.HasOne(d => d.IdProfessorNavigation)
                    .WithMany(p => p.SubjectProfessor)
                    .HasForeignKey(d => d.IdProfessor)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Subject_Professor_Professor");

                entity.HasOne(d => d.IdSubjectNavigation)
                    .WithMany(p => p.SubjectProfessor)
                    .HasForeignKey(d => d.IdSubject)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Subject_Subject_Professor");
            });

            modelBuilder.Entity<Time>(entity =>
            {
                entity.HasKey(e => e.IdTime)
                    .HasName("PK_Time");

                entity.Property(e => e.IdTime).HasColumnName("Id_time");

                entity.Property(e => e.Time1)
                    .IsRequired()
                    .HasColumnName("Time")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.TelegramId)
                    .HasName("PK_User");

                entity.Property(e => e.TelegramId)
                    .HasColumnName("Telegram_Id")
                    .ValueGeneratedNever();

                entity.Property(e => e.TelegramName)
                    .IsRequired()
                    .HasColumnName("Telegram_Name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<UserSubject>(entity =>
            {
                entity.ToTable("User_Subject");

                entity.Property(e => e.IdSubject).HasColumnName("Id_subject");

                entity.Property(e => e.TelegramId).HasColumnName("Telegram_Id");

                entity.HasOne(d => d.IdSubjectNavigation)
                    .WithMany(p => p.UserSubject)
                    .HasForeignKey(d => d.IdSubject)
                    .HasConstraintName("FK_User_Subject_Subject");

                entity.HasOne(d => d.Telegram)
                    .WithMany(p => p.UserSubject)
                    .HasForeignKey(d => d.TelegramId)
                    .HasConstraintName("FK_User_Subject_User");
            });
        }
    }
}