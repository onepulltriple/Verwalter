using AnwendungWPF.Models;

namespace AnwendungWPF.Utilities
{
    public sealed class DeleteCountryConfirmationEventArguments : EventArgs
    {
        public CountryModel Country { get; }

        public bool HasReferences { get; }

        public bool Confirmed { get; set; }

        public DeleteCountryConfirmationEventArguments(
            CountryModel country,
            bool hasReferences)
        {
            Country = country;
            HasReferences = hasReferences;
        }
    }
}