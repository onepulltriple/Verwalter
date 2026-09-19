using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Entities;

public partial class AppPermission
{
    public Guid AppPermissionId { get; set; }

    public string Code { get; set; } = null!;

    public string NameEnGb { get; set; } = null!;

    public string? Description { get; set; }

    public bool Active { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime ModifiedUtc { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<AppGroupAppPermission> AppGroupAppPermissions { get; set; } = new List<AppGroupAppPermission>();

    public virtual ICollection<AppUserAppPermission> AppUserAppPermissions { get; set; } = new List<AppUserAppPermission>();
}
