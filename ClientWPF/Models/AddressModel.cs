using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnwendungWPF.Models
{
    public sealed class AddressModel
    {
        public Guid AddressId { get; init; }

        public Guid PlaceId { get; init; }

        public string AddressLine1 { get; init; } = string.Empty;

        public string? AddressLine2 { get; init; }

        public string? AddressLine3 { get; init; }

        public string? Name1 { get; init; }

        public string? Name2 { get; init; }
    }
}
