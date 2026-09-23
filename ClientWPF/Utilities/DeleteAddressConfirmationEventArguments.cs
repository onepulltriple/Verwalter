using AnwendungWPF.Models;

namespace AnwendungWPF.Utilities
{
    public sealed class DeleteAddressConfirmationEventArguments : EventArgs
    {
        public AddressModel Address { get; }

        public bool HasReferences { get; }

        public bool Confirmed { get; set; }

        public DeleteAddressConfirmationEventArguments(
            AddressModel address,
            bool hasReferences)
        {
            Address = address;
            HasReferences = hasReferences;
        }
    }
}
