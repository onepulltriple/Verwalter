using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Entities;

public partial class AddressDatabaseContext : DbContext
{
    public AddressDatabaseContext()
    {
    }

    public AddressDatabaseContext(DbContextOptions<AddressDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<AddressAddressCategory> AddressAddressCategories { get; set; }

    public virtual DbSet<AddressCategory> AddressCategories { get; set; }

    public virtual DbSet<AppGroup> AppGroups { get; set; }

    public virtual DbSet<AppGroupAppPermission> AppGroupAppPermissions { get; set; }

    public virtual DbSet<AppPermission> AppPermissions { get; set; }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<AppUserAddress> AppUserAddresses { get; set; }

    public virtual DbSet<AppUserAppGroup> AppUserAppGroups { get; set; }

    public virtual DbSet<AppUserAppPermission> AppUserAppPermissions { get; set; }

    public virtual DbSet<AppUserAppUserCategory> AppUserAppUserCategories { get; set; }

    public virtual DbSet<AppUserCategory> AppUserCategories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Place> Places { get; set; }

    public virtual DbSet<Postcode> Postcodes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=AddressDatabase;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).IsClustered(false);

            entity.ToTable("Address", "address");

            entity.HasIndex(e => e.PlaceId, "CX_Address_Place").IsClustered();

            entity.Property(e => e.AddressId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.AddressLine1).HasMaxLength(200);
            entity.Property(e => e.AddressLine2).HasMaxLength(200);
            entity.Property(e => e.AddressLine3).HasMaxLength(200);
            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ModifiedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Name1).HasMaxLength(200);
            entity.Property(e => e.Name2).HasMaxLength(200);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Place).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.PlaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Address_Place");
        });

        modelBuilder.Entity<AddressAddressCategory>(entity =>
        {
            entity.HasKey(e => new { e.AddressCategoryId, e.AddressId });

            entity.ToTable("AddressAddressCategory", "address");

            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.AddressCategory).WithMany(p => p.AddressAddressCategories)
                .HasForeignKey(d => d.AddressCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AddressAddressCategory_AddressCategory");

            entity.HasOne(d => d.Address).WithMany(p => p.AddressAddressCategories)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AddressAddressCategory_Address");
        });

        modelBuilder.Entity<AddressCategory>(entity =>
        {
            entity.ToTable("AddressCategory", "address");

            entity.HasIndex(e => e.Code, "UQ_AddressCategory_Code").IsUnique();

            entity.Property(e => e.AddressCategoryId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ModifiedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.NameEnGb).HasMaxLength(100);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<AppGroup>(entity =>
        {
            entity.ToTable("AppGroup", "security");

            entity.HasIndex(e => e.Code, "UQ_AppGroup_Code").IsUnique();

            entity.Property(e => e.AppGroupId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ModifiedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.NameEnGb).HasMaxLength(100);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<AppGroupAppPermission>(entity =>
        {
            entity.HasKey(e => new { e.AppGroupId, e.AppPermissionId });

            entity.ToTable("AppGroupAppPermission", "security");

            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.AppGroup).WithMany(p => p.AppGroupAppPermissions)
                .HasForeignKey(d => d.AppGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppGroupAppPermission_AppGroup");

            entity.HasOne(d => d.AppPermission).WithMany(p => p.AppGroupAppPermissions)
                .HasForeignKey(d => d.AppPermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppGroupAppPermission_AppPermission");
        });

        modelBuilder.Entity<AppPermission>(entity =>
        {
            entity.ToTable("AppPermission", "security");

            entity.HasIndex(e => e.Code, "UQ_AppPermission_Code").IsUnique();

            entity.Property(e => e.AppPermissionId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ModifiedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.NameEnGb).HasMaxLength(100);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.ToTable("AppUser", "security");

            entity.HasIndex(e => e.Username, "UQ_AppUser_Username").IsUnique();

            entity.Property(e => e.AppUserId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.ModifiedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .UseCollation("Latin1_General_100_CS_AS");
        });

        modelBuilder.Entity<AppUserAddress>(entity =>
        {
            entity.HasKey(e => new { e.AppUserId, e.AddressId });

            entity.ToTable("AppUserAddress", "security");

            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Address).WithMany(p => p.AppUserAddresses)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserAddress_Address");

            entity.HasOne(d => d.AppUser).WithMany(p => p.AppUserAddresses)
                .HasForeignKey(d => d.AppUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserAddress_AppUser");
        });

        modelBuilder.Entity<AppUserAppGroup>(entity =>
        {
            entity.HasKey(e => new { e.AppGroupId, e.AppUserId });

            entity.ToTable("AppUserAppGroup", "security");

            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.AppGroup).WithMany(p => p.AppUserAppGroups)
                .HasForeignKey(d => d.AppGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserAppGroup_AppGroup");

            entity.HasOne(d => d.AppUser).WithMany(p => p.AppUserAppGroups)
                .HasForeignKey(d => d.AppUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserAppGroup_AppUser");
        });

        modelBuilder.Entity<AppUserAppPermission>(entity =>
        {
            entity.HasKey(e => new { e.AppPermissionId, e.AppUserId });

            entity.ToTable("AppUserAppPermission", "security");

            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.AppPermission).WithMany(p => p.AppUserAppPermissions)
                .HasForeignKey(d => d.AppPermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserAppPermission_AppPermission");

            entity.HasOne(d => d.AppUser).WithMany(p => p.AppUserAppPermissions)
                .HasForeignKey(d => d.AppUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserAppPermission_AppUser");
        });

        modelBuilder.Entity<AppUserAppUserCategory>(entity =>
        {
            entity.HasKey(e => new { e.AppUserCategoryId, e.AppUserId });

            entity.ToTable("AppUserAppUserCategory", "security");

            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.AppUserCategory).WithMany(p => p.AppUserAppUserCategories)
                .HasForeignKey(d => d.AppUserCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserAppUserCategory_AppUserCategory");

            entity.HasOne(d => d.AppUser).WithMany(p => p.AppUserAppUserCategories)
                .HasForeignKey(d => d.AppUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserAppUserCategory_AppUser");
        });

        modelBuilder.Entity<AppUserCategory>(entity =>
        {
            entity.ToTable("AppUserCategory", "security");

            entity.HasIndex(e => e.Code, "UQ_AppUserCategory_Code").IsUnique();

            entity.Property(e => e.AppUserCategoryId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ModifiedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.NameEnGb).HasMaxLength(100);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).IsClustered(false);

            entity.ToTable("City", "address");

            entity.HasIndex(e => e.CountryId, "CX_City_Country").IsClustered();

            entity.HasIndex(e => new { e.CountryId, e.CityId }, "UQ_City_Country_City").IsUnique();

            entity.Property(e => e.CityId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ModifiedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.NameEnGb).HasMaxLength(100);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_City_Country");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country", "address");

            entity.HasIndex(e => e.IsoAlpha2, "UX_Country_IsoAlpha2")
                .IsUnique()
                .HasFilter("([IsoAlpha2] IS NOT NULL)");

            entity.HasIndex(e => e.IsoAlpha3, "UX_Country_IsoAlpha3")
                .IsUnique()
                .HasFilter("([IsoAlpha3] IS NOT NULL)");

            entity.Property(e => e.CountryId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsoAlpha2)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.IsoAlpha3)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ModifiedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.NameEnGb).HasMaxLength(100);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Place>(entity =>
        {
            entity.HasKey(e => e.PlaceId).IsClustered(false);

            entity.ToTable("Place", "address");

            entity.HasIndex(e => e.PostcodeId, "CX_Place_PostcodeId").IsClustered();

            entity.HasIndex(e => new { e.CountryId, e.PostcodeId, e.CityId }, "UQ_Place_Country_Postcode_City").IsUnique();

            entity.Property(e => e.PlaceId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ModifiedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.NameEnGb).HasMaxLength(100);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.City).WithMany(p => p.Places)
                .HasPrincipalKey(p => new { p.CountryId, p.CityId })
                .HasForeignKey(d => new { d.CountryId, d.CityId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Place_City");

            entity.HasOne(d => d.Postcode).WithMany(p => p.Places)
                .HasPrincipalKey(p => new { p.CountryId, p.PostcodeId })
                .HasForeignKey(d => new { d.CountryId, d.PostcodeId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Place_Postcode");
        });

        modelBuilder.Entity<Postcode>(entity =>
        {
            entity.HasKey(e => e.PostcodeId).IsClustered(false);

            entity.ToTable("Postcode", "address");

            entity.HasIndex(e => e.CountryId, "CX_Postcode_Country").IsClustered();

            entity.HasIndex(e => new { e.CountryId, e.Postcode1 }, "UQ_Postcode_Country_Postcode").IsUnique();

            entity.HasIndex(e => new { e.CountryId, e.PostcodeId }, "UQ_Postcode_Country_PostcodeId").IsUnique();

            entity.Property(e => e.PostcodeId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ModifiedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Postcode1)
                .HasMaxLength(50)
                .HasColumnName("Postcode");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Country).WithMany(p => p.Postcodes)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Postcode_Country");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
