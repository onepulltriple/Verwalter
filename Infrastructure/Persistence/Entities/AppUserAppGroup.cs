using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Entities;

public partial class AppUserAppGroup
{
    public Guid AppUserId { get; set; }

    public Guid AppGroupId { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual AppGroup AppGroup { get; set; } = null!;

    public virtual AppUser AppUser { get; set; } = null!;
}
