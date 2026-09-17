using CRMSystem.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Organization> Organizations => Set<Organization>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<CustomerContact> CustomerContacts => Set<CustomerContact>();
        public DbSet<CustomerAddress> CustomerAddresses => Set<CustomerAddress>();

        public DbSet<Lead> Leads => Set<Lead>();
        public DbSet<LeadSource> LeadSources => Set<LeadSource>();
        public DbSet<LeadStatus> LeadStatuses => Set<LeadStatus>();

        public DbSet<Pipeline> Pipelines => Set<Pipeline>();
        public DbSet<PipelineStage> PipelineStages => Set<PipelineStage>();
        public DbSet<Opportunity> Opportunities => Set<Opportunity>();

        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<Activity> Activities => Set<Activity>();
        public DbSet<Note> Notes => Set<Note>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<ApplicationUser>(entity =>
            {

                entity.HasOne(x => x.Organization)
                    .WithMany(x => x.Users)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(x => x.OrganizationId);
            });



            builder.Entity<Organization>(entity =>
            {
                entity.ToTable("organizations");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(x => x.Name)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(x => x.Email)
                    .HasMaxLength(150);

                entity.Property(x => x.Phone)
                    .HasMaxLength(50);

                entity.Property(x => x.Address)
                    .HasMaxLength(255);

                entity.Property(x => x.Status)
                    .HasMaxLength(30)
                    .HasDefaultValue("ACTIVE");

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(x => x.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });



            builder.Entity<Customer>(entity =>
            {
                entity.ToTable("customers");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(x => x.CustomerCode)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(x => x.FirstName)
                    .HasMaxLength(100);

                entity.Property(x => x.LastName)
                    .HasMaxLength(100);

                entity.Property(x => x.CompanyName)
                    .HasMaxLength(150);

                entity.Property(x => x.Email)
                    .HasMaxLength(150);

                entity.Property(x => x.Phone)
                    .HasMaxLength(50);

                entity.Property(x => x.Status)
                    .HasMaxLength(30)
                    .HasDefaultValue("ACTIVE");

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(x => x.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(x => x.Organization)
                    .WithMany(x => x.Customers)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.AssignedUser)
                    .WithMany(x => x.Customers)
                    .HasForeignKey(x => x.AssignedUserId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(x => new
                {
                    x.OrganizationId,
                    x.CustomerCode
                }).IsUnique();

                entity.HasIndex(x => x.OrganizationId);

                entity.HasIndex(x => x.AssignedUserId);

                entity.HasIndex(x => x.Email);
            });

            builder.Entity<Customer>()
                .HasIndex(x => x.CustomerCode)
                .IsUnique();


            builder.Entity<CustomerContact>(entity =>
            {
                entity.ToTable("customer_contacts");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(x => x.FirstName)
                    .HasMaxLength(100);

                entity.Property(x => x.LastName)
                    .HasMaxLength(100);

                entity.Property(x => x.Position)
                    .HasMaxLength(100);

                entity.Property(x => x.Email)
                    .HasMaxLength(150);

                entity.Property(x => x.Phone)
                    .HasMaxLength(50);

                entity.HasOne(x => x.Customer)
                    .WithMany(x => x.Contacts)
                    .HasForeignKey(x => x.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });



            builder.Entity<CustomerAddress>(entity =>
            {
                entity.ToTable("customer_addresses");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(x => x.Type)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(x => x.AddressLine)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(x => x.City)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.Province)
                    .HasMaxLength(100);

                entity.Property(x => x.PostalCode)
                    .HasMaxLength(20);

                entity.Property(x => x.Country)
                    .HasMaxLength(100)
                    .HasDefaultValue("Philippines");

                entity.HasOne(x => x.Customer)
                    .WithMany(x => x.Addresses)
                    .HasForeignKey(x => x.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            builder.Entity<LeadSource>(entity =>
            {
                entity.ToTable("lead_sources");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(x => x.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(x => x.Organization)
                    .WithMany(x => x.LeadSources)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(x => new
                {
                    x.OrganizationId,
                    x.Name
                }).IsUnique();
            });


            builder.Entity<LeadStatus>(entity =>
            {
                entity.ToTable("lead_statuses");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(x => x.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasOne(x => x.Organization)
                    .WithMany(x => x.LeadStatuses)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(x => new
                {
                    x.OrganizationId,
                    x.Name
                }).IsUnique();

                entity.HasIndex(x => x.OrganizationId);
            });


            builder.Entity<Lead>(entity =>
            {
                entity.ToTable("leads");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(x => x.FirstName)
                    .HasMaxLength(100);

                entity.Property(x => x.LastName)
                    .HasMaxLength(100);

                entity.Property(x => x.CompanyName)
                    .HasMaxLength(150);

                entity.Property(x => x.Email)
                    .HasMaxLength(150);

                entity.Property(x => x.Phone)
                    .HasMaxLength(50);

                entity.Property(x => x.EstimatedValue)
                    .HasPrecision(18, 2);

                entity.HasOne(x => x.Organization)
                    .WithMany(x => x.Leads)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.AssignedUser)
                    .WithMany(x => x.Leads)
                    .HasForeignKey(x => x.AssignedUserId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(x => x.Source)
                    .WithMany(x => x.Leads)
                    .HasForeignKey(x => x.SourceId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Status)
                    .WithMany(x => x.Leads)
                    .HasForeignKey(x => x.StatusId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.ConvertedCustomer)
                    .WithMany()
                    .HasForeignKey(x => x.ConvertedCustomerId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(x => x.OrganizationId);
                entity.HasIndex(x => x.AssignedUserId);
                entity.HasIndex(x => x.SourceId);
                entity.HasIndex(x => x.StatusId);
                entity.HasIndex(x => x.Email);
            });


            builder.Entity<Pipeline>(entity =>
            {
                entity.ToTable("pipelines");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(x => x.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(x => x.Organization)
                    .WithMany(x => x.Pipelines)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(x => new
                {
                    x.OrganizationId,
                    x.Name
                }).IsUnique();
            });


            builder.Entity<PipelineStage>(entity =>
            {
                entity.ToTable("pipeline_stages");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(x => x.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.Probability)
                    .HasPrecision(5, 2);

                entity.HasOne(x => x.Pipeline)
                    .WithMany(x => x.Stages)
                    .HasForeignKey(x => x.PipelineId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            builder.Entity<Opportunity>(entity =>
            {
                entity.ToTable("opportunities");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(x => x.Name)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(x => x.Value)
                    .HasPrecision(18, 2);

                entity.Property(x => x.Status)
                    .HasMaxLength(30)
                    .HasDefaultValue("OPEN");

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(x => x.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(x => x.Organization)
                    .WithMany(x => x.Opportunities)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Customer)
                    .WithMany(x => x.Opportunities)
                    .HasForeignKey(x => x.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Pipeline)
                    .WithMany(x => x.Opportunities)
                    .HasForeignKey(x => x.PipelineId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Stage)
                    .WithMany(x => x.Opportunities)
                    .HasForeignKey(x => x.StageId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.AssignedUser)
                    .WithMany(x => x.Opportunities)
                    .HasForeignKey(x => x.AssignedUserId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(x => x.OrganizationId);
                entity.HasIndex(x => x.CustomerId);
                entity.HasIndex(x => x.PipelineId);
                entity.HasIndex(x => x.StageId);
                entity.HasIndex(x => x.AssignedUserId);
            });


            builder.Entity<TaskItem>(entity =>
            {
                entity.ToTable("tasks");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(x => x.Title)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(x => x.Priority)
                    .HasMaxLength(30)
                    .HasDefaultValue("MEDIUM");

                entity.Property(x => x.Status)
                    .HasMaxLength(30)
                    .HasDefaultValue("PENDING");

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(x => x.Organization)
                    .WithMany(x => x.Tasks)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.AssignedUser)
                    .WithMany(x => x.Tasks)
                    .HasForeignKey(x => x.AssignedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Customer)
                    .WithMany(x => x.Tasks)
                    .HasForeignKey(x => x.CustomerId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(x => x.Lead)
                    .WithMany(x => x.Tasks)
                    .HasForeignKey(x => x.LeadId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(x => x.Opportunity)
                    .WithMany(x => x.Tasks)
                    .HasForeignKey(x => x.OpportunityId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(x => x.OrganizationId);
                entity.HasIndex(x => x.AssignedUserId);
                entity.HasIndex(x => x.DueDate);
                entity.HasIndex(x => x.Status);
            });



            builder.Entity<Activity>(entity =>
            {
                entity.ToTable("activities");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(x => x.Type)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(x => x.Subject)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(x => x.Organization)
                    .WithMany(x => x.Activities)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.User)
                    .WithMany(x => x.Activities)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Customer)
                    .WithMany(x => x.Activities)
                    .HasForeignKey(x => x.CustomerId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(x => x.Lead)
                    .WithMany(x => x.Activities)
                    .HasForeignKey(x => x.LeadId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(x => x.Opportunity)
                    .WithMany(x => x.Activities)
                    .HasForeignKey(x => x.OpportunityId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(x => x.OrganizationId);
                entity.HasIndex(x => x.UserId);
                entity.HasIndex(x => x.ActivityDate);
            });


            builder.Entity<Note>(entity =>
            {
                entity.ToTable("notes");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(x => x.Content)
                    .IsRequired();

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(x => x.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(x => x.Organization)
                    .WithMany(x => x.Notes)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.User)
                    .WithMany(x => x.Notes)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Customer)
                    .WithMany(x => x.Notes)
                    .HasForeignKey(x => x.CustomerId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(x => x.Lead)
                    .WithMany(x => x.NoteEntries)
                    .HasForeignKey(x => x.LeadId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(x => x.Opportunity)
                    .WithMany(x => x.Notes)
                    .HasForeignKey(x => x.OpportunityId)
                    .OnDelete(DeleteBehavior.SetNull);
            });


            builder.Entity<AuditLog>(entity =>
            {
                entity.ToTable("audit_logs");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(x => x.EntityType)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.Action)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(x => x.Organization)
                    .WithMany(x => x.AuditLogs)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.User)
                    .WithMany(x => x.AuditLogs)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(x => x.OrganizationId);
                entity.HasIndex(x => x.UserId);

                entity.HasIndex(x => new
                {
                    x.EntityType,
                    x.EntityId
                });

                entity.HasIndex(x => x.CreatedAt);
            });

            builder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("refresh_tokens");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(x => x.TokenHash)
                    .IsRequired();

                entity.HasIndex(x => x.TokenHash)
                    .IsUnique();

                entity.HasIndex(x => x.UserId);

                entity.HasOne(x => x.User)
                    .WithMany(x => x.RefreshTokens)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.ReplacedByToken)
                    .WithMany()
                    .HasForeignKey(x => x.ReplacedByTokenId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}