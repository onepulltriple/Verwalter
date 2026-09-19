using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Entities;

public partial class AppUserAppPermission
{
    public Guid AppUserId { get; set; }

    public Guid AppPermissionId { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual AppPermission AppPermission { get; set; } = null!;

    public virtual AppUser AppUser { get; set; } = null!;
}
