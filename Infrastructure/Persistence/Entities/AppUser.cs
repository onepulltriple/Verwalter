using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Entities;

public partial class AppUser
{
    public Guid AppUserId { get; set; }

    public string Username { get; set; } = null!;

    public string? EmailAddress { get; set; }

    public string PasswordHash { get; set; } = null!;

    public bool Active { get; set; }

    public DateTime? LastLoginUtc { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime ModifiedUtc { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<AppUserAddress> AppUserAddresses { get; set; } = new List<AppUserAddress>();

    public virtual ICollection<AppUserAppGroup> AppUserAppGroups { get; set; } = new List<AppUserAppGroup>();

    public virtual ICollection<AppUserAppPermission> AppUserAppPermissions { get; set; } = new List<AppUserAppPermission>();

    public virtual ICollection<AppUserAppUserCategory> AppUserAppUserCategories { get; set; } = new List<AppUserAppUserCategory>();
}
