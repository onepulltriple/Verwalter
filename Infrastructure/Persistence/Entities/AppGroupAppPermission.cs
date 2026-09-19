using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Entities;

public partial class AppGroupAppPermission
{
    public Guid AppGroupId { get; set; }

    public Guid AppPermissionId { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual AppGroup AppGroup { get; set; } = null!;

    public virtual AppPermission AppPermission { get; set; } = null!;
}
