using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Entities;

public partial class AddressAddressCategory
{
    public Guid AddressId { get; set; }

    public Guid AddressCategoryId { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Address Address { get; set; } = null!;

    public virtual AddressCategory AddressCategory { get; set; } = null!;
}
