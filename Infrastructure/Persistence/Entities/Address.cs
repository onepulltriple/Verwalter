using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Entities;

public partial class Address
{
    public Guid AddressId { get; set; }

    public Guid PlaceId { get; set; }

    public string AddressLine1 { get; set; } = null!;

    public string? AddressLine2 { get; set; }

    public string? AddressLine3 { get; set; }

    public string? Name1 { get; set; }

    public string? Name2 { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime ModifiedUtc { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<AddressAddressCategory> AddressAddressCategories { get; set; } = new List<AddressAddressCategory>();

    public virtual ICollection<AppUserAddress> AppUserAddresses { get; set; } = new List<AppUserAddress>();

    public virtual Place Place { get; set; } = null!;
}
