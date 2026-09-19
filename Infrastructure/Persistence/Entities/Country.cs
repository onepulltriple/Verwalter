using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Entities;

public partial class Country
{
    public Guid CountryId { get; set; }

    public string? IsoAlpha2 { get; set; }

    public string? IsoAlpha3 { get; set; }

    public string NameEnGb { get; set; } = null!;

    public bool Active { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime ModifiedUtc { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual ICollection<Postcode> Postcodes { get; set; } = new List<Postcode>();
}
