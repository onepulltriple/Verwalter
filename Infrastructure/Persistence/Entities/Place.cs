using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Entities;

public partial class Place
{
    public Guid PlaceId { get; set; }

    public Guid CountryId { get; set; }

    public Guid PostcodeId { get; set; }

    public Guid CityId { get; set; }

    public string? NameEnGb { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime ModifiedUtc { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual City City { get; set; } = null!;

    public virtual Postcode Postcode { get; set; } = null!;
}
