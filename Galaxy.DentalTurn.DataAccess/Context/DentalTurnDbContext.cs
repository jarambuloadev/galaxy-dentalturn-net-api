using Galaxy.DentalTurn.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql.NameTranslation;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Galaxy.DentalTurn.DataAccess.Context;

public class DentalTurnDbContext : DbContext
{
    public DentalTurnDbContext(DbContextOptions<DentalTurnDbContext> options) : base(options) { }

    public DbSet<Service> Services => Set<Service>();
    public DbSet<Practitioner> Practitioners => Set<Practitioner>();
    public DbSet<PractitionerService> PractitionerServices => Set<PractitionerService>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<BusinessHours> BusinessHours => Set<BusinessHours>();
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Prisma's enum labels kept their original casing (PENDING, CONFIRMED, ...) — use an
        // identity translator so EF's migration matches the labels already in the database.
        modelBuilder.HasPostgresEnum<AppointmentStatus>("AppointmentStatus", nameTranslator: new NpgsqlNullNameTranslator());

        modelBuilder.Entity<Service>(e =>
        {
            e.ToTable("services");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").HasColumnType("integer").UseIdentityByDefaultColumn();
            e.Property(x => x.Name).HasColumnName("name").IsRequired();
            e.Property(x => x.Description).HasColumnName("description");
            e.Property(x => x.DurationMin).HasColumnName("durationMin").HasDefaultValue(30);
            e.Property(x => x.Active).HasColumnName("active").HasDefaultValue(true);
            e.Property(x => x.CreatedAt).HasColumnName("createdAt").HasColumnType("timestamp(3) without time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
            e.Property(x => x.UpdatedAt).HasColumnName("updatedAt").HasColumnType("timestamp(3) without time zone");

            e.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Practitioner>(e =>
        {
            e.ToTable("practitioners");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").HasColumnType("integer").UseIdentityByDefaultColumn();
            e.Property(x => x.Name).HasColumnName("name").IsRequired();
            e.Property(x => x.Active).HasColumnName("active").HasDefaultValue(true);
            e.Property(x => x.CreatedAt).HasColumnName("createdAt").HasColumnType("timestamp(3) without time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
            e.Property(x => x.UpdatedAt).HasColumnName("updatedAt").HasColumnType("timestamp(3) without time zone");
        });

        modelBuilder.Entity<PractitionerService>(e =>
        {
            e.ToTable("practitioner_services");
            e.HasKey(x => new { x.PractitionerId, x.ServiceId });
            e.Property(x => x.PractitionerId).HasColumnName("practitionerId").HasColumnType("integer");
            e.Property(x => x.ServiceId).HasColumnName("serviceId").HasColumnType("integer");

            e.HasOne(x => x.Practitioner)
                .WithMany(p => p.Services)
                .HasForeignKey(x => x.PractitionerId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Service)
                .WithMany(s => s.Practitioners)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Patient>(e =>
        {
            e.ToTable("patients");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").HasColumnType("integer").UseIdentityByDefaultColumn();
            e.Property(x => x.Name).HasColumnName("name").IsRequired();
            e.Property(x => x.Email).HasColumnName("email").IsRequired();
            e.Property(x => x.Phone).HasColumnName("phone").IsRequired();
            e.Property(x => x.CreatedAt).HasColumnName("createdAt").HasColumnType("timestamp(3) without time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
            e.Property(x => x.UpdatedAt).HasColumnName("updatedAt").HasColumnType("timestamp(3) without time zone");

            e.HasIndex(x => x.Email); // no unique — igual que @@index([email]) en Prisma
        });

        modelBuilder.Entity<Appointment>(e =>
        {
            e.ToTable("appointments");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").HasColumnType("integer").UseIdentityByDefaultColumn();
            e.Property(x => x.PatientId).HasColumnName("patientId").HasColumnType("integer");
            e.Property(x => x.ServiceId).HasColumnName("serviceId").HasColumnType("integer");
            e.Property(x => x.PractitionerId).HasColumnName("practitionerId").HasColumnType("integer");
            e.Property(x => x.Date).HasColumnName("date").HasColumnType("date");
            e.Property(x => x.StartTime).HasColumnName("startTime").HasColumnType("timestamp(3) without time zone");
            e.Property(x => x.EndTime).HasColumnName("endTime").HasColumnType("timestamp(3) without time zone");
            // Ignore BaseEntity.Status for Appointment and map the enum property to the existing "status" column
            e.Ignore(x => x.Status);
            e.Property(x => x.AppointmentStatus)
                .HasColumnName("status")
                .HasColumnType("\"AppointmentStatus\"")
                .HasDefaultValueSql("'PENDING'::\"AppointmentStatus\"");
            e.Property(x => x.Notes).HasColumnName("notes");
            e.Property(x => x.CreatedAt).HasColumnName("createdAt").HasColumnType("timestamp(3) without time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
            e.Property(x => x.UpdatedAt).HasColumnName("updatedAt").HasColumnType("timestamp(3) without time zone");

            e.HasOne(x => x.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Service).WithMany(s => s.Appointments)
                .HasForeignKey(x => x.ServiceId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Practitioner).WithMany(p => p.Appointments)
                .HasForeignKey(x => x.PractitionerId).OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => new { x.PractitionerId, x.StartTime }).IsUnique();
            e.HasIndex(x => new { x.Date, x.AppointmentStatus });
            e.HasIndex(x => x.ServiceId);
        });

        modelBuilder.Entity<BusinessHours>(e =>
        {
            e.ToTable("business_hours");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").HasColumnType("integer").UseIdentityByDefaultColumn();
            e.Property(x => x.DayOfWeek).HasColumnName("dayOfWeek");
            e.Property(x => x.IsOpen).HasColumnName("isOpen").HasDefaultValue(true);
            e.Property(x => x.OpenTime).HasColumnName("openTime").IsRequired();
            e.Property(x => x.CloseTime).HasColumnName("closeTime").IsRequired();
            e.Property(x => x.UpdatedAt).HasColumnName("updatedAt").HasColumnType("timestamp(3) without time zone");

            e.HasIndex(x => x.DayOfWeek).IsUnique();
        });

        modelBuilder.Entity<AdminUser>(e =>
        {
            e.ToTable("admin_users");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").HasColumnType("integer").UseIdentityByDefaultColumn();
            e.Property(x => x.Email).HasColumnName("email").IsRequired();
            e.Property(x => x.PasswordHash).HasColumnName("passwordHash").IsRequired();
            e.Property(x => x.Name).HasColumnName("name");
            e.Property(x => x.Rol).HasColumnName("rol").HasDefaultValue("admin").IsRequired();
            e.Property(x => x.CreatedAt).HasColumnName("createdAt").HasColumnType("timestamp(3) without time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
            e.Property(x => x.UpdatedAt).HasColumnName("updatedAt").HasColumnType("timestamp(3) without time zone");

            e.HasIndex(x => x.Email).IsUnique();
        });
    }

    public override int SaveChanges()
    {
        TouchUpdatedAt();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        TouchUpdatedAt();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void TouchUpdatedAt()
    {
        var now = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State != EntityState.Modified) continue;
            var updatedAt = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "UpdatedAt");
            if (updatedAt is not null) updatedAt.CurrentValue = now;
        }
    }
}
