using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Entities;

public partial class City
{
    public Guid CityId { get; set; }

    public Guid CountryId { get; set; }

    public string NameEnGb { get; set; } = null!;

    public DateTime CreatedUtc { get; set; }

    public DateTime ModifiedUtc { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Place> Places { get; set; } = new List<Place>();
}
