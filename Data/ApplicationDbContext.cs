using CampusPlacementManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace CampusPlacementManagement.Data;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<College> Colleges => Set<College>();

    public DbSet<PlacementOfficer> PlacementOfficers
        => Set<PlacementOfficer>();

    public DbSet<Company> Companies => Set<Company>();

    public DbSet<CompanyHR> CompanyHRs => Set<CompanyHR>();

    public DbSet<Student> Students => Set<Student>();

    public DbSet<JobOpening> JobOpenings => Set<JobOpening>();

    public DbSet<JobTargetCollege> JobTargetColleges
        => Set<JobTargetCollege>();

    public DbSet<JobApplication> JobApplications
        => Set<JobApplication>();

    public DbSet<ApplicationStatusHistory> ApplicationStatusHistories
        => Set<ApplicationStatusHistory>();

    public DbSet<RecruitmentDrive> RecruitmentDrives
        => Set<RecruitmentDrive>();

    public DbSet<InterviewRound> InterviewRounds
        => Set<InterviewRound>();

    public DbSet<InterviewSchedule> InterviewSchedules
        => Set<InterviewSchedule>();





    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);





        // College

        builder.Entity<College>()
            .HasIndex(c => c.CollegeCode)
            .IsUnique();

        builder.Entity<College>()
            .Property(c => c.CollegeName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Entity<College>()
            .Property(c => c.CollegeCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.Entity<College>()
            .Property(c => c.OfficialEmail)
            .HasMaxLength(255)
            .IsRequired();

        // Placement Officer

        builder.Entity<PlacementOfficer>()
            .HasOne(p => p.User)
            .WithOne()
            .HasForeignKey<PlacementOfficer>(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<PlacementOfficer>()
            .HasOne(p => p.College)
            .WithMany(c => c.PlacementOfficers)
            .HasForeignKey(p => p.CollegeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<PlacementOfficer>()
            .HasIndex(p => p.UserId)
            .IsUnique();

        // Company HR

        builder.Entity<CompanyHR>()
            .HasOne(h => h.User)
            .WithOne()
            .HasForeignKey<CompanyHR>(h => h.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<CompanyHR>()
            .HasOne(h => h.Company)
            .WithMany(c => c.CompanyHRs)
            .HasForeignKey(h => h.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<CompanyHR>()
            .HasIndex(h => h.UserId)
            .IsUnique();

        // Student

        builder.Entity<Student>()
            .HasOne(s => s.User)
            .WithOne()
            .HasForeignKey<Student>(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Student>()
            .HasOne(s => s.College)
            .WithMany(c => c.Students)
            .HasForeignKey(s => s.CollegeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Student>()
            .HasIndex(s => new
            {
                s.CollegeId,
                s.RegisterNumber
            })
            .IsUnique();

        builder.Entity<Student>()
            .HasIndex(s => s.UserId)
            .IsUnique();

        builder.Entity<Student>()
            .Property(s => s.CGPA)
            .HasPrecision(4, 2);

        // Company

        builder.Entity<Company>()
            .Property(c => c.CompanyName)
            .HasMaxLength(200)
            .IsRequired();

        // Job Opening

        builder.Entity<JobOpening>(entity =>
        {
            entity.HasKey(j => j.JobOpeningId);

            entity.HasOne(j => j.Company)
                .WithMany(c => c.JobOpenings)
                .HasForeignKey(j => j.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(j => j.SalaryPackage)
                .HasPrecision(12, 2);

            entity.Property(j => j.MinimumCGPA)
                .HasPrecision(4, 2);

            // Job Target College

            builder.Entity<JobTargetCollege>()
            .HasOne(t => t.Job)
            .WithMany(j => j.TargetColleges)
            .HasForeignKey(t => t.JobId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<JobTargetCollege>()
                .HasOne(t => t.College)
                .WithMany(c => c.JobTargetColleges)
                .HasForeignKey(t => t.CollegeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<JobTargetCollege>()
                .HasIndex(t => new
                {
                    t.JobId,
                    t.CollegeId
                })
                .IsUnique();

            // Job Application

            builder.Entity<JobApplication>()
                .HasOne(a => a.Student)
                .WithMany(s => s.JobApplications)
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<JobApplication>()
                .HasOne(a => a.Job)
                .WithMany(j => j.JobApplications)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<JobApplication>()
                .HasIndex(a => new
                {
                    a.StudentId,
                    a.JobId
                })
                .IsUnique();

            // Application Status History

            builder.Entity<ApplicationStatusHistory>()
                .HasOne(h => h.JobApplication)
                .WithMany(a => a.StatusHistory)
                .HasForeignKey(h => h.JobApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Recruitment Drive

            builder.Entity<RecruitmentDrive>()
                .HasOne(d => d.Job)
                .WithMany()
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RecruitmentDrive>()
                .HasOne(d => d.College)
                .WithMany(c => c.RecruitmentDrives)
                .HasForeignKey(d => d.CollegeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Interview Round

            builder.Entity<InterviewRound>()
                .HasOne(r => r.RecruitmentDrive)
                .WithMany(d => d.InterviewRounds)
                .HasForeignKey(r => r.RecruitmentDriveId)
                .OnDelete(DeleteBehavior.Restrict);

            // Interview Schedule

            builder.Entity<InterviewSchedule>()
                .HasOne(i => i.JobApplication)
                .WithMany(a => a.InterviewSchedules)
                .HasForeignKey(i => i.JobApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<InterviewSchedule>()
                .HasOne(i => i.InterviewRound)
                .WithMany(r => r.InterviewSchedules)
                .HasForeignKey(i => i.InterviewRoundId)
                .OnDelete(DeleteBehavior.Restrict);
        });


    }
} 