using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Lecturer> Lecturers { get; set; }
    public DbSet<Claim> Claims { get; set; }
    public DbSet<SupportingDocument> SupportingDocuments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Claim>()
            .HasOne(c => c.Lecturer)
            .WithMany()
            .HasForeignKey(c => c.LecturerID);

        modelBuilder.Entity<SupportingDocument>()
            .HasOne(sd => sd.Claim)
            .WithMany()
            .HasForeignKey(sd => sd.ClaimID);
    }
}


