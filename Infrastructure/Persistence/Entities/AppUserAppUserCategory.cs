using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Entities;

public partial class AppUserAppUserCategory
{
    public Guid AppUserId { get; set; }

    public Guid AppUserCategoryId { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual AppUser AppUser { get; set; } = null!;

    public virtual AppUserCategory AppUserCategory { get; set; } = null!;
}
