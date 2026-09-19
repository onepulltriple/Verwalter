using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Entities;

public partial class AppUserAddress
{
    public Guid AppUserId { get; set; }

    public Guid AddressId { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Address Address { get; set; } = null!;

    public virtual AppUser AppUser { get; set; } = null!;
}
